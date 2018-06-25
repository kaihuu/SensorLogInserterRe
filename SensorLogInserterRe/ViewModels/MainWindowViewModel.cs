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
using SensorLogInserterRe.Utils;

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

        #region IsCheckedSpeedLPFMapMatching変更通知プロパティ
        private bool _IsCheckedSpeedLPFMapMatching;
        public bool IsCheckedSpeedLPFMapMatching
        {
            get
            { return _IsCheckedSpeedLPFMapMatching; }
            set
            {
                if (_IsCheckedSpeedLPFMapMatching == value)
                    return;
                _IsCheckedSpeedLPFMapMatching = value;
                RaisePropertyChanged();
            }
        }
#endregion

        #region IsCheckedInsertAcc変更通知プロパティ
        private bool _IsCheckedInsertAcc;

        public bool IsCheckedInsertAcc
        {
            get
            { return _IsCheckedInsertAcc; }
            set
            {
                if (_IsCheckedInsertAcc == value)
                    return;
                _IsCheckedInsertAcc = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region IsCheckedInsertCorrectedAcc変更通知プロパティ
        private bool _IsCheckedInsertCorrectedAcc;

        public bool IsCheckedInsertCorrectedAcc
        {
            get
            { return _IsCheckedInsertCorrectedAcc; }
            set
            {
                if (_IsCheckedInsertCorrectedAcc == value)
                    return;
                _IsCheckedInsertCorrectedAcc = value;
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
        #endregion)


        #region IsCheckedNormal変更通知プロパティ
        private bool _IsCheckedNormal;

        public bool IsCheckedNormal
        {
            get
            { return _IsCheckedNormal; }
            set
            { 
                if (_IsCheckedNormal == value)
                    return;
                _IsCheckedNormal = value;
                RaisePropertyChanged("IsCheckedNormal");
            }
        }
        #endregion


        #region IsCheckedLPFEx変更通知プロパティ
        private bool _IsCheckedLPFEx;

        public bool IsCheckedLPFEx
        {
            get
            { return _IsCheckedLPFEx; }
            set
            { 
                if (_IsCheckedLPFEx == value)
                    return;
                _IsCheckedLPFEx = value;
                RaisePropertyChanged("IsCheckedLPFEx");
            }
        }
        #endregion


        #region IsEnabledStartUpLoopButton変更通知プロパティ
        private bool _IsEnabledStartUpLoopButton;

        public bool IsEnabledStartUpLoopButton
        {
            get
            { return _IsEnabledStartUpLoopButton; }
            set
            { 
                if (_IsEnabledStartUpLoopButton == value)
                    return;
                _IsEnabledStartUpLoopButton = value;
                RaisePropertyChanged("IsEnabledStartUpLoopButton");
            }
        }
        #endregion


        private InsertConfig InsertConfig { get; set; }

        private List<string> InsertFileList { get; set; }

        private List<InsertDatum> InsertDatumList { get; set; }

        public delegate void UpdateTextDelegate(string text);

        private UpdateTextDelegate UpdateText { get; set; }

        public void Initialize()
        {
            InitDriversChecked();
            InitPeriod();
            InitEvEstimationModel();
            InitModelChecked();
            InitGpsCorrection();
            InitInsertionTarget();
            InitButton();

            

            UpdateText += (s) =>
            {
                this.LogText += s + "\n";
            };
        }

        private void InitDriversChecked()
        {
            this.IsCheckedTommy = true;
            this.IsCheckedMori = true;
            this.IsCheckedTamura = true;
            this.IsCheckedLabMember = true;

        }

        private void InitPeriod()
        {
            this.IsCheckedPeriod = false;
            this.StartDate = DateTime.Now.AddDays(-7);
            this.EndDate = DateTime.Now;
        }

        private void InitEvEstimationModel()
        {
            this.IsCheckedLeafEarlyModel = true;
        }

        private void InitModelChecked()
        {
            this.IsCheckedEvModel = true;
            this.IsCheckedMlModel = false;
        }

        private void InitGpsCorrection()
        {
            this.IsCheckedMapMatching = true;
            this.IsCheckedDeadReckoning = false;
            this.IsCheckedSpeedLPFMapMatching = true;
            this.IsCheckedNormal = true;
        }

        private void InitInsertionTarget()
        {
            this.IsCheckedInsertAcc = true;
            this.IsCheckedInsertCorrectedAcc = false;
        }

        private void InitButton()
        {
            IsEnabledInsertButton = true;
            IsEnabledStartUpLoopButton = true;
            LoopButtonText = "ループ起動";
        }

        public void StartUpLoop()
        {
            MessageBox.Show("StartUpLoop");
            
            Insert();
            Task.Delay(1000 * 3600);
            IsEnabledInsertButton = false;
            IsEnabledStartUpLoopButton = false;
        }

        public async void Insert()
        {
            this.InsertDatumList = new List<InsertDatum>();
            this.InsertFileList = new List<string>();
            IsEnabledInsertButton = false;
            IsEnabledStartUpLoopButton = false;
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            
            this.InsertConfig = this.GenerateInsertConfig();
            #region ファイル検索

            this.LogText += LogTexts.DuringCheckOfTheUpdateFile + "\n";
            LogWritter.WriteLog(LogWritter.LogMode.Search, LogTexts.DuringCheckOfTheUpdateFile + "\n");

            await Task.Run(() =>
            {
                this.InsertFileList = DirectorySearcher.DirectorySearch(this.InsertConfig);
            });

            this.LogText += $"{LogTexts.NumberOfTheInsertedFile}: {this.InsertFileList.Count}\n";
            LogWritter.WriteLog(LogWritter.LogMode.Search, $"{LogTexts.NumberOfTheInsertedFile}: {this.InsertFileList.Count}\n");

            #endregion

            #region GPS挿入

            this.LogText += LogTexts.TheSrartOfTheInsertingGps + "\n";
            LogWritter.WriteLog(LogWritter.LogMode.Gps, LogTexts.TheSrartOfTheInsertingGps + "\n");

            //await Task.Run(() =>
            //{
            //    for (int i = 0; i < this.InsertConfig.Correction.Count; i++)
            //    {
            //        GpsInserter.InsertGps(this.InsertFileList, this.InsertConfig, i, this.InsertDatumList);
            //    }
            //});

            Parallel.For(0, this.InsertConfig.Correction.Count, i =>
           {
               GpsInserter.InsertGps(this.InsertFileList, this.InsertConfig, i, this.InsertDatumList);
           });

            this.LogText += LogTexts.TheEndOfTheInsertingGps + "\n";
            LogWritter.WriteLog(LogWritter.LogMode.Gps, LogTexts.TheEndOfTheInsertingGps + "\n");

            #endregion

            #region 加速度挿入

            if (IsCheckedInsertAcc)
            {
                this.LogText += LogTexts.TheSrartOfTheInsertingAcc + "\n";
                LogWritter.WriteLog(LogWritter.LogMode.Acc, LogTexts.TheSrartOfTheInsertingAcc + "\n");

                await Task.Run(() =>
                {
                    AccInserter.InsertAcc(this.InsertFileList, this.InsertConfig, this.InsertDatumList);
                });

                this.LogText += LogTexts.TheEndOfTheInsertingAcc + "\n";
                LogWritter.WriteLog(LogWritter.LogMode.Acc, LogTexts.TheEndOfTheInsertingAcc + "\n");
            }

            #endregion

            foreach (var datum in InsertDatumList)
            {
                #region トリップ挿入

                //await Task.Run(() =>
                //{
                    for (int i = 0; i < this.InsertConfig.Correction.Count; i++)
                    {
                        TripInserter.InsertTrip(datum, InsertConfig.Correction[i]);
                    }
                //});

                #endregion

                #region 補正加速度挿入

                //if (IsCheckedInsertCorrectedAcc)
                //{
                //    await Task.Run(() =>
                //    {
                //        AccInserter.InsertCorrectedAcc(datum, InsertConfig);
                //    });
                //}

                #endregion
            }
            int count = 0;
            Parallel.For(0, InsertDatumList.Count, i =>
            {
                #region ECOLOG挿入
                //     sw.Start();

                     if (IsCheckedSpeedLPFMapMatching)
                    {
                        EcologInserter.InsertEcologSpeedLPF005MM(InsertDatumList[i], this.UpdateText, InsertConfig.GpsCorrection.SpeedLPFMapMatching);
                    }
                    if (IsCheckedMapMatching)
                    {
                        EcologInserter.InsertEcologMM(InsertDatumList[i], this.UpdateText, InsertConfig.GpsCorrection.MapMatching);
                    }

                    if (IsCheckedNormal)
                    {
                        EcologInserter.InsertEcolog(InsertDatumList[i], this.UpdateText, InsertConfig.GpsCorrection.Normal,out count);
                    }

                
                //       sw.Stop();
                //      LogWritter.WriteLog(LogWritter.LogMode.Elapsedtime, "Total Time:" + sw.Elapsed);
                #endregion
            });
            this.LogText += LogTexts.TheEndOfTheInsertingEcolog + "\n";
            /*if (count > 0)
            {
                SlackUtil.commentToSlack(InsertConfig.StartDate, InsertConfig.EndDate, InsertConfig.Correction);
            }
            else {
                SlackUtil.commentToSlackNotInsert(InsertConfig.StartDate, InsertConfig.EndDate, InsertConfig.Correction);
            }*/
            IsEnabledInsertButton = true;
            IsEnabledStartUpLoopButton = true;
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
            if (this.IsCheckedLabMember)
                insertConfig.CheckeDrivers.Add(DriverNames.Arisimu);
			

            //if (this.IsCheckedLabMember)
            //insertConfig.CheckeDrivers.Add(DriverNames.Uemura);
            // TODO 研究室メンバー

            #endregion

            #region 期間の設定
            if (IsCheckedPeriod)
            {
                insertConfig.StartDate = this.StartDate;
                insertConfig.EndDate = this.EndDate;
            }
            else
            {
                insertConfig.StartDate = DateTime.Now.AddDays(-7);
                insertConfig.EndDate = DateTime.Now.AddDays(1);
            }
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
            if(this.IsCheckedNormal)
            insertConfig.Correction.Add(InsertConfig.GpsCorrection.Normal);
            if (this.IsCheckedMapMatching)
                insertConfig.Correction.Add(InsertConfig.GpsCorrection.MapMatching);
            //else if (this.IsCheckedDeadReckoning)
            //    insertConfig.Correction = InsertConfig.GpsCorrection.DeadReckoning;
            if (this.IsCheckedSpeedLPFMapMatching)
                insertConfig.Correction.Add(InsertConfig.GpsCorrection.SpeedLPFMapMatching);

            #endregion

            LogWritter.WriteLog(LogWritter.LogMode.Search, insertConfig.ToString());

            return insertConfig;
        }
    }
}
