using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;
using System.Data;
using SensorLogInserterRe.Daos;

namespace SensorLogInserterRe.Models
{
    public class Driver : NotificationObject
    {

        #region DriverId変更通知プロパティ
        private int _DriverId;

        public int DriverId
        {
            get
            { return _DriverId; }
            set
            { 
                if (_DriverId == value)
                    return;
                _DriverId = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Name変更通知プロパティ
        private string _Name;

        public string Name
        {
            get
            { return _Name; }
            set
            { 
                if (_Name == value)
                    return;
                _Name = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        public static Driver GetDriver(string driverName)
        {
            var result = DriverDao.Get(driverName);
            return new Driver()
            {
                DriverId = result.Rows[0].Field<int>(DriverDao.ColumnDriverId),
                Name = result.Rows[0].Field<string>(DriverDao.ColumnName)
            };
        }

    }
}
