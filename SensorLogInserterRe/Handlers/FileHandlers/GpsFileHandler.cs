using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using SensorLogInserterRe.Daos;
using SensorLogInserterRe.Models;
using SensorLogInserterRe.Utils;

namespace SensorLogInserterRe.Handlers.FileHandlers
{
    class GpsFileHandler
    {
        private static readonly DateTime NmeaStartDate = DateTime.Parse("2016-05-13 23:58:43.000");
        public static DataTable ConvertCsvToDataTable(string filePath, InsertDatum datum, InsertConfig.GpsCorrection correction)
        {
            var parser = GetParser(filePath);

            var gpsRawTable = new DataTable();
            if (correction == InsertConfig.GpsCorrection.DopplerSpeed)
            {
                gpsRawTable = DataTableUtil.GetAndroidGpsRawDopplerTable();
            }
            else
            {
                gpsRawTable = DataTableUtil.GetAndroidGpsRawTable();
            }
            string beforeJst = null;

            while (!parser.EndOfData)
            {
                try
                {
                    string[] fields = parser.ReadFields();

                    DataRow row = gpsRawTable.NewRow();

                    row.SetField(AndroidGpsRawDao.ColumnDriverId, datum.DriverId);
                    row.SetField(AndroidGpsRawDao.ColumnCarId, datum.CarId);
                    row.SetField(AndroidGpsRawDao.ColumnSensorId, datum.SensorId);

                    DateTime jst = DateTime.Parse(fields[0].ToString());
                    DateTime androidTime = DateTime.Parse(fields[1].ToString());
                    TimeSpan span = jst - androidTime;

                    #region Jstの設定
                    //android端末で取得したGPSの時刻が1日進む現象への対処
                    if (span.TotalHours > 23 && span.TotalHours < 25)
                        jst = jst.AddDays(-1);

                    row.SetField<DateTime>(AndroidGpsRawDao.ColumnJst, jst);
                    #endregion

                    #region AndroidTimeの設定

                    if (androidTime.Year == 1970)
                    {
                        androidTime = androidTime.AddYears(42);
                        androidTime = androidTime.AddMonths(6);
                    }
                    row.SetField(AndroidGpsRawDao.ColumnAndroidTime, androidTime);
                    // TODO string から DateTimeに変えて影響がないか 
                    // row[AndroidGpsRawDao.ColumnAndroidTime] = androidTime.ToString(StringUtil.JstFormat);

                    #endregion

                    row.SetField(AndroidGpsRawDao.ColumnLatitude, fields[2]); //　VALID
                    row.SetField(AndroidGpsRawDao.ColumnLongitude, fields[3]); //　LATITUDE
                    row.SetField(AndroidGpsRawDao.ColumnAltitude, fields[4]); //　LONGITUDE
                    if (correction == InsertConfig.GpsCorrection.DopplerSpeed && jst < NmeaStartDate)
                    {

                    }
                    else if (beforeJst != jst.ToString(StringUtil.JstFormat))
                    {
                        gpsRawTable.Rows.Add(row);
                    }


                    beforeJst = jst.ToString(StringUtil.JstFormat);
                }
                catch (NullReferenceException nre)
                {
                    // TODO エラー処理
                    continue;
                }
                catch (IndexOutOfRangeException iore)
                {
                    // TODO エラー処理
                    continue;
                }
                catch (FormatException fe)
                {
                    Console.WriteLine(fe.Message);
                    // TODO エラー処理
                }
                catch (ArgumentException ae)
                {
                    Console.WriteLine(ae.Message);
                }
            }

            parser.Close();
            return gpsRawTable;
        }
        public static DataTable ConvertCsvToDataTableDoppler(string filePath, InsertDatum datum, InsertConfig.GpsCorrection correction)
        {
            var parser = GetParser(filePath);

            var gpsRawTable = new DataTable();
            if(correction == InsertConfig.GpsCorrection.DopplerSpeed)
            {
                gpsRawTable = DataTableUtil.GetAndroidGpsRawDopplerTable();
            }
            else
            {
                gpsRawTable = DataTableUtil.GetAndroidGpsRawTable();
            }
            string beforeJst = null;

            while (!parser.EndOfData)
            {
                try
                {
                    string[] fields = parser.ReadFields();

                    DataRow row = gpsRawTable.NewRow();

                    row.SetField(AndroidGpsRawDao.ColumnDriverId, datum.DriverId);
                    row.SetField(AndroidGpsRawDao.ColumnCarId, datum.CarId);
                    row.SetField(AndroidGpsRawDao.ColumnSensorId, datum.SensorId);

                    DateTime jst = DateTime.Parse(fields[0].ToString());
                    DateTime androidTime = DateTime.Parse(fields[1].ToString());
                    TimeSpan span = jst - androidTime;

                    #region Jstの設定
                    //android端末で取得したGPSの時刻が1日進む現象への対処
                    if (span.TotalHours > 23 && span.TotalHours < 25)
                        jst = jst.AddDays(-1);

                    row.SetField<DateTime>(AndroidGpsRawDao.ColumnJst, jst);
                    #endregion

                    #region AndroidTimeの設定

                    if (androidTime.Year == 1970)
                    {
                        androidTime = androidTime.AddYears(42);
                        androidTime = androidTime.AddMonths(6);
                    }
                    row.SetField(AndroidGpsRawDao.ColumnAndroidTime, androidTime);
                    // TODO string から DateTimeに変えて影響がないか 
                    // row[AndroidGpsRawDao.ColumnAndroidTime] = androidTime.ToString(StringUtil.JstFormat);

                    #endregion

                    row.SetField(AndroidGpsRawDao.ColumnLatitude, fields[2]); //　VALID
                    row.SetField(AndroidGpsRawDao.ColumnLongitude, fields[3]); //　LATITUDE
                    row.SetField(AndroidGpsRawDao.ColumnAltitude, fields[4]); //　LONGITUDE
                    row.SetField(AndroidGpsRawDopplerDao.ColumnAccuracy, (int)float.Parse(fields[5]));// ACCURACY
                    if (fields.Length > 6)
                    {
                        double speed = Convert.ToDouble(fields[6]);
                        speed = speed * 3.6;
                        row.SetField(AndroidGpsRawDopplerDao.ColumnSpeed, speed); //SPEED
                        row.SetField(AndroidGpsRawDopplerDao.ColumnBearing, fields[7]); //BEARING
                    }
                    else
                    {
                        row.SetField(AndroidGpsRawDopplerDao.ColumnSpeed, DBNull.Value);
                        row.SetField(AndroidGpsRawDopplerDao.ColumnBearing, DBNull.Value);
                    }
                    if (correction == InsertConfig.GpsCorrection.DopplerSpeed && jst < NmeaStartDate)
                    {

                    }
                    else if (beforeJst != jst.ToString(StringUtil.JstFormat))
                    {
                            gpsRawTable.Rows.Add(row);
                     }
                    

                    beforeJst = jst.ToString(StringUtil.JstFormat);
                }
                catch (NullReferenceException nre)
                {
                    // TODO エラー処理
                    continue;
                }
                catch (IndexOutOfRangeException iore)
                {
                    // TODO エラー処理
                    continue;
                }
                catch (FormatException fe)
                {
                    Console.WriteLine(fe.Message);
                    // TODO エラー処理
                }
                catch (ArgumentException ae)
                {
                    Console.WriteLine(ae.Message);
                }
            }

            parser.Close();
            return gpsRawTable;
        }

        private static TextFieldParser GetParser(string filePath)
        {
            TextFieldParser parser = new TextFieldParser(filePath, Encoding.GetEncoding(932))
            {
                TextFieldType = FieldType.Delimited,
                Delimiters = new string[] { "," },
                HasFieldsEnclosedInQuotes = true,
                TrimWhiteSpace = true
            };

            return parser;
        }
    }
}
