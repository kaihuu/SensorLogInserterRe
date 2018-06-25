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
            var insertFileList = new List<string>();

            foreach (var driver in config.CheckeDrivers)
            {
                if (driver.Equals(DriverNames.Tommy))
                    CheckFiles(DirectoryNames.DirectoryTommy, config.StartDate, config.EndDate, insertFileList);
                if (driver.Equals(DriverNames.Mori))
                    CheckFiles(DirectoryNames.DirectoryMori, config.StartDate, config.EndDate, insertFileList);
                if (driver.Equals(DriverNames.Tamura))
                    CheckFiles(DirectoryNames.DirectoryTamura, config.StartDate, config.EndDate, insertFileList);
                if (driver.Equals(DriverNames.Uemura))
                    CheckFiles(DirectoryNames.DirectoryUemura, config.StartDate, config.EndDate, insertFileList);
                if(driver.Equals(DriverNames.Simulation))
                    CheckFiles(DirectoryNames.DirectorySimulation, config.StartDate, config.EndDate, insertFileList);
				if (driver.Equals(DriverNames.Arisimu))
					CheckFiles(DirectoryNames.DirectoryArisimu, config.StartDate, config.EndDate, insertFileList);

				// TODO 研究室メンバー
			}

			return insertFileList;
        }

        private static void CheckFiles(string folderPass, DateTime startDate, DateTime endDate, List<string> insertFileList)
        {
            long startLong = DateTimeUtil.ConvertDateTimeToLongFormatted(startDate);
            long endLong = DateTimeUtil.ConvertDateTimeToLongFormatted(endDate);

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
                    
                }
                //AutoControlGDLTimestamp等の時刻がファイル名に入っていないファイルのFormatExceptionをキャッチ
                catch (System.FormatException)
                {
                    
                }
            }

            //フォルダ直下のフォルダを検索
            string[] directories = System.IO.Directory.GetDirectories(folderPass);

            foreach (string directory in directories)
            {
                //ファイル名を区切る
                string[] word = directory.Split('\\');

                if (word[word.Length - 1] == "DrivingLoggerAppLog" || word[word.Length - 1] == "DrivingLoggerTempLogging"
                    || word[word.Length - 1] == "ECOLOG_Config" || word[word.Length - 1] == "ErrorData" || word[word.Length - 2] == "DrivingLoggerCamera")
                {
                    continue;
                }

                try
                {
                    CheckFiles(directory, startDate, endDate, insertFileList);
                }
                //隠しファイルなどのアクセス許可のないファイルをキャッチ(何もしない)
                catch (System.UnauthorizedAccessException)
                {

                }
            }
        }
    }
}
