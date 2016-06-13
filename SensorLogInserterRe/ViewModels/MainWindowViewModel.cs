using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;
using SensorLogInserterRe.Constant;
using SensorLogInserterRe.Handlers;
using SensorLogInserterRe.Inserters;
using SensorLogInserterRe.Models;

namespace SensorLogInserterRe.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        #region IsCheckedTommy変更通知プロパティ
        private bool _IsCheckedTommy;

        public bool IsCheckedTommy
        {
            get
            { return _IsCheckedTommy; }
            set
            {
                if (_IsCheckedTommy == value)
                    return;
                _IsCheckedTommy = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region IsCheckedMori変更通知プロパティ
        private bool _IsCheckedMori;

        public bool IsCheckedMori
        {
            get
            { return _IsCheckedMori; }
            set
            {
                if (_IsCheckedMori == value)
                    return;
                _IsCheckedMori = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region IsCheckedTamura変更通知プロパティ
        private bool _IsCheckedTamura;

        public bool IsCheckedTamura
        {
            get
            { return _IsCheckedTamura; }
            set
            {
                if (_IsCheckedTamura == value)
                    return;
                _IsCheckedTamura = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region IsCheckedLabMember変更通知プロパティ
        private bool _IsCheckedLabMember;

        public bool IsCheckedLabMember
        {
            get
            { return _IsCheckedLabMember; }
            set
            {
                if (_IsCheckedLabMember == value)
                    return;
                _IsCheckedLabMember = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region IsCheckedPeriod変更通知プロパティ
        private bool _IsCheckedPeriod;

        public bool IsCheckedPeriod
        {
            get
            { return _IsCheckedPeriod; }
            set
            {
                if (_IsCheckedPeriod == value)
                    return;
                _IsCheckedPeriod = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region StartDate変更通知プロパティ
        private DateTime _StartDate;

        public DateTime StartDate
        {
            get
            { return _StartDate; }
            set
            {
                if (_StartDate == value)
                    return;
                _StartDate = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region EndDate変更通知プロパティ
        private DateTime _EndDate;

        public DateTime EndDate
        {
            get
            { return _EndDate; }
            set
            {
                if (_EndDate == value)
                    return;
                _EndDate = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region IsCheckedLeafEarlyModel変更通知プロパティ
        private bool _IsCheckedLeafEarlyModel;

        public bool IsCheckedLeafEarlyModel
        {
            get
            { return _IsCheckedLeafEarlyModel; }
            set
            {
                if (_IsCheckedLeafEarlyModel == value)
                    return;
                _IsCheckedLeafEarlyModel = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region IsCheckedEvModel変更通知プロパティ
        private bool _IsCheckedEvModel;

        public bool IsCheckedEvModel
        {
            get
            { return _IsCheckedEvModel; }
            set
            {
                if (_IsCheckedEvModel == value)
                    return;
                _IsCheckedEvModel = value;
                _IsCheckedMlModel = !value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region IsCheckedMlModel変更通知プロパティ
        private bool _IsCheckedMlModel;

        public bool IsCheckedMlModel
        {
            get
            { return _IsCheckedMlModel; }
            set
            {
                if (_IsCheckedMlModel == value)
                    return;
                _IsCheckedMlModel = value;
                _IsCheckedEvModel = !value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region IsCheckedMapMatching変更通知プロパティ
        private bool _IsCheckedMapMatching;

        public bool IsCheckedMapMatching
        {
            get
            { return _IsCheckedMapMatching; }
            set
            {
                if (_IsCheckedMapMatching == value)
                    return;
                _IsCheckedMapMatching = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region IsCheckedDeadReckoning変更通知プロパティ
        private bool _IsCheckedDeadReckoning;

        public bool IsCheckedDeadReckoning
        {
            get
            { return _IsCheckedDeadReckoning; }
            set
            {
                if (_IsCheckedDeadReckoning == value)
                    return;
                _IsCheckedDeadReckoning = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region IsEnabledInsertButton変更通知プロパティ
        private bool _IsEnabledInsertButton;

        public bool IsEnabledInsertButton
        {
            get
            { return _IsEnabledInsertButton; }
            set
            {
                if (_IsEnabledInsertButton == value)
                    return;
                _IsEnabledInsertButton = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region LoopButtonText変更通知プロパティ
        private string _LoopButtonText;

        public string LoopButtonText
        {
            get
            { return _LoopButtonText; }
            set
            {
                if (_LoopButtonText == value)
                    return;
                _LoopButtonText = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region LogText変更通知プロパティ
        private string _LogText;

        public string LogText
        {
            get
            { return _LogText; }
            set
            {
                if (_LogText == value)
                    return;
                _LogText = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        private InsertConfig InsertConfig { get; set; }

        private List<string> InsertFileList { get; set; }

        private List<InsertDatum> InsertDatumList { get; set; }

        public void Initialize()
        {
            InitDriversChecked();
            InitPeriod();
            InitModelChecked();
            InitGpsCorrection();
            InitButton();
        }

        private void InitDriversChecked()
        {
            this.IsCheckedTommy = true;
            this.IsCheckedMori = true;
            this.IsCheckedTamura = true;
            this.IsCheckedLabMember = false;
        }

        private void InitPeriod()
        {
            this.IsCheckedPeriod = false;
            this.StartDate = DateTime.Now.AddDays(-7);
            this.EndDate = DateTime.Now;
        }

        private void InitModelChecked()
        {
            this.IsCheckedEvModel = true;
            this.IsCheckedMlModel = false;
        }

        private void InitGpsCorrection()
        {
            this.IsCheckedMapMatching = false;
            this.IsCheckedDeadReckoning = false;
        }

        private void InitButton()
        {
            IsEnabledInsertButton = true;
            LoopButtonText = "ループ起動";
        }

        public void StartUpLoop()
        {
            MessageBox.Show("StartUpLoop");
        }

        public void Insert()
        {
            this.LogText += LogTexts.TheStartOfTheCheckUpdateFile + "\n";
            this.InsertConfig = this.GenerateInsertConfig();

            this.SearchDirectory();
            this.InsertGps();
            this.InsertAcc();

            foreach (var datum in InsertDatumList)
            {
                this.InsertTrips(datum);
                this.InsertCorrectedAcc(datum);
                this.InsertEcolog(datum);
            }
        }

        private InsertConfig GenerateInsertConfig()
        {
            var insertConfig = InsertConfig.GetInstance();

            #region ドライバーの設定
            if (this.IsCheckedTommy)
                insertConfig.CheckeDrivers.Add(DriverNames.Tommy);
            if (this.IsCheckedMori)
                insertConfig.CheckeDrivers.Add(DriverNames.Mori);
            if (this.IsCheckedTamura)
                insertConfig.CheckeDrivers.Add(DriverNames.Tamura);
            // TODO 研究室メンバー
            #endregion

            #region 期間の設定

            insertConfig.StartDate = this.StartDate;
            insertConfig.EndDate = this.EndDate;

            #endregion

            #region 推定対象車両の設定
            if (this.IsCheckedLeafEarlyModel)
                insertConfig.CarModel = InsertConfig.EstimatedCarModel.LeafEarlyModel;
            #endregion

            #region 推定モデルの設定

            if (this.IsCheckedEvModel)
                insertConfig.EstModel = InsertConfig.EstimationModel.EvEnergyConsumptionModel;
            else if (this.IsCheckedMlModel)
                insertConfig.EstModel = InsertConfig.EstimationModel.MachineLearningModel;

            #endregion

            #region GPS補正の設定

            if (this.IsCheckedMapMatching)
                insertConfig.Correction = InsertConfig.GpsCorrection.MapMatching;
            else if (this.IsCheckedDeadReckoning)
                insertConfig.Correction = InsertConfig.GpsCorrection.DeadReckoning;

            #endregion

            return insertConfig;
        }

        private async void SearchDirectory()
        {
            this.LogText += LogTexts.DuringCheckOfTheUpdateFile + "\n";

            await Task.Run(() =>
            {
                this.InsertFileList = DirectorySearcher.DirectorySearch(this.InsertConfig);
            });

            this.LogText += $"{LogTexts.NumberOfTheInsertedFile}: {this.InsertFileList.Count}\n";
        }

        private async void InsertGps()
        {
            this.LogText += LogTexts.TheSrartOfTheInsertingGps + "\n";

            await Task.Run(() =>
            {
                GpsInserter.InsertGps(this.InsertFileList, this.InsertConfig, this.InsertDatumList);
            });

            this.LogText += LogTexts.TheEndOfTheInsertingGps + "\n";
        }

        private async void InsertAcc()
        {
            this.LogText += LogTexts.TheSrartOfTheInsertingAcc + "\n";

            await Task.Run(() =>
            {
                AccInserter.InsertAcc(this.InsertFileList, this.InsertConfig, this.InsertDatumList);
            });

            this.LogText += LogTexts.TheEndOfTheInsertingAcc + "\n";
        }

        private async void InsertTrips(InsertDatum datum)
        {
            this.LogText += LogTexts.TheStartOfTheInsertingTrips + "\n";

            await Task.Run(() =>
            {
                TripInserter.InsertTrip(datum);
            });

            this.LogText += LogTexts.TheEndOfTheInsertingTrips + "\n";
        }

        private async void InsertCorrectedAcc(InsertDatum datum)
        {
            this.LogText += LogTexts.TheStartOfTheInsertingCorrectedAcc + "\n";

            await Task.Run(() =>
            {
                AccInserter.InsertCorrectedAcc(datum);
            });

            this.LogText += LogTexts.TheEndOfTheInsertingCorrectedAcc + "\n";
        }

        private async void InsertEcolog(InsertDatum datum)
        {
            this.LogText += LogTexts.TheStartOfTheInsertingEcolog + "\n";

            await Task.Run(() =>
            {
                EcologInserter.InsertEcolog(datum);
            });

            this.LogText += LogTexts.TheEndOfTheInsertingEcolog + "\n";
        }
    }
}
