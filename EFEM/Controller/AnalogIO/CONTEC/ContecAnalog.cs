using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrameOfSystem3.Controller.AnalogIO.Contec.CaioCs;

namespace FrameOfSystem3.Controller.AnalogIO.Contec
{
	/// <summary>
	/// Device Name : AIO000
	/// Device : AIO-160802AY-USB
	/// </summary>

	public enum EN_ANALOG_WAVE_FORM
	{
		SINE_WAVE,
		SQUARE_WAVE,
	}

	public class ContecAnalog
	{
		short m_nDeviceID;
		//short[] m_arrAIRange;		// 2022.07.28 by junho [DEL] not used

		// 2022.07.28 by junho [MOD] data type change
		//float[] m_arrAOValue;
		int[] m_arrAOValue;

		int m_nInModuleCount;
		int m_nOutModuleCount;
		int m_nInChannelCount;
		int m_nOutChannelCount;
		private Caio m_module;
		private static ContecAnalog _Instance = null;

		#region 생성자
		private ContecAnalog()
		{
			unsafe
			{
				pdelegate_func = new PAOCALLBACK(CallBackProc);
				gCh = System.Runtime.InteropServices.GCHandle.Alloc(pdelegate_func);
			}
			m_module = new Caio();
		}
		~ContecAnalog()
		{
			gCh.Free();
		}
		public static ContecAnalog GetInstance()
		{
			if (_Instance == null)
			{
				_Instance = new ContecAnalog();
			}
			return _Instance;
		}
		#endregion

		#region 속성
		public int InModuleCount
		{
			get
			{
				return m_nInModuleCount;
			}
		}
		public int OutModuleCount
		{
			get
			{
				return m_nOutModuleCount;
			}
		}
		public int InChannelCount
		{
			get
			{
				return m_nInChannelCount;
			}
		}
		public int OutChannelCount
		{
			get
			{
				return m_nOutChannelCount;
			}
		}
		#endregion

		// 2021.09.14 hkyu [ADD]
		private bool m_bInit = false;
		public bool bInitAIO
		{
			get
			{
				return m_bInit;
			}
			set
			{
				m_bInit = value;
			}
		}

		public bool InitController(out string sResult)
		{
			int nErrorCode;
			sResult = "";
			bInitAIO = false;


			string readId = Work.AppConfigManager.Instance.ContecId;
			nErrorCode = m_module.Init(readId, out m_nDeviceID);

			if (nErrorCode != 0)
			{
				m_module.GetErrorString(nErrorCode, out sResult);
				return false;
			}
			else
			{
				m_nInModuleCount = 1; m_nInChannelCount = 8;
				m_nOutModuleCount = 1; m_nOutChannelCount = 8;
				//m_arrAIRange = new short[m_nInChannelCount];	// 2022.07.28 by junho [DEL] not used
				m_arrAOValue = new int[m_nOutChannelCount];
				//for (short nChannel = 0; nChannel < 8; nChannel++)
				//{
				//	m_module.GetAiRange(m_nDeviceID, nChannel, out m_arrAIRange[nChannel]);	// 2022.07.28 by junho [DEL] not used
				//}
				m_Freq = new CFrequency[m_nOutChannelCount];

				//メモリのリセット
				m_module.ResetAoMemory(m_nDeviceID);

				bInitAIO = true;  // 2021.09.14 hkyu [ADD]

				return true;
			}
		}
		public void CloseController()
		{
			if (m_module != null)
			{
				m_module.Exit(m_nDeviceID);
				//m_arrAIRange = null;	// 2022.07.28 by junho [DEL] not used
				m_arrAOValue = null;
			}
		}
		public bool ReadInput(int nChannel, out int nCount)
		{
			int nErrorCode;
			nErrorCode = m_module.SingleAi(m_nDeviceID, (short)nChannel, out nCount);
			if (nErrorCode != 0) return false;
			else return true;
		}
		// 2022.07.28 by junho [MOD] (double)Volt -> (int)Count
		public bool ReadAllInput(int nChannel, int nCountOfChannel, out int[] arCount)
		{
			int nErrorCode = 0;
			//int nReadCount;
			arCount = new int[nCountOfChannel];

			nErrorCode = m_module.MultiAi(m_nDeviceID, (short)nCountOfChannel, arCount);
			if (nErrorCode != 0) return false;

			//for (int i = 0; i < nCountOfChannel; i++)
			//{
			//	lock (_lock)
			//	{
			//		nErrorCode = m_module.SingleAi(m_nDeviceID, (short)i, out nReadCount);
			//		if (nErrorCode != 0) return false;

			//		arCount[i] = nReadCount;
			//	}
			//}

			return true;
		}
		// 2022.07.28 by junho [MOD] (double)Volt -> (int)Count
		public bool ReadOutput(int nChannel, out int nCount)
		{
			nCount = 0;

			if (m_arrAOValue == null) return false;
			else if (nChannel >= m_nOutChannelCount) return false;

			nCount = m_arrAOValue[nChannel];
			return true;
		}
		// 2022.07.28 by junho [MOD] (double)Volt -> (int)Count
		public bool ReadAllOutput(int nChannel, int nCountOfChannel, out int[] arCount)
		{
			int nErrorCode = 0;
			int nSavedCount;
			arCount = new int[nCountOfChannel];

			for (int i = 0; i < nCountOfChannel; i++)
			{
				nSavedCount = m_arrAOValue[i];
				arCount[i] = nSavedCount;
				if (nErrorCode != 0) return false;
			}

			if (nErrorCode != 0) return false;
			else return true;
		}
		// 2022.07.28 by junho [MOD] (double)Volt -> (int)Count
		public bool WriteOutput(int nChannel, int nCount)
		{
			int nErrorCode;

			// Momory reset
			nErrorCode = m_module.ResetAoMemory(m_nDeviceID);
			if (nErrorCode != 0)
			{
				String ErrorString;
				m_module.GetErrorString(nErrorCode, out ErrorString);
			}
			nErrorCode = m_module.SingleAo(m_nDeviceID, (short)nChannel, nCount);
			if (nErrorCode != 0) return false;
			else
			{
				m_arrAOValue[nChannel] = nCount;
				return true;
			}
		}

		#region <OnOff> //2021.03.22 add
		System.Runtime.InteropServices.GCHandle gCh;
		PAOCALLBACK pdelegate_func;

		private class CFrequency
		{
			public bool flag;
			public int lChannelNo;
			public double lo;
			public double hi;
			public double frequency;
			public EN_ANALOG_WAVE_FORM wave;
			//public int DATA_MAX = 16000;
			public int AoSamplingTimes;
			private List<float> listAoData = new List<float>();
			public CFrequency(EN_ANALOG_WAVE_FORM _wave, double vLo, double vHi, double _freq, int _lChannelNo)
			{
				wave = _wave;
				lo = vLo;
				hi = vHi;
				frequency = _freq;
				lChannelNo = _lChannelNo;

				listAoData.Clear();

				double range = hi - lo;
				double bias = (hi + lo) / 2.0;

				//周波数
				AoSamplingTimes = (int)(1000 / frequency);
				//出力サンプリング回数がデータ格納配列サイズを上回らないよう調整
				//if (AoSamplingTimes * 1 > DATA_MAX)
				//{
				//    DATA_MAX = AoSamplingTimes * 1;
				//}

				//出力データ作成：サイン波形
				int i;
				float AoVolt;
				for (i = 0; i < AoSamplingTimes; i++)
				{
					switch (wave)
					{
						case EN_ANALOG_WAVE_FORM.SINE_WAVE: //正弦波
							{
								double sinDat = Math.Sin((2 * Math.PI * i / AoSamplingTimes) + (3.0 / 4.0 * 2.0 * Math.PI));
								AoVolt = (float)((range * sinDat) + bias);
							}
							break;
						case EN_ANALOG_WAVE_FORM.SQUARE_WAVE: //矩形波
							{
								int half = AoSamplingTimes / 2;
								AoVolt = (float)((i < half) ? (lo) : (hi));
							}
							break;
						default:
							continue;
					}
					listAoData.Add(AoVolt);
				}

				flag = true;
			}

			public float[] GetAoDatas()
			{
				float[] AoData = new float[AoSamplingTimes];

				for (int i = 0; i < AoSamplingTimes; i++)
				{
					AoData[i] = listAoData[i];
				}
				return AoData;
			}
			public float GetAoData(int index)
			{
				if (0 <= index &&
					index < listAoData.Count)
				{
					return listAoData[index];
				}
				return 0.0F;
			}
		}
		private CFrequency[] m_Freq;
		public bool WriteWaveFormOutputStartVoltage(int lChannelNo, EN_ANALOG_WAVE_FORM wave, int vLo, int vHi, double freq)
		{
			m_Freq[lChannelNo] = new CFrequency(wave, vLo, vHi, freq, lChannelNo);
			m_arrAOValue[lChannelNo] = vHi;

			int Ret;
			string ErrorString;

			//デバイスのリセット、ドライバの初期化を行います。
			Ret = m_module.ResetDevice(m_nDeviceID);
			if (Ret != 0)
			{
				m_module.GetErrorString(Ret, out ErrorString);
				//label_Information.Text = "aio.SetAoChannels = " + Ret.ToString() + " : " + ErrorString;
				return false;
			}

			//チャネル数の設定：1チャネル
			short m_AoChannels = (short)(lChannelNo + 1);
			Ret = m_module.SetAoChannels(m_nDeviceID, m_AoChannels);
			if (Ret != 0)
			{
				m_module.GetErrorString(Ret, out ErrorString);
				//label_Information.Text = "aio.SetAoChannels = " + Ret.ToString() + " : " + ErrorString;
				return false;
			}

			//メモリ形式の設定：RING
			Ret = m_module.SetAoMemoryType(m_nDeviceID, 1);
			if (Ret != 0)
			{
				m_module.GetErrorString(Ret, out ErrorString);
				//label_Information.Text = "aio.SetAoMemoryType = " + Ret.ToString() + " : " + ErrorString;
				return false;
			}

			//クロック種類の設定：内部
			Ret = m_module.SetAoClockType(m_nDeviceID, 0);
			if (Ret != 0)
			{
				m_module.GetErrorString(Ret, out ErrorString);
				//label_Information.Text = "aio.SetAoClockType = " + Ret.ToString() + " : " + ErrorString;
				return false;
			}

			//変換速度の設定：1000usec
			//Ret = aio.SetAoSamplingClock(m_Id, 1000);
			Ret = m_module.SetAoSamplingClock(m_nDeviceID, 1000);
			/*double frequency = 1;
			double.TryParse( textBox_Frequency.Text, out frequency );
			double clk = 1.0 / frequency;
			float fClk = (float)(clk*1000.0*1000.0/1000.0);
			int iClk = (int)(fClk / 0.025);
			fClk = (float)(0.025 * iClk);
			Ret = aio.SetAoSamplingClock(m_Id, fClk);*/
			if (Ret != 0)
			{
				m_module.GetErrorString(Ret, out ErrorString);
				//label_Information.Text = "aio.SetAoSamplingClock = " + Ret.ToString() + " : " + ErrorString;
				return false;
			}

			//開始条件の設定：ソフトウェア
			Ret = m_module.SetAoStartTrigger(m_nDeviceID, 0);
			if (Ret != 0)
			{
				m_module.GetErrorString(Ret, out ErrorString);
				//label_Information.Text = "aio.SetAoStartTrigger = " + Ret.ToString() + " : " + ErrorString;
				return false;
			}

			//停止条件の設定：設定回数変換終了
			Ret = m_module.SetAoStopTrigger(m_nDeviceID, 0);
			if (Ret != 0)
			{
				m_module.GetErrorString(Ret, out ErrorString);
				//label_Information.Text = "aio.SetAoStopTrigger = " + Ret.ToString() + " : " + ErrorString;
				return false;
			}

			//メモリのリセット
			Ret = m_module.ResetAoMemory(m_nDeviceID);
			if (Ret != 0)
			{
				m_module.GetErrorString(Ret, out ErrorString);
				//label_Information.Text = "aio.ResetAoMemory = " + Ret.ToString() + " : " + ErrorString;
				return false;
			}

			//出力データ、サンプリング回数の設定：1000回
			float[] AoData;
			int samplingNum = m_Freq[lChannelNo].AoSamplingTimes;
			if (m_AoChannels <= 1)
			{
				AoData = m_Freq[lChannelNo].GetAoDatas();
			}
			else
			{
				int cnt = m_Freq[lChannelNo].AoSamplingTimes;
				AoData = new float[samplingNum * m_AoChannels];
				for (int i = 0; i < cnt; i++)
				{
					for (int j = 0; j < m_AoChannels; j++)
					{
						AoData[i * m_AoChannels + j] = m_Freq[lChannelNo].GetAoData(i);
					}
				}
			}
			Ret = m_module.SetAoSamplingDataEx(m_nDeviceID, samplingNum, AoData);
			if (Ret != 0)
			{
				m_module.GetErrorString(Ret, out ErrorString);
				//label_Information.Text = "aio.SetAoSamplingDataEx = " + Ret.ToString() + " : " + ErrorString;
				return false;
			}

			//リピート回数：無限
			Ret = m_module.SetAoRepeatTimes(m_nDeviceID, 0);
			if (Ret != 0)
			{
				m_module.GetErrorString(Ret, out ErrorString);
				//label_Information.Text = "aio.SetAoRepeatTimes = " + Ret.ToString() + " : " + ErrorString;
				return false;
			}

			//イベント要因の設定：デバイス動作終了イベント
			//Ret = m_module.SetAoEvent(m_nDeviceID, (uint)Handle, (int)CONTEC_ANALOG.CaioConst.AIE_END);
			//if (Ret != 0)
			//{
			//    m_module.GetErrorString(Ret, out ErrorString);
			//    label_Information.Text = "aio.SetAoEvent = " + Ret.ToString() + " : " + ErrorString;
			//    return false;
			//}
			unsafe
			{
				//IntPtr pfunc = System.Runtime.InteropServices.Marshal.GetFunctionPointerForDelegate(pdelegate_func);
				//void *パラメータ渡しテスト
				//fixed (int* Temp = &m_Freq[lChannelNo].lChannelNo)

				//Ret = m_module.SetAoCallBackProc(m_nDeviceID, pdelegate_func, (int)CONTEC_ANALOG.CaioConst.AOE_END, Temp);

				IntPtr ipFunc = System.Runtime.InteropServices.Marshal.GetFunctionPointerForDelegate(pdelegate_func);
				Ret = m_module.SetAoCallBackProc(m_nDeviceID, ipFunc, (int)CaioConst.AOE_END, null);
			}
			if (Ret != 0)
			{
				m_module.GetErrorString(Ret, out ErrorString);
				//label_Information.Text = "aio.SetAoCallBackProc = " + Ret.ToString() + " : " + ErrorString;
				return false;
			}

			//label_Information.Text = "変換条件設定 : 正常終了";

			//リレーON
			//Ret = m_module.EnableAo(m_nDeviceID, (short)lChannelNo);
			//if (Ret != 0)
			//{
			//    m_module.GetErrorString(Ret, out ErrorString);
			//    //label_Information.Text = "aio.StartAo = " + Ret.ToString() + " : " + ErrorString;
			//    return false;
			//}

			//変換開始
			Ret = m_module.StartAo(m_nDeviceID);
			if (Ret != 0)
			{
				m_module.GetErrorString(Ret, out ErrorString);
				//label_Information.Text = "aio.StartAo = " + Ret.ToString() + " : " + ErrorString;
				return false;
			}

			//label_Information.Text = "変換開始：正常終了";
			return true;
		}

		public bool WriteWaveFormOutputStop(int lChannelNo, bool immStop, ref double dblVolt)
		{
			//変換停止
			string ErrorString;
			int Ret;
			if (immStop == false)
			{
				if (m_Freq[lChannelNo] != null &&
					m_Freq[lChannelNo].flag &&
					0.0 < m_Freq[lChannelNo].frequency)
				{
					DateTime tmStart = DateTime.Now;
					double ms = 1000.0 / m_Freq[lChannelNo].frequency;
					int AoSamplingCount;
					while (IsWaveFormOutput(lChannelNo))
					{
						//出力済サンプリング数の取得
						Ret = m_module.GetAoSamplingCount(m_nDeviceID, out AoSamplingCount);
						if (Ret != 0)
						{
							m_module.GetErrorString(Ret, out ErrorString);
							//label_Information.Text = "aio.GetAoSamplingCount = " + Ret.ToString() + " : " + ErrorString;
							break;
						}
						if (AoSamplingCount == 0)
						{
							break;
						}
						DateTime cur = DateTime.Now;
						TimeSpan ts = (cur - tmStart);
						if ((ms * 2.0) < ts.TotalMilliseconds)
						{
							break;
						}
					}
				}
			}
			else
			{
				if (IsWaveFormOutput(lChannelNo))
				{
					if (m_Freq[lChannelNo] != null &&
						m_Freq[lChannelNo].flag)
					{
						//出力済サンプリング数の取得
						int AoSamplingCount;
						Ret = m_module.GetAoSamplingCount(m_nDeviceID, out AoSamplingCount);
						if (Ret != 0)
						{
							m_module.GetErrorString(Ret, out ErrorString);
							//label_Information.Text = "aio.GetAoSamplingCount = " + Ret.ToString() + " : " + ErrorString;
						}
						else
						{
							dblVolt = m_Freq[lChannelNo].GetAoData(AoSamplingCount);
						}
					}
					else
					{
						dblVolt = 0.0;
					}
				}
				else
				{
					dblVolt = 0.0;
				}
			}
			m_Freq[lChannelNo] = null;
			Ret = m_module.StopAo(m_nDeviceID);
			if (Ret != 0)
			{
				m_module.GetErrorString(Ret, out ErrorString);
				//label_Information.Text = "aio.StopAo = " + Ret.ToString() + " : " + ErrorString;
				//メモリのリセット
				m_module.ResetAoMemory(m_nDeviceID);
				return false;
			}
			//メモリのリセット
			Ret = m_module.ResetAoMemory(m_nDeviceID);
			if (Ret != 0)
			{
				m_module.GetErrorString(Ret, out ErrorString);
				//label_Information.Text = "aio.ResetAoMemory = " + Ret.ToString() + " : " + ErrorString;
				return false;
			}
			return true;
		}
		public bool IsWaveFormOutput(int lChannelNo)
		{
			if (bInitAIO == false) return false;  // 2021.09.14. hkyu [ADD]

			if (m_Freq[lChannelNo] != null &&
				m_Freq[lChannelNo].flag)
			{
				return true;
			}
			return false;
		}
		public bool IsWaveFormOutputCheck(int lChannelNo)
		{
			if (bInitAIO == false) return false;  // 2021.09.14. hkyu [ADD]

			//ステータスの取得
			int AoStatus;
			int Ret = m_module.GetAoStatus(m_nDeviceID, out AoStatus);
			if (Ret != 0)
			{
				return false;
			}
			if ((AoStatus & (int)CaioConst.AOS_BUSY) != 0)
			{
				return true;
			}
			return false;
		}
		public bool IsSameWaveFormOutput(int lChannelNo, EN_ANALOG_WAVE_FORM wave, double vLo, double vHi, double freq)
		{
			if (IsWaveFormOutput(lChannelNo))
			{
				CFrequency pFreq = m_Freq[lChannelNo];
				if (pFreq.wave == wave &&
					pFreq.lo == vLo &&
					pFreq.hi == vHi &&
					pFreq.frequency == freq)
				{
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// コールバックルーチン
		/// </summary>
		unsafe public int CallBackProc(short Id, short Message, int wParam, int lParam, void* Param)
		{
			if (Message == (short)CaioConst.AIOM_AOE_END)
			{
				/*int Ret;
				string ErrorString;
                
				//delegade処理。"デバイス動作終了イベント発生"のメッセージを出力します。
				this.Invoke(new SetMessageDelegate(SetMessageText), new Object[] { });

				//出力済サンプリング数の取得
				int AoSamplingCount;
				Ret = aio.GetAoSamplingCount(m_Id, out AoSamplingCount);
				if (Ret != 0)
				{
					aio.GetErrorString(Ret, out ErrorString);
					label_Information.Text = "aio.GetAoSamplingCount = " + Ret.ToString() + " : " + ErrorString;
					return 0;
				}
				//delegade処理。出力済サンプリング数を出力します。
				this.Invoke(new SetMessageItemOutDelegate(SetMessageItemOutText), new Object[] { AoSamplingCount });

				//ステータスの取得
				int AoStatus;
				Ret = aio.GetAoStatus(m_Id, out AoStatus);
				if (Ret != 0)
				{
					aio.GetErrorString(Ret, out ErrorString);
					label_Information.Text = "aio.GetAoStatus = " + Ret.ToString() + " : " + ErrorString;
					return 0;
				}
				//delegade処理。ステータス出力します。
				this.Invoke(new SetMessageStatusOutDelegate(SetMessageStatusOutText), new Object[] { AoStatus });

				//メモリ内未出力サンプリング回数の取得
				int AoSamplingTimes;
				Ret = aio.GetAoSamplingTimes(m_Id, out AoSamplingTimes);
				if (Ret != 0)
				{
					aio.GetErrorString(Ret, out ErrorString);
					label_Information.Text = "aio.GetAoSamplingTimes = " + Ret.ToString() + " : " + ErrorString;
					return 0;
				}

				//delegade処理。メモリ内未出力サンプリング回数を出力します。
				this.Invoke(new SetOutDataDelegate(SetOutDataText), new Object[] { AoSamplingTimes });
				*/
			}
			return 0;
		}
		#endregion </OnOff>
	}
}
