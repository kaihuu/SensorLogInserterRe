using SensorLogInserterRe.Daos;
using SensorLogInserterRe.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using MathNet.Numerics.IntegralTransforms;

namespace SensorLogInserterRe.Cleansers.Components
{
    static class LowPassFilter
    {
        private static void CopyRawDataToCorrectedRow(DataRow correctedRow, DataRow rawRow)
        {
            correctedRow.SetField(CorrectedGpsSpeedLPF005MMDao.ColumnDriverId, rawRow.Field<int>(CorrectedGpsDao.ColumnDriverId));
            correctedRow.SetField(CorrectedGpsSpeedLPF005MMDao.ColumnCarId, rawRow.Field<int>(CorrectedGpsDao.ColumnCarId));
            correctedRow.SetField(CorrectedGpsSpeedLPF005MMDao.ColumnSensorId, rawRow.Field<int>(CorrectedGpsDao.ColumnSensorId));
            correctedRow.SetField(CorrectedGpsSpeedLPF005MMDao.ColumnJst, rawRow.Field<DateTime>(CorrectedGpsDao.ColumnJst));
            correctedRow.SetField(CorrectedGpsSpeedLPF005MMDao.ColumnLatitude, rawRow.Field<double>(CorrectedGpsDao.ColumnLatitude));
            correctedRow.SetField(CorrectedGpsSpeedLPF005MMDao.ColumnLongitude, rawRow.Field<double>(CorrectedGpsDao.ColumnLongitude));
            correctedRow.SetField(CorrectedGpsSpeedLPF005MMDao.ColumnDistanceDifference, rawRow.Field<float>(CorrectedGpsDao.ColumnDistanceDifference));
            correctedRow.SetField(CorrectedGpsSpeedLPF005MMDao.ColumnHeading, rawRow.Field<float>(CorrectedGpsDao.ColumnHeading));
        }
        public static DataTable speedLowPassFilter(DataTable correctedGpsTable, double cutOffFrequency)
        {
            DataTable correctedGpsSpeedLPFTable = DataTableUtil.GetCorrectedGpsTable();

            #region ローパスフィルタ適用
            double[] speed = convertDataRowToDoubleRow(correctedGpsTable);

            Boolean[] speedbool = new Boolean[speed.Length];
            for (int i = 0; i < speedbool.Length; i++)
            {
                if (speed[i] == 0)//スピードがゼロのインデックスを記録
                {
                    speedbool[i] = true;
                }
                else
                {
                    speedbool[i] = false;
                }
            }
            Complex[] data = LowPassFilter.fourier(speed);
            double[] frequencyScale = LowPassFilter.getFrequencyScale(speed, 1);
            Complex[] filteredData = LowPassFilter.applyLowPassFilter(data, frequencyScale, cutOffFrequency);//ローパスフィルタ＆逆フーリエ変換

            for(int i = 0;i < speedbool.Length; i++)
            {
                if(speedbool[i])
                {
                    filteredData[i] = new Complex(0, filteredData[i].Imaginary);//もともと車速がゼロのデータを逆フーリエ変換後もゼロに
                }
            }

            #endregion

            #region インデックスが 0 の場合
            DataRow firstRow = correctedGpsSpeedLPFTable.NewRow();
            CopyRawDataToCorrectedRow(firstRow, correctedGpsTable.Rows[0]);
            firstRow.SetField(CorrectedGpsDao.ColumnSpeed, filteredData[0].Real);

            correctedGpsSpeedLPFTable.Rows.Add(firstRow);
            #endregion


            for (int i = 1; i < correctedGpsTable.Rows.Count - 1; i++)
            {
                DataRow row = correctedGpsSpeedLPFTable.NewRow();

                CopyRawDataToCorrectedRow(row, correctedGpsTable.Rows[i]);

                row.SetField(CorrectedGpsSpeedLPF005MMDao.ColumnSpeed, filteredData[i].Real);


                correctedGpsSpeedLPFTable.Rows.Add(row);
            }

            #region インデックスが最後の場合
            DataRow lastRow = correctedGpsSpeedLPFTable.NewRow();
            CopyRawDataToCorrectedRow(lastRow, correctedGpsTable.Rows[correctedGpsTable.Rows.Count - 1]);
            lastRow.SetField(CorrectedGpsDao.ColumnSpeed, filteredData[correctedGpsTable.Rows.Count - 1].Real);

            correctedGpsSpeedLPFTable.Rows.Add(lastRow);

            #endregion
            return correctedGpsSpeedLPFTable;
        }
        private static Complex[] fourier(double[] data)
        {
            Complex[] complexData = new Complex[data.Length];
            int count = 0;
            foreach (double x in data)
            {
                complexData[count] = x;
                count++;
            }
            Fourier.Forward(complexData, FourierOptions.Default);//フーリエ変換
            return complexData;
        }
        private static double[] getFrequencyScale(double[] data, double samplingRate)
        {
            double[] result = Fourier.FrequencyScale(data.Length, samplingRate);//周波数スケールを取得
            return result;
        }
        private static Complex[] applyLowPassFilter(Complex[] data, double[] frequencyScale, double cutOffFrequency)
        {
            Complex[] result = data;
            for (int i = 0; i < frequencyScale.Length; i++)
            {
                if (frequencyScale[i] > cutOffFrequency)
                {
                    result[i] = 0;
                }
                else if (frequencyScale[i] < -cutOffFrequency)
                {
                    result[i] = 0;
                }
            }
            Fourier.Inverse(result, FourierOptions.Default);
            return result;
        }

        private static double[] convertDataRowToDoubleRow(DataTable dt)
        {
            double[] result = new double[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                result[i] = Convert.ToDouble(dt.Rows[i]["SPEED"]);
            }
            return result;
        }
    }
}
