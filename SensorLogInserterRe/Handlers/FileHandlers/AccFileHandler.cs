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




                    DataRow row = accRawTable.NewRow();

                    row.SetField(AndroidAccRawDao.ColumnDriverId, driverId);
                    row.SetField(AndroidAccRawDao.ColumnCarId, carId);
                    row.SetField(AndroidAccRawDao.ColumnSensorId, sensorId);
                    

                    if (fields != null)
                    {
                        DateTime androidTime = DateTime.Parse(fields[0]);

                        #region AndroidTimeの設定

                        if (androidTime.Year == 1970)
                        {
                            androidTime = androidTime.AddYears(42);
                            androidTime = androidTime.AddMonths(6);
                        }
                        row.SetField(AndroidAccRawDao.ColumnDateTime, androidTime);
                        #endregion

                        row.SetField(AndroidAccRawDao.ColumnAccX, fields[1]);
                        row.SetField(AndroidAccRawDao.ColumnAccY, fields[2]);
                        if (fields[3] == "")
                        {
                            row.SetField(AndroidAccRawDao.ColumnAccZ, 0);
                        }
                        else {
                            row.SetField(AndroidAccRawDao.ColumnAccZ, fields[3]);
                        }
                        accRawTable.Rows.Add(row);
                    }
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
                catch (ArgumentException)
                {

                }
            }

            parser.Close();
            return accRawTable;
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
