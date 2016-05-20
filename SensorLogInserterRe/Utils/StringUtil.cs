using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Utils
{
    static class StringUtil
    {
        public static long ConvertFileNameToCreatedTime(string fileName)
        {
            //ファイル名を区切る
            string[] word = fileName.Split('\\');

            if (word[word.Length - 1].Length >= 14)
            {
                // yyyyMMddhhmmss の 14 文字
                long fileCreatedTime = long.Parse(word[word.Length - 1].Substring(0, 14));

                return fileCreatedTime;
            }

            return 0;
        }

    }
}
