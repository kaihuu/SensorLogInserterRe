using Codeplex.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Models;

namespace SensorLogInserterRe.Utils
{
    static class SlackUtil
    {
        static string WEBHOOK_URL = "https://hooks.slack.com/services/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";

        public static void commentToSlack(String text)
        {
            var wc = new WebClient();

            var data = DynamicJson.Serialize(new
            {
                text = "",
                icon_emoji = ":finish:", //アイコンを動的に変更する
                username = "SensorLogInserter"  //名前を動的に変更する
            });

            wc.Headers.Add(HttpRequestHeader.ContentType, "application/json;charset=UTF-8");
            wc.Encoding = Encoding.UTF8;


            wc.UploadString(WEBHOOK_URL, data);
        }

        public static void commentToSlack(DateTime startTime, DateTime endTime, InsertConfig.GpsCorrection correction)
        {
           

            var wc = new WebClient();

            var data = DynamicJson.Serialize(new
            {
                text = startTime.ToShortDateString() + " ～ " + endTime.ToShortDateString() + " 補正方法： " 
                + Enum.GetName(typeof(InsertConfig.GpsCorrection), correction),
                icon_emoji = ":finish:", //アイコンを動的に変更する
                username = "SensorLogInserter"  //名前を動的に変更する
            });

            wc.Headers.Add(HttpRequestHeader.ContentType, "application/json;charset=UTF-8");
            wc.Encoding = Encoding.UTF8;


            wc.UploadString(WEBHOOK_URL, data);
        }
    }
}
