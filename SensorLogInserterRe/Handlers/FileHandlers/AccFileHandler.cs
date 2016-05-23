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
    class AccFileHandler
    {
        public static DataTable ConvertCsvToDataTable(string filePath, int driverId, int carId, int sensorId)
        {
            var parser = GetParser(filePath);

            var accRawTable = DataTableUtil.GetAndroidAccRawTable();

            while (!parser.EndOfData)
            {
                try
                {
                    string[] fields = parser.ReadFields();

                    DataRow dr = accRawTable.NewRow();

                    dr[AndroidGpsRawDao.ColumnDriverId] = driverId;
                    dr[AndroidGpsRawDao.ColumnCarId] = carId;
                    dr[AndroidGpsRawDao.ColumnSensorId] = sensorId;

                    //

                    DateTime androidTime = DateTime.Parse(fields[0].ToString());

                    #region AndroidTimeの設定
                    if (androidTime.Year == 1970)
                    {
                        androidTime = androidTime.AddYears(42);
                        androidTime = androidTime.AddMonths(6);
                        dr[AndroidAccRawDao.ColumnDateTime] = androidTime.ToString(StringUtil.JstFormat);
                    }
                    else
                    {
                        dr[AndroidAccRawDao.ColumnDateTime] = fields[0];
                    }
                    #endregion

                    dr[AndroidAccRawDao.ColumnAccX] = fields[1];
                    dr[AndroidAccRawDao.ColumnAccY] = fields[2];
                    dr[AndroidAccRawDao.ColumnAccZ] = fields[3];

                    accRawTable.Rows.Add(dr);
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
            return accRawTable;
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
