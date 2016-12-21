using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Daos
{
    class LinkDao
    {
        private static readonly string TableName = "links";

        public static void Insert(DataTable dataTable)
        {
            DatabaseAccesser.Insert(TableName, dataTable);
        }

        public static DataTable Get()
        {
            string query = "SELECT * FROM " + TableName;

            return DatabaseAccesser.GetResult(query);
        }
        public static DataTable GetLinkId(int Latitude, int Longitude)
        {
            double maxLatitude = System.Convert.ToDouble(Latitude + 20) / 10000.0;
            double minLatitude = System.Convert.ToDouble(Latitude - 20) / 10000.0;
            double maxLongitude = System.Convert.ToDouble(Longitude + 20) / 10000.0;
            double minLongitude = System.Convert.ToDouble(Longitude - 20) / 10000.0;
            string query = "with LINKS_TABLE as (SELECT LINKS.* ";
            query += $"FROM LINKS ";
            query += $"WHERE latitude >= {minLatitude} AND longitude >= {minLongitude} AND latitude <= {maxLatitude} AND longitude <= {maxLongitude} AND ";
            query += $"{TableName}.NUM = LINKS.NUM AND {TableName}.LINK_ID = LINKS.LINK_ID), ";
            query += " DIRECTION as ";
            query += "( ";
            query += "select LINK_ID, LAT1,LON1,LAt2,LON2, ";
            query += " case ";
            query += " when ATN2(Y,X) * 180 / PI() >= 0 then ATN2(Y,X) * 180 / PI() ";
            query += " when ATN2(Y,X) * 180 / PI() < 0 then ATN2(Y,X) * 180 / PI() + 180 ";
            query += " end as HEADING,A,B,C ";
            query += " from ( ";
            query += " select *,ROUND((COS(RAD_LAT2) * SIN(RAD_LON2 - RAD_LON1)* 1000000),2) as Y, ROUND((COS(RAD_LAT1) * SIN(RAD_LAT2) - SIN(RAD_LAT1) * COS(RAD_LAT2) * COS(RAD_LON2 - RAD_LON1))*1000000,2) as X, LAT2-LAT1 as A,LON1-LON2 as B,LON2+LAT1-LON1*LAT2 as C ";
            query += " from( ";
            query += " select node1.LINK_ID, node1.LATITUDE as LAT1, node1. LONGITUDE as LON1, node2.LATITUDE as LAT2, node2.LONGITUDE as LON2, node1.LATITUDE * PI() / 180 as RAD_LAT1, node1.LONGITUDE * PI() / 180 as RAD_LON1, node2.LATITUDE * PI() / 180 as RAD_LAT2, node2.LONGITUDE * PI() / 180 as RAD_LON2 ";
            query += " from ( ";
            query += " select * ";
            query += " from LINKS ";
            query += " where NODE_ID is not null ";
            query += " and DIRECTION = 1 ";
            query += " ) as node1,( ";
            query += " select * ";
            query += " from LINKS ";
            query += " where NODE_ID is not null ";
            query += " and DIRECTION = 2 ";
            query += " ) as node2 ";
            query += " where node1.LINK_ID = node2.LINK_ID ";
            query += " ) as rad ";
            query += ") as XY ";
            query += "where (X != 0 ";
            query += "or Y != 0) ";
            query += ") ";
            query += "select LINKS.LINK_ID,LINKS.LATITUDE,LINKS.LONGITUDE,LAT1,LON1,LAt2,LON2,DIRECTION.HEADING,A,B,C ";
            query += "from LINKS_TABLE AS LINKS ";
            query += "left join DIRECTION on LINKS.LINK_ID = DIRECTION.LINK_ID ";

            return DatabaseAccesser.GetResult(query);
        }

        public static DataTable GetLinkTableWithHeadingAndLine()
        {
            // TODO テーブル関数に置き換えると疎結合にできるね

            string query = "with DIRECTION as ";
            query += "( ";
            query += "select LINK_ID, LAT1,LON1,LAt2,LON2, ";
            query += "	case ";
            query += "	when ATN2(Y,X) * 180 / PI() >= 0 then ATN2(Y,X) * 180 / PI() ";
            query += "	when ATN2(Y,X) * 180 / PI() < 0 then ATN2(Y,X) * 180 / PI() + 180 ";
            query += "	end as HEADING,A,B,C ";
            query += "from ( ";
            query += "	select *,ROUND((COS(RAD_LAT2) * SIN(RAD_LON2 - RAD_LON1)* 1000000),2) as Y, ROUND((COS(RAD_LAT1) * SIN(RAD_LAT2) - SIN(RAD_LAT1) * COS(RAD_LAT2) * COS(RAD_LON2 - RAD_LON1))*1000000,2) as X, LAT2-LAT1 as A,LON1-LON2 as B,LON2+LAT1-LON1*LAT2 as C  ";
            query += "	from( ";
            query += "		select node1.LINK_ID, node1.LATITUDE as LAT1, node1. LONGITUDE as LON1, node2.LATITUDE as LAT2, node2.LONGITUDE as LON2, node1.LATITUDE * PI() / 180 as RAD_LAT1, node1.LONGITUDE * PI() / 180 as RAD_LON1, node2.LATITUDE * PI() / 180 as RAD_LAT2, node2.LONGITUDE * PI() / 180 as RAD_LON2 ";
            query += "		from ( ";
            query += "			select * ";
            query += "			from LINKS ";
            query += "			where NODE_ID is not null ";
            query += "			and DIRECTION = 1 ";
            query += "		) as node1,( ";
            query += "			select * ";
            query += "			from LINKS ";
            query += "			where NODE_ID is not null ";
            query += "			and DIRECTION = 2 ";
            query += "		) as node2 ";
            query += "		where node1.LINK_ID = node2.LINK_ID ";
            query += "	) as rad ";
            query += ") as XY ";
            query += "where (X != 0 ";
            query += "or Y != 0) ";
            query += ") ";
            query += "select LINKS.LINK_ID,LINKS.LATITUDE,LINKS.LONGITUDE,LAT1,LON1,LAt2,LON2,DIRECTION.HEADING,A,B,C ";
            query += "from LINKS ";
            query += "left join DIRECTION on LINKS.LINK_ID = DIRECTION.LINK_ID ";

            return DatabaseAccesser.GetResult(query);
        }
        public static DataTable GetLinkTableforMM(int[] id)
        {
            string query = "select l1.LINK_ID as LINK_ID , l1.NUM, l1.LATITUDE as START_LAT, l1.LONGITUDE as START_LONG,l2.LATITUDE as END_LAT, l2.LONGITUDE as END_LONG ";
            query += ",SQRT((l1.LATITUDE - l2.LATITUDE) * (l1.LATITUDE - l2.LATITUDE) + (l1.LONGITUDE - l2.LONGITUDE) * (l1.LONGITUDE - l2.LONGITUDE)) as DISTANCE  ";
            query += "from LINKS as l1,LINKS as l2,( ";
            query += "select l1.NUM,MIN(l2.NUM - l1.NUM) as diff ";
            query += "from LINKS as l1,LINKS as l2,SEMANTIC_LINKS  ";
            query += "where l1.NUM < l2.NUM  ";
            query += "and l1.LINK_ID = l2.LINK_ID  ";
            query += "and l1.LINK_ID = SEMANTIC_LINKS.LINK_ID  ";
            query += "and SEMANTIC_LINK_ID in ( " + id[0];

            for (int i = 1; i < id.Length; i++)
            {
                query += ", " + id;
            }

            query += ") ";
            query += "group by l1.NUM) as Corres ";
            query += "where l1.NUM = Corres.NUM ";
            query += "and l2.NUM = l1.NUM + Corres.diff ";
            query += "order by NUM ";

            return DatabaseAccesser.GetResult(query);
        }
    }
}
