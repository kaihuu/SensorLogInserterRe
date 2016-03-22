using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;
using System.Collections.ObjectModel;
using System.Data;

namespace ECOLOGSemanticViewer.Models.EcologModels
{
    public class Ecolog : NotificationObject
    {
        #region TripId変更通知プロパティ
        private int _TripId;

        public int TripId
        {
            get
            { return _TripId; }
            set
            {
                if (_TripId == value)
                    return;
                _TripId = value;
                RaisePropertyChanged();
            }
        }
        #endregion

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

        #region CarId変更通知プロパティ
        private int _CarId;

        public int CarId
        {
            get
            { return _CarId; }
            set
            {
                if (_CarId == value)
                    return;
                _CarId = value;
                RaisePropertyChanged();
            }
        }
        #endregion

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

        #region Jst変更通知プロパティ
        private DateTime _Jst;

        public DateTime Jst
        {
            get
            { return _Jst; }
            set
            {
                if (_Jst == value)
                    return;
                _Jst = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Latitude変更通知プロパティ
        private double _Latitude;

        public double Latitude
        {
            get
            { return _Latitude; }
            set
            {
                if (_Latitude == value)
                    return;
                _Latitude = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Longitude変更通知プロパティ
        private double _Longitude;

        public double Longitude
        {
            get
            { return _Longitude; }
            set
            {
                if (_Longitude == value)
                    return;
                _Longitude = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Speed変更通知プロパティ
        private float _Speed;

        public float Speed
        {
            get
            { return _Speed; }
            set
            {
                if (_Speed == value)
                    return;
                _Speed = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Heading変更通知プロパティ
        private float _Heading;

        public float Heading
        {
            get
            { return _Heading; }
            set
            {
                if (_Heading == value)
                    return;
                _Heading = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region DistanceDifference変更通知プロパティ
        private float _DistanceDifference;

        public float DistanceDifference
        {
            get
            { return _DistanceDifference; }
            set
            {
                if (_DistanceDifference == value)
                    return;
                _DistanceDifference = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region TerrainAltitude変更通知プロパティ
        private float _TerrainAltitude;

        public float TerrainAltitude
        {
            get
            { return _TerrainAltitude; }
            set
            {
                if (_TerrainAltitude == value)
                    return;
                _TerrainAltitude = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region TerrainAltitudeDifference変更通知プロパティ
        private float _TerrainAltitudeDifference;

        public float TerrainAltitudeDifference
        {
            get
            { return _TerrainAltitudeDifference; }
            set
            {
                if (_TerrainAltitudeDifference == value)
                    return;
                _TerrainAltitudeDifference = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region LongitudinalAcc変更通知プロパティ
        private float _LongitudinalAcc;

        public float LongitudinalAcc
        {
            get
            { return _LongitudinalAcc; }
            set
            {
                if (_LongitudinalAcc == value)
                    return;
                _LongitudinalAcc = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region LateralAcc変更通知プロパティ
        private float _LateralAcc;

        public float LateralAcc
        {
            get
            { return _LateralAcc; }
            set
            {
                if (_LateralAcc == value)
                    return;
                _LateralAcc = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region VerticalAcc変更通知プロパティ
        private float _VerticalAcc;

        public float VerticalAcc
        {
            get
            { return _VerticalAcc; }
            set
            {
                if (_VerticalAcc == value)
                    return;
                _VerticalAcc = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region EnergyByAirResistance変更通知プロパティ
        private float _EnergyByAirResistance;

        public float EnergyByAirResistance
        {
            get
            { return _EnergyByAirResistance; }
            set
            {
                if (_EnergyByAirResistance == value)
                    return;
                _EnergyByAirResistance = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region EnergyByRollingResistance変更通知プロパティ
        private float _EnergyByRollingResistance;

        public float EnergyByRollingResistance
        {
            get
            { return _EnergyByRollingResistance; }
            set
            {
                if (_EnergyByRollingResistance == value)
                    return;
                _EnergyByRollingResistance = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region EnergyByClimbingResistance変更通知プロパティ
        private float _EnergyByClimbingResistance;

        public float EnergyByClimbingResistance
        {
            get
            { return _EnergyByClimbingResistance; }
            set
            {
                if (_EnergyByClimbingResistance == value)
                    return;
                _EnergyByClimbingResistance = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region EnergyByAccResistance変更通知プロパティ
        private float _EnergyByAccResistance;

        public float EnergyByAccResistance
        {
            get
            { return _EnergyByAccResistance; }
            set
            {
                if (_EnergyByAccResistance == value)
                    return;
                _EnergyByAccResistance = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region ConvertLoss変更通知プロパティ
        private float _ConvertLoss;

        public float ConvertLoss
        {
            get
            { return _ConvertLoss; }
            set
            {
                if (_ConvertLoss == value)
                    return;
                _ConvertLoss = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region RegeneLoss変更通知プロパティ
        private float _RegeneLoss;

        public float RegeneLoss
        {
            get
            { return _RegeneLoss; }
            set
            {
                if (_RegeneLoss == value)
                    return;
                _RegeneLoss = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region RegeneEnergy変更通知プロパティ
        private float _RegeneEnergy;

        public float RegeneEnergy
        {
            get
            { return _RegeneEnergy; }
            set
            {
                if (_RegeneEnergy == value)
                    return;
                _RegeneEnergy = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region LostEnergy変更通知プロパティ
        private float _LostEnergy;

        public float LostEnergy
        {
            get
            { return _LostEnergy; }
            set
            {
                if (_LostEnergy == value)
                    return;
                _LostEnergy = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Efficiency変更通知プロパティ
        private float _Efficiency;

        public float Efficiency
        {
            get
            { return _Efficiency; }
            set
            {
                if (_Efficiency == value)
                    return;
                _Efficiency = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region ConsumedElectricEnergy変更通知プロパティ
        private float _ConsumedElectricEnergy;

        public float ConsumedElectricEnergy
        {
            get
            { return _ConsumedElectricEnergy; }
            set
            {
                if (_ConsumedElectricEnergy == value)
                    return;
                _ConsumedElectricEnergy = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region LostEnergyByWellToWheel変更通知プロパティ
        private float _LostEnergyByWellToWheel;

        public float LostEnergyByWellToWheel
        {
            get
            { return _LostEnergyByWellToWheel; }
            set
            {
                if (_LostEnergyByWellToWheel == value)
                    return;
                _LostEnergyByWellToWheel = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region ConsumedFuel変更通知プロパティ
        private float _ConsumedFuel;

        public float ConsumedFuel
        {
            get
            { return _ConsumedFuel; }
            set
            {
                if (_ConsumedFuel == value)
                    return;
                _ConsumedFuel = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region ConsumedFuelByWellToWheel変更通知プロパティ
        private float _ConsumedFuelByWellToWheel;

        public float ConsumedFuelByWellToWheel
        {
            get
            { return _ConsumedFuelByWellToWheel; }
            set
            {
                if (_ConsumedFuelByWellToWheel == value)
                    return;
                _ConsumedFuelByWellToWheel = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region EnergyByEquipment変更通知プロパティ
        private float _EnergyByEquipment;

        public float EnergyByEquipment
        {
            get
            { return _EnergyByEquipment; }
            set
            {
                if (_EnergyByEquipment == value)
                    return;
                _EnergyByEquipment = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region EnergyByCooling変更通知プロパティ
        private float _EnergyByCooling;

        public float EnergyByCooling
        {
            get
            { return _EnergyByCooling; }
            set
            {
                if (_EnergyByCooling == value)
                    return;
                _EnergyByCooling = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region EnergyByHeating変更通知プロパティ
        private float _EnergyByHeating;

        public float EnergyByHeating
        {
            get
            { return _EnergyByHeating; }
            set
            {
                if (_EnergyByHeating == value)
                    return;
                _EnergyByHeating = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region TripDirection変更通知プロパティ
        private string _TripDirection;

        public string TripDirection
        {
            get
            { return _TripDirection; }
            set
            {
                if (_TripDirection == value)
                    return;
                _TripDirection = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region MeshId変更通知プロパティ
        private int _MeshId;

        public int MeshId
        {
            get
            { return _MeshId; }
            set
            {
                if (_MeshId == value)
                    return;
                _MeshId = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region LinkId変更通知プロパティ
        private string _LinkId;

        public string LinkId
        {
            get
            { return _LinkId; }
            set
            {
                if (_LinkId == value)
                    return;
                _LinkId = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region RoadTheta変更通知プロパティ
        private float _RoadTheta;

        public float RoadTheta
        {
            get
            { return _RoadTheta; }
            set
            {
                if (_RoadTheta == value)
                    return;
                _RoadTheta = value;
                RaisePropertyChanged();
            }
        }
        #endregion
    }
}