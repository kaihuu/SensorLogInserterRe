using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Models;

namespace SensorLogInserterRe.Daos
{
    class CorrectedAccDao
    {
        private static readonly string TableName = "corrected_acc";
        public static readonly string ColumnDriverId = "driver_id";
        public static readonly string ColumnCarId = "car_id";
        public static readonly string ColumnSensorId = "sensor_id";
        public static readonly string ColumnJst = "jst";
        public static readonly string ColumnLongitudinalAcc = "longitudinal_acc";
        public static readonly string ColumnLateralAcc = "lateral_acc";
        public static readonly string ColumnVerticalAcc = "vertical_acc";
        public static readonly string ColumnRoll = "roll";
        public static readonly string ColumnPitch = "pitch";
        public static readonly string ColumnYaw = "yaw";
        public static readonly string ColumnAlpha = "alpha";
        public static readonly string ColumnVectorX = "vector_x";
        public static readonly string ColumnVectorY = "vector_y";
        public static readonly string ColumnBeta = "beta";
        public static readonly string ColumnGamma = "gamma";

        public static void Insert(DataTable dataTable)
        {
            DatabaseAccesser.Insert(TableName, dataTable);
        }

        public static DataTable Get()
        {
            string query = "SELECT * FROM " + TableName;

            return DatabaseAccesser.GetResult(query);
        }

        /***
         * このレベルになるとDB側にテーブル関数作ったほうがいいよね...
         * 頑張れる方、リファクタリングしてください
         * Elizabeth
         * Your tears and my fears are almost disappear
         * So let's share the perfect time,For you and me
         * You knocked on my door
         * So let's start our journey 
         * Because you came to see me first
         ***/
        public static DataTable GetAccurateStoppingAccRaw(int timeDiff, InsertDatum datum)
        {
            string query = "with LOW_SPEED as ";
            query += "( ";
            query += "	select DRIVER_ID,SENSOR_ID,DATEADD(second,-1,JST) as START_TIME, JST as END_TIME ";
            query += "	from CORRECTED_GPS ";
            query += "	where DRIVER_ID = " + datum.DriverId + " ";
            query += "	and SENSOR_ID = " + datum.SensorId + " ";
            query += "	and SPEED < 1 ";
            query += "	and JST >= '" + datum.StartTime + "' ";
            query += "	and JST <= '" + datum.EndTime + "' ";
            query += ") ";
            query += ", UPPER_TIME as ";
            query += "( ";
            query += "	select ROW_NUMBER() over(order by START_TIME) as NUMBER, DRIVER_ID, SENSOR_ID, START_TIME ";
            query += "	from ";
            query += "	( ";
            query += "		select DRIVER_ID,SENSOR_ID,START_TIME ";
            query += "		from LOW_SPEED ";
            query += "		except ";
            query += "		select DRIVER_ID,SENSOR_ID,END_TIME ";
            query += "		from LOW_SPEED ";
            query += "	) as UP ";
            query += ") ";
            query += ", LOWER_TIME as ";
            query += "( ";
            query += "	select ROW_NUMBER() over(order by END_TIME) as NUMBER, DRIVER_ID,SENSOR_ID,END_TIME ";
            query += "	from ";
            query += "	( ";
            query += "		select DRIVER_ID,SENSOR_ID,END_TIME ";
            query += "		from LOW_SPEED ";
            query += "		except ";
            query += "		select DRIVER_ID,SENSOR_ID,START_TIME ";
            query += "		from LOW_SPEED ";
            query += "	) as LOW ";
            query += ") ";
            query += ", LOW_SPEED_SPAN as ";
            query += "( ";
            query += "	select LOWER_TIME.DRIVER_ID,LOWER_TIME.SENSOR_ID,UPPER_TIME.START_TIME,LOWER_TIME.END_TIME ";
            query += "	from LOWER_TIME,UPPER_TIME ";
            query += "	where LOWER_TIME.NUMBER = UPPER_TIME.NUMBER ";
            query += ") ";

            query += "select AVG(ACC_X) as ACC_X,AVG(ACC_Y) as ACC_Y,AVG(ACC_Z) as ACC_Z ";
            query += "from ANDROID_ACC_RAW,LOW_SPEED_SPAN ";
            query += "where ANDROID_ACC_RAW.DRIVER_ID = LOW_SPEED_SPAN.DRIVER_ID ";
            query += "and ANDROID_ACC_RAW.SENSOR_ID = LOW_SPEED_SPAN.SENSOR_ID ";
            query += "and ANDROID_ACC_RAW.DATETIME <= DATEADD(MILLISECOND,-1*" + timeDiff + ",LOW_SPEED_SPAN.END_TIME) ";
            query += "and ANDROID_ACC_RAW.DATETIME > DATEADD(MILLISECOND,-1*" + timeDiff + ",LOW_SPEED_SPAN.START_TIME) ";

            return  DatabaseAccesser.GetResult(query);
        }

        public static DataTable GetAccurateAccBreakingRaw(DateTime startTime, DateTime endTime, InsertDatum datum)
        {
            string query = "with LOW_SPEED as ";
            query += "( ";
            query += "	select g1.DRIVER_ID,g1.SENSOR_ID,g1.JST ";
            query += "	from CORRECTED_GPS g1,CORRECTED_GPS g2 ";
            query += "	where g1.DRIVER_ID = " + datum.DriverId + " ";
            query += "	and g1.SENSOR_ID = " + datum.SensorId + " ";
            query += "	and g1.SPEED < 10 ";
            query += "	and g2.DRIVER_ID = " + datum.DriverId + " ";
            query += "	and g2.SENSOR_ID = " + datum.SensorId + " ";
            query += "	and g2.SPEED > 10 ";
            query += "	and g1.JST = DATEADD(second,1,g2.JST) ";
            query += "	and g1.JST > '" + startTime + "' ";
            query += "	and g1.JST < '" + endTime + "' ";
            query += ") ";
            query += "select * ";
            query += "from LOW_SPEED ";
            query += "except ";
            query += "select LOW_SPEED.DRIVER_ID,LOW_SPEED.SENSOR_ID,CONVERT(varchar,LOW_SPEED.JST,121) as JST ";
            query += "from CORRECTED_GPS,LOW_SPEED ";
            query += "where CORRECTED_GPS.DRIVER_ID = LOW_SPEED.DRIVER_ID ";
            query += "and CORRECTED_GPS.SENSOR_ID = LOW_SPEED.SENSOR_ID ";
            query += "and CORRECTED_GPS.JST < LOW_SPEED.JST ";
            query += "and CORRECTED_GPS.JST >= DATEADD(second,-10,LOW_SPEED.JST) ";
            query += "and SPEED < 10 ";
            query += "group by LOW_SPEED.DRIVER_ID,LOW_SPEED.SENSOR_ID,LOW_SPEED.JST ";

            return DatabaseAccesser.GetResult(query);
        }
    }
}
