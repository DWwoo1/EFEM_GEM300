using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;
using RecipeManager_;
using Serial_;
using ThreadTimer_;
using TickCounter_;

using FrameOfSystem3.ExternalDevice.Serial.FanFilterUnit.Bluecord;

namespace FrameOfSystem3.ExternalDevice.Serial.FanFilterUnit
{
    public class UnitItem
    {
        public int UnitId { get; set; }
        public double SettingSpeed { get; set; }    // 2024.12.24 dwlim [ADD] Speed Set 추가
        public double CurrentSpeed { get; set; }
        public double SettingDifferentialPressure { get; set; }
        public double CurrentDifferentialPressure { get; set; }
        public string Status { get; set; }
        public string AlarmCode { get; set; }       // 2024.12.24 dwlim [ADD] UI에 Alarm Code 없어서 추가
        public bool IsError { get; set; }
    }

    public class FanFilterUnitManager
    {
        #region <Constructors>
        private FanFilterUnitManager()
        {
        }
        #endregion </Constructors>

        #region <Fields>
        private static FanFilterUnitManager _instance = null;

        private FanFilterUnitController _controller = null;
        #endregion </Fields>

        #region <Properties>
        public static FanFilterUnitManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FanFilterUnitManager();
                }

                return _instance;
            }
        }
        public bool Skipped { get; set; }
        public bool IsErrored { get; private set; }
        public bool UseDifferentialPressureMode 
        {
            get
            {
                if (_controller == null)
                    return false;

                return _controller.UseDifferentialPressureMode;
            }
            set
            {
                if (_controller == null)
                    return;

                _controller.UseDifferentialPressureMode = value;
            }
        }
        public int Count
        {
            get
            {
                if (_controller == null)
                    return 0;

                return _controller.UnitCount;
            }
        }
        #endregion </Properties>

        #region <Methods>

        #region <INTERFACE>

        #region <Communication methods>
        public bool Activate()
        {
            Skipped = false;
            if (_controller == null)
                return false;

            return _controller.Connect();
        }

        public void Deactivate()
        {
            if (_controller == null)
                return;

            _controller.DisConnect();
        }

        public void AddController(FanFilterUnitController controller)
        {
            _controller = controller;
            UseDifferentialPressureMode = controller.UseDifferentialPressureMode;
        }
        public bool IsConnected()
        {
            return _controller.IsConnected;
        }
        public void DoOpen()
        {
            if (Skipped) return;

            _controller.Connect();
        }

        public bool IsOpened()
        {
            if (Skipped) return true;

            return _controller.IsConnected;
        }
        // 2024.12.24 dwlim [ADD] Speed Set 추가
        public bool SetSpeedAll(double speed)
        {
            if (_controller == null)
                return false;

            return _controller.SetSpeedAll(speed);
        }

        public bool GetUnitSpeedAll(ref double[] speed)
        {
            if (_controller == null)
                return false;

            return _controller.GetUnitSpeedAll(ref speed);
        }

        // 2024.12.27. by dwlim [ADD] 차압 Setting Command 추가
        public bool SetPressure(double pressure)
        {
            if (_controller == null)
                return false;

            return _controller.SetUnitDifferentialPressure(pressure);
        }
        public bool GetInformation(int index, ref UnitItem unitInfo)
        {
            if (_controller == null)
                return false;

            return _controller.GetInformation(index, ref unitInfo);
        }        
        #endregion </Communication methods>

        #endregion </INTERFACE>

        public void Execute()
        {
            _controller.Monitoring();
        }
        #endregion </Methods>
    }
}
