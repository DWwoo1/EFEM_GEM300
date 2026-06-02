using System;
using System.Text;
using System.Runtime.InteropServices;

namespace MKUNIT
{
    #region <MKUNIT DLL IMPORT>
    
    public static class MkUnitw
    {

        /* ---------- USPG-46 互換部分 初期化--------- */
        // DLL をロード
        /// <summary>
        /// USPG-46 + USIO-32 : USPG-46 DLL 로드, MK-UNIT : MK-UNIT DLL 로드
        /// </summary>
        /// <returns></returns>
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wDllOpen();
        // DLL をアンロード
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wDllClose();

        // デバイスの使用を宣言
        /// <summary>
        /// 각 BSN 사용 선언 (USPG-46 + USIO-32 : 0~15, MK-UNIT : 0~7)
        /// </summary>
        /// <param name="wBsn"></param>
        /// <returns></returns>
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wCreate(ref short wBsn);
        // デバイスの解放
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wClose(short wBsn);

        /* ---------- USPG-46 互換部分 情報取得--------- */
        // DLL のバージョン取得
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wGetLibVersion(StringBuilder pbLibVersion);
        // デバドラのバージョン取得
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wGetDrvVersion(StringBuilder pbDrvVersion);
        // ROMのバージョン取得
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wGetRomVersion(short wBsn, StringBuilder pbDrvVersion);
        // デバイスの状態を取得
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wDeviceStatus(short wBsn, ref byte pbStatus);
        // エラーコード取得
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wGetLastError(short wBsn);

        /* ---------- USPG-46 互換部分 入出力取得--------- */
        // 読み込み実行
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wInPort(short wBsn, short wAxis, ushort wPort, ref byte pbData);
        // 書き込み実行
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wOutPort(short wBsn, short wAxis, ushort wPort, byte bData);
        // ドライブステータス取得
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wGetDriveStatus(short wBsn, short wAxis, ref byte pbStatus);
        // エンドステータス取得
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wGetEndStatus(short wBsn, short wAxis, ref byte pbStatus);
        // メカニカルシグナル取得
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wGetMechanicalSignal(short wBsn, short wAxis, ref byte pbSignal);
        // ユニバーサルシグナル取得
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wGetUniversalSignal(short wBsn, short wAxis, ref byte pbSignal);
        // モード１書き込み
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wMode1Write(short wBsn, short wAxis, byte bMode);
        // モード２書き込み
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wMode2Write(short wBsn, short wAxis, byte bMode);
        // ユニバーサルシグナル書き込み
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wUniversalSignalWrite(short wBsn, short wAxis, byte bSignal);
        // ２バイトデータ読み込み
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wDataHalfRead(short wBsn, short wAxis, byte bCmd, ref short pwData);
        // ４バイトデータ読み込み
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wDataFullRead(short wBsn, short wAxis, byte bCmd, ref int pdwData);
        // コマンド書き込み
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wCommandWrite(short wBsn, short wAxis, byte bCmd);
        // ２バイトデータ書き込み
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wDataHalfWrite(short wBsn, short wAxis, byte bCmd, short wData);
        // ４バイトデータ書き込み
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wDataFullWrite(short wBsn, short wAxis, byte bCmd, int dwData);
        // 全軸のステータスを取得
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wGetAxisStatus(short wBsn, ref byte pbData);
        // 指定軸の各ステータスを取得
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wGetAxisAllPort(short wBsn, short wAxis, ref uint pdwData);

        /* ---------- USPG-46 互換部分 入出力取得2--------- */
        // インターナルカウンター取得
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wGetInternalCounter(short wBsn, short wAxis, ref int pdwData);
        // エクスターナルカウンター取得
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wGetExternalCounter(short wBsn, short wAxis, ref int pdwData);
		// SPEED DATA取得
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wGetNowSpeedData(short wBsn, short wAxis, ref short pwData);
        // ドライブパルスカウンター取得
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wGetDrivePulseCounter(short wBsn, short wAxis, ref uint pdwData);

        /* ---------- USPG-46 互換部分 入出力取得3--------- */
        // ドライブステータス取得
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wGetDriveStatusW(short wBsn, short wAxis, ref byte pbStatus);
        // エンドステータス取得
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wGetEndStatusW(short wBsn, short wAxis, ref byte pbStatus);
        // メカニカルシグナル取得
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wGetMechanicalSignalW(short wBsn, short wAxis, ref byte pbSignal);
        // ユニバーサルシグナル取得
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wGetUniversalSignalW(short wBsn, short wAxis, ref byte pbSignal);
        // インターナルカウンター取得
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wGetInternalCounterW(short wBsn, short wAxis, ref int pdwData);
        // エクスターナルカウンター取得
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wGetExternalCounterW(short wBsn, short wAxis, ref int pdwData);
		// SPEED DATA取得
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wGetNowSpeedDataW(short wBsn, short wAxis, ref short pwData);
        // ドライブパルスカウンター取得
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wGetDrivePulseCounterW(short wBsn, short wAxis, ref uint pdwData);

        /* ---------- USPG-46 互換部分 拡張機能 --------- */
        // コネクタ抜けた時の出力動作設定
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wLineFallOut(short wBsn, short wAxis, short wData);
        // スタート信号の制御
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wStartSignalWrite(short wBsn, short wAxisStart1, short wAxisStart2, short wAxisStart3, short wAxisStart4);
        //指定モード原点復帰ドライブの起動
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wOriginReturn(short wBsn, short wAxis, short wMode);
        //原点復帰　確認
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wOrgStatus(short wBsn, short wAxis, ref byte pbData);
        //ユーザ定義（原点復帰モード２０）の原点復帰シーケンスを設定
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wOriginTableSet(short wBsn, short wAxis, short wTableNumber, short wMode, short wDir, short wWait, short wSensEdge, short wSpeed);
        // 書き込み関数のバッファリング設定
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wBufferCountSet(short wBsn, short wCount);
        // バッファ済みの関数をボードに送信する
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wBufferFlash(short wBsn);

        /* ---------- USPG-46 互換部分 速度機能 --------- */
        //速度パラメータ書き込み
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wSpeedParameterWrite(short wBsn, short wAxis, double dLowSpeed, double dHighSpeed, short sAccTime, double dSRate);
        //速度パラメータ読み込み
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wSpeedParameterRead(short wBsn, short wAxis, ref double pdLowSpeed, ref double pdHighSpeed, ref short psAccTime, ref double pdSRate);
        //速度書き込み
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wSpeedWrite(short wBsn, short wAxis, double dObjSpeed);
        //速度読み込み
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wSpeedRead(short wBsn, short wAxis, ref double pdObjSpeed);

        /* ---------- MK-UNIT オリジナル ----------*/
        [DllImport("MkUnitAPI.dll")]
        public static extern int MkUnitwGetBordInf(short wBsn, ref uint pdwBordInf);
        /* -------- Ver 1.01 ------- */
        /// <summary>
        /// USPG-46 + USIO-32 (Default), 1: MK-UNIT
        /// </summary>
        /// <param name="wMode">USPG-46 + USIO-32 (Default), 1: MK-UNIT</param>
        /// <returns></returns>
        [DllImport("MkUnitAPI.dll")]
        public static extern int MkUnitwSetUnitType(short wMode);

        /* ---------- USIO-32 初期化--------- */
        // DLL をロード
        [DllImport("MkUnitAPI.dll")]
        public static extern int Usio32wDllOpen();
        // DLL をアンロード
        [DllImport("MkUnitAPI.dll")]
        public static extern int Usio32wDllClose();
        // デバイスの使用を宣言
        [DllImport("MkUnitAPI.dll")]
        public static extern int Usio32wCreate(ref short pwBsn); // psBsn : 보드 선택 번호
        // デバイスの解放
        [DllImport("MkUnitAPI.dll")]
        public static extern int Usio32wClose(short wBsn);

        /* ---------- USIO-32 情報取得--------- */
        // DLL のバージョン取得
        [DllImport("MkUnitAPI.dll")]
        public static extern int Usio32wGetLibVersion(StringBuilder pbLibVersion);
        // デバドラのバージョン取得
        [DllImport("MkUnitAPI.dll")]
        public static extern int Usio32wGetDrvVersion(StringBuilder pbDrvVersion);
        // ROM のバージョン取得
        [DllImport("MkUnitAPI.dll")]
        public static extern int Usio32wGetRomVersion(short wBsn, StringBuilder pbRomVersion);
        // デバイスの状態を取得
        [DllImport("MkUnitAPI.dll")]
        public static extern int Usio32wDeviceStatus(short wBsn, ref byte pbStatus);
        // エラーコード取得
        [DllImport("MkUnitAPI.dll")]
        public static extern int Usio32wGetLastError(short wBsn);

        /* ---------- USIO-32 入出力取得--------- */
        // 入力論理レベル設定
        [DllImport("MkUnitAPI.dll")]
        public static extern int Usio32wInLevelChange(short wBsn, uint dwData);
        // 出力論理レベル設定
        [DllImport("MkUnitAPI.dll")]
        public static extern int Usio32wOutLevelChange(short wBsn, uint dwData);
        // 読み込み実行
        [DllImport("MkUnitAPI.dll")]
        public static extern int Usio32wInPort(short wBsn, ushort wPort, ref byte pbData);
        // 書き込み実行
        [DllImport("MkUnitAPI.dll")]
        public static extern int Usio32wOutPort(short wBsn, ushort wPort, byte bData);
        // 読み込み実行
        [DllImport("MkUnitAPI.dll")]
        public static extern int Usio32wInPortW(short wBsn, ushort wPort, ref ushort pwData);
        // 書き込み実行
        [DllImport("MkUnitAPI.dll")]
        public static extern int Usio32wOutPortW(short wBsn, ushort wPort, ushort wData);
        // 読み込み実行
        [DllImport("MkUnitAPI.dll")]
        public static extern int Usio32wInPortDW(short wBsn, ref uint pdwData);
        // 書き込み実行
        [DllImport("MkUnitAPI.dll")]
        public static extern int Usio32wOutPortDW(short wBsn, uint dwData);
        // 読み込み実行
        [DllImport("MkUnitAPI.dll")]
        public static extern int Usio32wInPortBit(short wBsn, short wStartBit, short wNum, ref uint pdwData);
        // 書き込み実行
        [DllImport("MkUnitAPI.dll")]
        public static extern int Usio32wOutPortBit(short wBsn, short wStartBit, short wNum, uint dwData);

        /* ---------- USIO-32 拡張機能 --------- */
        // コネクタ抜けた時の出力動作設定
        [DllImport("MkUnitAPI.dll")]
        public static extern int Usio32wLineFallOut(short wBsn, byte bBit, byte bOutPortBack, byte bLatch, byte bCount);
        // ブリンク処理
        [DllImport("MkUnitAPI.dll")]
        public static extern int Usio32wOutPortBack(short wBsn, ushort wBit, int dwTime, int dwCount);
        // ラッチ時の周期設定
        [DllImport("MkUnitAPI.dll")]
        public static extern int Usio32wLatchSampleTime(short wBsn, int dwData);
        // ラッチの設定
        [DllImport("MkUnitAPI.dll")]
        public static extern int Usio32wSetLatchStatus(short wBsn, uint dwData);
        // ラッチデータ取得
        [DllImport("MkUnitAPI.dll")]
        public static extern int Usio32wGetLatchData(short wBsn, ref uint pdwData);
        // カウント時の周期設定
        [DllImport("MkUnitAPI.dll")]
        public static extern int Usio32wCountSampleTime(short wBsn, int dwData);
        // カウントビットの設定
        [DllImport("MkUnitAPI.dll")]
        public static extern int Usio32wSetCountStatus(short wBsn, ushort wBit, short wData);
        // カウント値の取得
        [DllImport("MkUnitAPI.dll")]
        public static extern int Usio32wGetCountData(short wBsn, ushort wBit, ref int pdwData);
        // 書き込み関数のバッファリング設定
        [DllImport("MkUnitAPI.dll")]
        public static extern int Usio32wBufferCountSet(short wBsn, short wCount);
        // バッファ済みの関数をボードに送信する
        [DllImport("MkUnitAPI.dll")]
        public static extern int Usio32wBufferFlash(short wBsn);

        /* ------- USPG-46 / USIO-32 ------- */
        /* -------- Ver 1.02 ------- */
        [DllImport("MkUnitAPI.dll")]
        public static extern int Uspg46wGetStatus(short wBsn);
        [DllImport("MkUnitAPI.dll")]
        public static extern int Usio32wGetStatus(short wBsn);

        // 読み込み実行
        [DllImport("MkUnitAPI.dll")]
        public static extern int Usio32wInPortDWALL(ref uint pdwData);
        // 書き込み実行
        [DllImport("MkUnitAPI.dll")]
        public static extern int Usio32wOutPortDWALL(ref uint dwData);
        // USB切断確認関数
        [DllImport("MkUnitAPI.dll")]
        public static extern int MkUnitwLineFallOut();//2014.02.18 sasaki 追加
    }

    #endregion </MKUNIT DLL IMPORT>

    #region <USPG46 ENUM>

    public enum USPG46_ERROR_CODE
    {
        SUCCESS              =   0,           
        ERR_NO_DEVICE        =   2,       
        ERR_IN_USE           =   3,           
        ERR_INVALID_BSN      =   4,       
				                    
        ERR_INVALID_PORT     =   6,
        ERR_WRAPDLL          =   31, // 미나미소스 (DWORD-1)
        ERR_INVALID_AXIS     =   50,      
        ERR_TRANS            =   60,          
        ERR_RECEIVE          =   61,          
        ERR_USB_REMOVE       =   62,  
    }
    public enum USPG46_MAX_SLOT
    {
        MAX_SLOT = 16,
    }
    public enum USPG46_AXIS
    {
        AXIS_1 = 0,
        AXIS_2 = 1,
        AXIS_3 = 2,
        AXIS_4 = 3,
    }
    public enum USPG46_IO_MAP
    {
        PORT_DATA_1             = 0x00,
        PORT_DATA_2             = 0x01,
        PORT_DATA_3             = 0x02,
        PORT_DATA_4             = 0x03,
        PORT_COMMAND            = 0x04,
        PORT_MODE_1             = 0x05,
        PORT_MODE_2             = 0x06,
        PORT_UNIVERSAL_SIGNAL   = 0x07,
        PORT_DRIVE_STATUS       = 0x04,
        PORT_END_STATUS         = 0x05,
        PORT_MECHENICAL_SIGNAL  = 0x06,
    }
    public enum EN_USPG46_COMMAND : byte
    {
        /// <summary>
        /// [Half] RANGE DATA 쓰기 (RANGE DATA = 4 x 10^6 / 최고주파수)
        /// </summary>
        RANGE_WRITE = 0x00,
        /// <summary>
        /// RANGE DATA 읽기 (RANGE DATA = 4 x 10^6 / 최고주파수)
        /// </summary>
        RANGE_READ = 0x01,
        START_STOP_SPEED_DATA_WRITE = 0x02,
        START_STOP_SPEED_DATA_READ = 0x03,
        OBJECT_SPEED_DATA_WRITE = 0x04,
        OBJECT_SPEED_DATA_READ = 0x05,
        RATE1_DATA_WRITE = 0x06,
        RATE1_DATA_READ = 0x07,
        RATE2_DATA_WRITE = 0x08,
        RATE2_DATA_READ = 0x09,
        RATE3_DATA_WRITE = 0x0a,
        RATE3_DATA_READ = 0x0b,
        RATE_CHANGE_POINT_1_2_WRITE = 0x0c,
        RATE_CHANGE_POINT_1_2_READ = 0x0d,
        RATE_CHANGE_POINT_2_3_WRITE = 0x0e,
        RATE_CHANGE_POINT_2_3_READ = 0x0f,
        SLOW_DOWN_REAR_PULSE_WRITE = 0x10,
        SLOW_DOWN_REAR_PULSE_READ = 0x11,
        NOW_SPEED_DATA_READ = 0x12,
        DRIVE_PULSE_COUNTER_READ = 0x13,
        PRESET_PULSE_DATA_OVERRIDE = 0x14,
        PRESET_PULSE_DATA_READ = 0x15,
        DEVIATION_DATA_READ = 0x16,
        /// <summary>
        /// 펄스 출력이 종료된 후에도 INP-n 신호 또는 ALM-n 신호가 활성화 될 때까지  BUSY-n = H를 유지
        /// (INPOSITION_WAIT_MODE2_SET 은 취소됨 => 미나미 예제에서 사용안함!)
        /// </summary>
        INPOSITION_WAIT_MODE1_SET = 0x17,
        /// <summary>
        /// 펄스 출력이 종료된 후에도 INP-n 신호가 활성화 될 때까지 BUSY-n = H를 유지
        /// (INPOSITION_WAIT_MODE1_SET 은 취소됨)
        /// </summary>
        INPOSITION_WAIT_MODE2_SET = 0x18,
        /// <summary>
        /// INPOSITION_WAIT_MODE1_SET, INPOSITION_WAIT_MODE2_SET 해제 (리셋 후와 동일 상황)
        /// : 펄스 출력 종료 된 시점에서 BUSY-n = L
        /// </summary>
        INPOSITION_WAIT_MODE_RESET = 0x19,
        /// <summary>
        /// ALARM 신호 활성화 시 E-STOP 기능 활성화 (리셋 후와 동일)
        /// </summary>
        ALARM_STOP_ENABLE_MODE_SET = 0x1a,
        /// <summary>
        /// ALARM 신호 활성화 시 E-STOP 기능 비활성화
        /// </summary>
        ALARM_STOP_ENABLE_MODE_RESET = 0x1b,

        INTERRUPT_OUT_ENABLE_MODE_SET = 0x1c, // 2019.07.12. by shkim. [ADD] MANUAL에 없고 예제에만 있는 것.
        INTERRUPT_OUT_ENABLE_MODE_RESET = 0x1d, // 2019.07.12. by shkim. [ADD] MANUAL에 없고 예제에만 있는 것.


        SLOW_DOWN_STOP = 0x1e,
        EMERGENCY_STOP = 0x1f,
        PLUS_PRESET_PULSE_DRIVE = 0x20,
        MINUS_PRESET_PULSE_DRIVE = 0x21,
        PLUS_CONTINUOUS_DRIVE = 0x22,
        MINUS_CONTINUOUS_DRIVE = 0x23,
        PLUS_SIGNAL_SEARCH1_DRIVE = 0x24,
        MINUS_SIGNAL_SEARCH1_DRIVE = 0x25,
        PLUS_SIGNAL_SEARCH2_DRIVE = 0x26,
        MINUS_SIGNAL_SEARCH2_DRIVE = 0x27,
        /// <summary>
        /// COMMAND POSITION(펄스) 쓰기
        /// </summary>
        INTERNAL_COUNTER_WRITE = 0x28,
        /// <summary>
        /// COMMAND POSITION(펄스) 읽기
        /// </summary>
        INTERNAL_COUNTER_READ = 0x29,
        INTERNAL_COMPARATE_DATA_WRITE = 0x2a,
        INTERNAL_COMPARATE_DATA_READ = 0x2b,
        /// <summary>
        /// ACTUAL POSITION(펄스) 쓰기
        /// </summary>
        EXTERNAL_COUNTER_WRITE = 0x2c,
        /// <summary>
        /// ACTUAL POSITION(펄스) 읽기
        /// </summary>
        EXTERNAL_COUNTER_READ = 0x2d,
        EXTERNAL_COMPARATE_DATA_WRITE = 0x2e,
        EXTERNAL_COMPARATE_DATA_READ = 0x2f,
        INTERNAL_PRE_SCALE_DATA_WRITE = 0x30,
        INTERNAL_PRE_SCALE_DATA_READ = 0x31,
        EXTERNAL_PRE_SCALE_DATA_WRITE = 0x32,
        EXTERNAL_PRE_SCALE_DATA_READ = 0x33,
        CLEAR_SIGNAL_SELECT = 0x34,
        ONE_TIME_CLEAR_REQUEST = 0x35,
        FULL_TIME_CLEAR_REQUEST = 0x36,
        CLEAR_REQUEST_RESET = 0x37,
        REVERSE_COUNT_MODE_SET = 0x38,
        REVERSE_COUNT_MODE_RESET = 0x39,
        NO_OPERATION = 0x3a,
        STRAIGHT_ACCELERATE_MODE_SET = 0x84,
        US_STRAIGHT_ACCELERATE_MODE_SET = 0x85,
        S_CURVE_ACCELERATE_MODE_SET = 0x86,
        US_S_CURVE_ACCELERATE_MODE_SET = 0x87,
        SW1_DATA_WRITE = 0x88,
        SW1_DATA_READ = 0x89,
        SW2_DATA_WRITE = 0x8a,
        SW2_DATA_READ = 0x8b,
        SLOW_DOWN_LIMIT_ENABLE_MODE_SET = 0x8c,
        SLOW_DOWN_LIMIT_ENABLE_MODE_RESET = 0x8d,
        /// <summary>
        /// [Command] +ELM-n, -ELM-n 신호 활성화 시 E-STOP 기능 활성화 (리셋 후와 동일)
        /// </summary>
        EMERGENCY_LIMIT_ENABLE_MODE_SET = 0x8e,
        /// <summary>
        /// +ELM-n, -ELM-n 신호 활성화 시 E-STOP 기능 비활성화
        /// </summary>
        EMERGENCY_LIMIT_ENABLE_MODE_RESET = 0x8f,
        INITIAL_CLEAR = 0x90,
        MODE1_READ = 0xa0,
        MODE2_READ = 0xa1,
        STATUS_READ = 0xa2,
        SLOW_DOWN_STOP2 = 0xa3,
        RISE_PULSE_COUNTER_READ = 0xa6,
        BI_PHASE_PULSE_SELECT_DATA_WRITE = 0xaa,
        BI_PHASE_PULSE_SELECT_DATA_READ = 0xab,
        SIGNAL_SERH2_REAR_PULSE_DATA_WRITE = 0xac,
        SIGNAL_SERH2_REAR_PULSE_DATA_READ = 0xad,
        MANUAL_PULSE_MODE_WRITE = 0xc0,
        MANUAL_PULSE_MODE_READ = 0xc1,
        SOFT_SYNC_MODE_WRITE = 0xc2,
        SOFT_SYNC_MODE_READ = 0xc3,
        SOFT_SYNC_EXECUTE = 0xc4,
    }
    public enum EN_ORIGIN_MODE : short
    {
        ORG_N_EDGE = 0,
        MINUS_DIR_ORG_P_EDGE_AFTER_Z_N_EDGE = 1,
        MINUS_DIR_ORG_SLOW_LIMIT = 2,       // -정지리밋센서
        PLUS_DIR_ORG_SLOW_LIMIT = 3,        // +정지리밋센서
        MINUS_DIR_ORG_E_LIMIT = 4,          // -정지리밋센서
        PLUS_DIR_ORG_E_LIMIT = 5,           // +정지리밋센서
        MINUS_DIR_SLOW_LIMIT_BEFORE_Z_N_EDGE = 6,
        PLUS_DIR_SLOW_LIMIT_BEFORE_Z_N_EDGE = 7, 
        Z_N_EDGE = 8,
        ORG_P_EDGE = 9,
        PLUS_DIR_ORG_P_EDGE_BEFORE_Z_N_DEG = 10,
        USER_MODE = 20,
    }
    public enum EN_USER_ORG_MODE
    {
        ON_SIGNAL_NEXT_SQC = 0,
        ON_SIGNAL_FORWARD_MOVE = 1,
        ON_SIGNAL_BACKWARD_MOVE = 2,
        ORGIN_END = 3,
    }

    public enum EN_USER_ORG_DIR
    {
        MINUS = 0,
        PLUS = 1,
    }
    public enum EN_USER_ORG_SENS_EDGE
    {
        E_LIMIT_P_DIR_N_EDGE = 0,
        E_LIMIT_N_DIR_N_EDGE = 1,
        S_LIMIT_P_DIR_N_EDGE = 2,
        S_LIMIT_N_DIR_N_EDGE = 3,
        ORG_P_EDGE = 4,
        Z_PHASE_P_EDGE = 5,
        IN_P_EDGE = 6,
        // 7 사용안함
        E_LIMIT_P_DIR_P_EDGE = 8,
        E_LIMIT_N_DIR_P_EDGE = 9,
        S_LIMIT_P_DIR_P_EDGE = 10,
        S_LIMIT_N_DIR_P_EDGE = 11,
        ORG_N_EDGE = 12,
        Z_PHASE_N_EDGE = 13,
        IN_N_EDGE = 14,
    }
    public enum EN_USER_ORG_SPEED
    {
        MAX_SPEED = 0,
        START_STOP_SPEED = 1,
    }

    public enum EN_DRIVE_STATUS : byte
    {
        ECG = 128,
        ECL = 64,
        ICG = 32,
        ICL = 16,
        UP = 8,
        COST = 4,
        DOWN = 2,
        BUSY = 1,
        EMPTY = 0,
    }
    public enum EN_END_STATUS : byte
    {
        DATA_ERROR_END  = 128,
        ALARM_SIGNAL_END = 64,
        E_STOP_COMMAND_END = 32,
        SLOW_DOWN_STOP_COMMAND_END = 16,
        E_STOP_SIGANL_END = 8,
        // 사용안함 4
        E_LIMIT_SIGNAL_END = 2,
        SLOW_DOWN_SIGNAL_END = 1,
        NO_ERROR = 0,
    }
    public enum EN_MECHANICAL_STATUS : byte
    {
        ECUP = 128,
        ECDN = 64,
        DEND = 32,
        DERR = 16,
        SLM_N = 8,  // - LIMIT (SLOW-STOP)
        SLM_P = 4,  // + LIMIT (SLOW-STOP)
        ELM_N = 2,  // - LIMIT (E-STOP)
        ELM_P = 1,  // + LIMIT (E-STOP)
        EMPTY = 0,
    }
    public enum EN_UNIVALSAL_STATUS : byte
    {
        // 미사용? 128
        IN  = 64,
        Z_PHASE = 32,
        ORG = 16,
        // 8
        // 4
        DRST = 2,
        DRIVE = 1,
        EMPTRY = 0,
    }
    
    #endregion </USPG46 ENUM>

    #region <USIO32 ENUM>
    public enum USIO32_ERROCODE
    {
        SUCCESS         =   0,
        ERR_NO_DEVICE   =   2,
        ERR_IN_USE      =   3,
        ERR_INVALID_BSN =   4,

        ERR_INVALID_PORT =  6,
        ERR_PARAMETER    =  7,
        ERR_TRANS        = 60,
        ERR_RECEIVE      = 61,
        ERR_USB_REMOVE   = 62,
        ERR_WRAPDLL      = 31, // Minami 프로그램 (DWORD-1)
    }
    public enum USIO32_SLOT
    {
        MAX_SLOT    = 16,
    }
    public enum USIO32_IO_MAP
    {
        PORT_1 = 0x00,
        PORT_2 = 0x01,
        PORT_3 = 0x02,
        PORT_4 = 0x03,
    }
    #endregion </USIO32 ENUM>
}