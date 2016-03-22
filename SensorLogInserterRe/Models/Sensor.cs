using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;

namespace SensorLogInserterRe.Models
{
    public class Sensor : NotificationObject
    {
        #region SensorId変更通知プロパティ
        private int _SensorId;

        public int SensorId
        {
            get
            { return _SensorId; }
            set
            {
                if (_SensorId == value)
                    return;
                _SensorId = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region SensorModel変更通知プロパティ
        private string _SensorModel;

        public string SensorModel
        {
            get
            { return _SensorModel; }
            set
            {
                if (_SensorModel == value)
                    return;
                _SensorModel = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Brand変更通知プロパティ
        private string _Brand;

        public string Brand
        {
            get
            { return _Brand; }
            set
            {
                if (_Brand == value)
                    return;
                _Brand = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region OsVersion変更通知プロパティ
        private float _OsVersion;

        public float OsVersion
        {
            get
            { return _OsVersion; }
            set
            {
                if (_OsVersion == value)
                    return;
                _OsVersion = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Ordinal変更通知プロパティ
        private int _Ordinal;

        public int Ordinal
        {
            get
            { return _Ordinal; }
            set
            {
                if (_Ordinal == value)
                    return;
                _Ordinal = value;
                RaisePropertyChanged();
            }
        }
        #endregion
    }
}
