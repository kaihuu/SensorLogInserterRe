using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using SensorLogInserterRe.Daos;
using SensorLogInserterRe.Utils;

namespace SensorLogInserterRe.Handlers.FileHandlers
{
    class GpsFileHandler
    {
        public static DataTable ConvertCsvToDataTable(string filePath, int driverId, int carId, int sensorId)
        {
            var parser = GetParser(filePath);

            var gpsRawTable = DataTableUtil.GetAndroidGpsRawTable();
            string beforeJst = null;

            while ( !parser.EndOfData)
            {
                try
                {
                    string[] fields = parser.ReadFields();

                    DataRow dr = gpsRawTable.NewRow();

                    dr[AndroidGpsRawDao.ColumnDriverId] = driverId;
                    dr[AndroidGpsRawDao.ColumnCarId] = carId;
                    dr[AndroidGpsRawDao.ColumnSensorId] = sensorId;

                    DateTime jst = DateTime.Parse(fields[0].ToString());
                    DateTime androidTime = DateTime.Parse(fields[1].ToString());
                    TimeSpan span = jst - androidTime;

                    #region Jstの設定
                    //android端末で取得したGPSの時刻が1日進む現象への対処
                    if (span.TotalHours > 23 && span.TotalHours < 25)
                    {
                        jst = jst.AddDays(-1);
                        dr[AndroidGpsRawDao.ColumnJst] = jst.ToString(StringUtil.JstFormat);
                    }
                    else
                    {
                        dr[AndroidGpsRawDao.ColumnJst] = jst.ToString(StringUtil.JstFormat);
                    }
                    #endregion

                    #region AndroidTimeの設定
                    if (androidTime.Year == 1970)
                    {
                        androidTime = androidTime.AddYears(42);
                        androidTime = androidTime.AddMonths(6);
                        dr[AndroidGpsRawDao.ColumnAndroidTime] = androidTime.ToString(StringUtil.JstFormat);
                    }
                    else
                    {
                        dr[AndroidGpsRawDao.ColumnAndroidTime] = fields[1];
                    }
                    #endregion

                    dr[AndroidGpsRawDao.ColumnLatitude] = fields[2]; //　VALID
                    dr[AndroidGpsRawDao.ColumnLongitude] = fields[3]; //　LATITUDE
                    dr[AndroidGpsRawDao.ColumnAltitude] = fields[4]; //　LONGITUDE

                    if (beforeJst != jst.ToString(StringUtil.JstFormat))
                    {
                        gpsRawTable.Rows.Add(dr);
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
            TextFieldParser parser = new TextFieldParser(filePath, Encoding.GetEncoding(932));
            parser.TextFieldType = FieldType.Delimited;
            parser.Delimiters = new string[] { "," };
            parser.HasFieldsEnclosedInQuotes = true;
            parser.TrimWhiteSpace = true;

            return parser;
        }
    }
}
