using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Constant;
using SensorLogInserterRe.Models;
using SensorLogInserterRe.Utils;

namespace SensorLogInserterRe.Handlers
{
    static class DirectorySearcher
    {
        public static List<string> DirectorySearch(InsertConfig config)
        {
            List<string> insertFileList = new List<string>();

            foreach (var driver in config.CheckeDrivers)
            {
                if (driver.Name.Equals(DriverNames.Tommy))
                    insertFileList.AddRange( CheckFiles(DirectoryNames.DirectoryTommy, config.StartDate, config.EndDate) );
                if (driver.Name.Equals(DriverNames.Mori))
                    insertFileList.AddRange( CheckFiles(DirectoryNames.DirectoryMori, config.StartDate, config.EndDate) );
                if(driver.Name.Equals(DriverNames.Tamura))
                    insertFileList.AddRange( CheckFiles(DirectoryNames.DirectoryTamura, config.StartDate, config.EndDate) );
                // TODO 研究室メンバー
            }

            return insertFileList;
        }

        private static List<string> CheckFiles(string folderPass, DateTime startDate, DateTime endDate)
        {
            long todayLong = DateTimeUtil.ConvertDateTimeToLongFormatted(DateTime.Now);
            long startLong = DateTimeUtil.ConvertDateTimeToLongFormatted(startDate);
            long endLong = DateTimeUtil.ConvertDateTimeToLongFormatted(endDate);

            List<string> insertFileList = new List<string>();

            string[] files = System.IO.Directory.GetFiles(folderPass, "*");

            foreach (string fileName in files)
            {
                try
                {
                    var fileCreateTime = StringUtil.ConvertFileNameToCreatedTime(fileName);

                    if (fileCreateTime > startLong && fileCreateTime < endLong)
                    {
                        {
                            insertFileList.Add(fileName);
                            // TODO WriteLog(fileName, LogMode.upload);
                        }
                    }

                }
                //隠しファイルなどのアクセス許可のないファイルをキャッチ(何もしない)
                catch (System.UnauthorizedAccessException)
                {
                    // TODO WriteLog
                }
                //AutoControlGDLTimestamp等の時刻がファイル名に入っていないファイルのFormatExceptionをキャッチ
                catch (System.FormatException)
                {
                    // TODO WriteLog
                }
            }

            return insertFileList;
        }
    }
}
