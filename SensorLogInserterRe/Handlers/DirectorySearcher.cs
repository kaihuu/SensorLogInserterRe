﻿using System;
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
                if (driver.Name.Equals(DriverNames.Tommy))
                    CheckFiles(DirectoryNames.DirectoryTommy, config.StartDate, config.EndDate, ref insertFileList);
                if (driver.Name.Equals(DriverNames.Mori))
                    CheckFiles(DirectoryNames.DirectoryMori, config.StartDate, config.EndDate, ref insertFileList);
                if (driver.Name.Equals(DriverNames.Tamura))
                    CheckFiles(DirectoryNames.DirectoryTamura, config.StartDate, config.EndDate, ref insertFileList);
                // TODO 研究室メンバー
            }

            return insertFileList;
        }

        private static void CheckFiles(string folderPass, DateTime startDate, DateTime endDate, ref List<string> insertFileList)
        {
            long todayLong = DateTimeUtil.ConvertDateTimeToLongFormatted(DateTime.Now);
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

                if (word[word.Length - 1] == "DrivingLoggerAppLog" || word[word.Length - 1] == "DrivingLoggerSentTempLogging"
                    || word[word.Length - 1] == "DrivingLoggerSentAppLog" || word[word.Length - 1] == "DrivingLoggerSentLog"
                    || word[word.Length - 1] == "ECOLOG_Config" || word[word.Length - 1] == "ErrorData" || word[word.Length - 2] == "DrivingLoggerCamera")
                {
                    continue;
                }

                try
                {
                    CheckFiles(directory, startDate, endDate, ref insertFileList);
                }
                //隠しファイルなどのアクセス許可のないファイルをキャッチ(何もしない)
                catch (System.UnauthorizedAccessException)
                {

                }
            }
        }
    }
}
