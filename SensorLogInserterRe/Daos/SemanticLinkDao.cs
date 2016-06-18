using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Daos
{
    class SemanticLinkDao
    {
        private static readonly string TableName = "semantic_links";

        public static void Insert(DataTable dataTable)
        {
            DatabaseAccesser.Insert(TableName, dataTable);
        }

        public static DataTable Get()
        {
            string query = "SELECT * FROM " + TableName;

            return DatabaseAccesser.GetResult(query);
        }

        public static DataTable GetSemanticLinkTableWithHeadingAndLine()
        {
            // TODO テーブル関数にすると疎結合にできるね

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
            query += "select SEMANTIC_LINKS.SEMANTIC_LINK_ID,SEMANTIC_LINKS.LINK_ID,LINKS.LATITUDE,LINKS.LONGITUDE,LAT1,LON1,LAT2,LON2,DIRECTION.HEADING,A,B,C ";
            query += "from SEMANTIC_LINKS ";
            query += "left join LINKS on LINKS.LINK_ID = SEMANTIC_LINKS.LINK_ID ";
            query += "left join DIRECTION on LINKS.LINK_ID = DIRECTION.LINK_ID ";
            query += "order by SEMANTIC_LINK_ID ";

            return DatabaseAccesser.GetResult(query);
        }

        public static DataTable GetSemanticLinkTableWithHeadingAndLine(int semanticLinkId)
        {
            // TODO テーブル関数にすると疎結合にできるね

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
            query += "select SEMANTIC_LINKS.SEMANTIC_LINK_ID,SEMANTIC_LINKS.LINK_ID,LINKS.LATITUDE,LINKS.LONGITUDE,LAT1,LON1,LAT2,LON2,DIRECTION.HEADING,A,B,C ";
            query += "from SEMANTIC_LINKS ";
            query += "left join LINKS on LINKS.LINK_ID = SEMANTIC_LINKS.LINK_ID ";
            query += "left join DIRECTION on LINKS.LINK_ID = DIRECTION.LINK_ID ";
            query += "where SEMANTIC_LINK_ID = " + semanticLinkId + " ";
            query += "order by SEMANTIC_LINK_ID ";

            return DatabaseAccesser.GetResult(query);
        }
    }
}
