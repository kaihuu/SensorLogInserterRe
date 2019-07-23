using Codeplex.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Models;
using System.Data;
using SensorLogInserterRe.Daos;

namespace SensorLogInserterRe.Utils
{
    static class SlackUtil
    {
        static string WEBHOOK_URL_uemura = "https://hooks.slack.com/services/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";//uemura
        static string WEBHOOK_URL = "https://hooks.slack.com/services/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";//ecolog
        static string WEBHOOK_URL_notice_ecologinserter = "https://hooks.slack.com/services/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";//notice

        public static void insertIsFinished(DateTime startTime, DateTime endTime, List<InsertConfig.GpsCorrection> correction)
        {
            string text = "Insert is finished:";

            text = joinFinishMessage(text, startTime, endTime, correction);

            commentToSlack(text);
        }
        public static void noInsertFile(DateTime startTime, DateTime endTime, List<InsertConfig.GpsCorrection> correction)
        {
            string text = "No Insert File:";

            text = joinFinishMessage(text, startTime, endTime, correction);

            commentToSlack(text);
        }
        public static void noSensorData(string filePath)
        {
            string text = "No Sensor Data: You must add or change SENSOR_NAME table, File Path is " + filePath;

            commentToSlack(text);
        }
        public static void noMapMatching(InsertDatum insertDatum, DataRow dataRow)
        {
            string text = "There is a out of registered links   DriverID: " + insertDatum.DriverId
                + "StartTime: " + dataRow.Field<DateTime>(AndroidGpsRawDao.ColumnJst);

            commentToSlack(text);
        }

        public static string joinFinishMessage(string text, DateTime startTime, DateTime endTime, List<InsertConfig.GpsCorrection> correction)
        {
            string correctionMethod = getCorrectionMethod(correction);
            text = text + startTime.ToShortDateString() + " ～ " + endTime.ToShortDateString() + " 補正方法： "
                    + correctionMethod;

            return text;
        }


        public static void commentToSlack(string text)
        {
            var data = generateJson(text);

            uploadToSlack(data);
        }

        //public static void commentToSlackUemura(string text)
        //{
        //    var data = generateJson(text);

        //    uploadToSlackUemura(data);
        //}


        private static string generateJson(string text)
        {
            string data = DynamicJson.Serialize(new
            {
                text = text,
                icon_emoji = ":finish:", //アイコンを動的に変更する
                username = "SensorLogInserter"  //名前を動的に変更する
            });
            return data;
        }

        private static void uploadToSlack(string data)
        {
            var wc = new WebClient();

            wc.Headers.Add(HttpRequestHeader.ContentType, "application/json;charset=UTF-8");
            wc.Encoding = Encoding.UTF8;


            //wc.UploadString(WEBHOOK_URL_notice_ecologinserter, data);
        }

        //private static void uploadToSlackUemura(string data)
        //{
        //    var wc = new WebClient();

        //    wc.Headers.Add(HttpRequestHeader.ContentType, "application/json;charset=UTF-8");
        //    wc.Encoding = Encoding.UTF8;


        //    wc.UploadString(WEBHOOK_URL_notice_ecologinserter, data);
        //}

        private static string getCorrectionMethod(List<InsertConfig.GpsCorrection> correction)
        {
            String correctionMethod = Enum.GetName(typeof(InsertConfig.GpsCorrection), correction[0]);

            for (int i = 1; i < correction.Count; i++)
            {
                correctionMethod += ", " + Enum.GetName(typeof(InsertConfig.GpsCorrection), correction[i]);
            }

            return correctionMethod;
        }
    }
}
