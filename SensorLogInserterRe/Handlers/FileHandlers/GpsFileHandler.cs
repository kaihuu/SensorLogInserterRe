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
        public static DataTable ConvertCsvToDataTable(string filePath, InsertDatum datum)
        {
            var parser = GetParser(filePath);

            var gpsRawTable = DataTableUtil.GetAndroidGpsRawTable();
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

                    row.SetField(AndroidGpsRawDao.ColumnLatitude, fields[2]); //　LATITUDE
                    row.SetField(AndroidGpsRawDao.ColumnLongitude, fields[3]); //　LONGITUDE

                    if(fields.Length > 4)//5列目（ALTITUDE)のカラムが存在する場合、その値を入力
                    { 
                        row.SetField(AndroidGpsRawDao.ColumnAltitude, fields[4]); //　ALTITUDE
                    }
                    else row.SetField(AndroidGpsRawDao.ColumnAltitude, -100000);//5列目（ALTITUDE)のカラムが存在しない場合、-100000を返す
                                                                                
                    //row.SetField(AndroidGpsRawDao.ColumnSpeed, fields[6]);
                    //row.SetField(AndroidGpsRawDao.ColumnBearing, fields[7]);

                    if (beforeJst != jst.ToString(StringUtil.JstFormat))
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
                catch (FormatException)
                {
                    // TODO エラー処理
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
