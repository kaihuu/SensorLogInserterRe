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
            var query = new StringBuilder();
            query.AppendLine("WITH direction AS ");
            query.AppendLine("( ");
            query.AppendLine("SELECT link_id, lat1, lon1, lat2, lon2, ");
            query.AppendLine("	CASE ");
            query.AppendLine("	WHEN ATN2(y, x) * 180 / PI() >= 0 THEN ATN2(y, x) * 180 / PI() ");
            query.AppendLine("	WHEN ATN2(y, x) * 180 / PI() < 0 THEN ATN2(y, x) * 180 / PI() + 180 ");
            query.AppendLine("	END AS heading,");
            query.AppendLine("  a,");
            query.AppendLine("  b,");
            query.AppendLine("  c ");
            query.AppendLine("FROM ( ");
            query.AppendLine("	SELECT *,");
            query.AppendLine("    ROUND((COS(rad_lat2) * SIN(rad_lon2 - rad_lon1)* 1000000),2) AS y,");
            query.AppendLine("    ROUND((COS(rad_lat1) * SIN(rad_lat2) - SIN(rad_lat1) * COS(rad_lat2) * COS(rad_lon2 - rad_lon1))*1000000,2) AS x");
            query.AppendLine("    lat2 - lat1 AS a, ");
            query.AppendLine("    lon1 - lon2 AS b, ");
            query.AppendLine("    lon2 + lat1 - lon1 * lat2 as C  "); // TODO 外積の間違い？
            query.AppendLine("  FROM(");
            query.AppendLine("    SELECT node1.link_id,");
            query.AppendLine("    node1.latitude AS lat1,");
            query.AppendLine("    node1.longitude AS lon1,");
            query.AppendLine("    node2.latitude AS lat2");
            query.AppendLine("    node2.longitude AS lon2");
            query.AppendLine("    node1.latitude * PI() / 180 AS rad_lat1,");
            query.AppendLine("    node1.longitude * PI() / 180 AS rad_lon1,");
            query.AppendLine("    node2.latitude * PI() / 180 AS rad_lat2,");
            query.AppendLine("    node2.longitude * PI() / 180 AS rad_lon2");
            query.AppendLine("    FROM (");
            query.AppendLine("      SELECT * ");
            query.AppendLine("      FROM links ");
            query.AppendLine("      WHERE node_id IS NOT NULL ");
            query.AppendLine("        AND direction = 1 ");
            query.AppendLine("    ) AS node 1, ");
            query.AppendLine("    (");
            query.AppendLine("    SELECT * ");
            query.AppendLine("    FROM links ");
            query.AppendLine("    WHERE node_id IS NOT NULL ");
            query.AppendLine("      AND direction = 2");
            query.AppendLine("    ) AS node2");
            query.AppendLine("    WHERE node1.link_id = node2.link_id");
            query.AppendLine("  ) AS rad ");
            query.AppendLine(") AS xy");
            query.AppendLine("WHERE (x != 0");
            query.AppendLine("  OR y != 0)");
            query.AppendLine(")");
            query.AppendLine("SELECT semantic_links.semantic_link_id,");
            query.AppendLine("  semantic_links.link_id,");
            query.AppendLine("  links.latitude,");
            query.AppendLine("  links.longitude,");
            query.AppendLine("  lat1,");
            query.AppendLine("  lon1,");
            query.AppendLine("  lat2,");
            query.AppendLine("  lon2,");
            query.AppendLine("  ");

            
            query.AppendLine("SELECT semantic_links.link_id,");
            query.AppendLine("  semantic_links.link_id,");
            query.AppendLine("  links.latitude,");
            query.AppendLine("  links.longitude,");
            query.AppendLine("  lat1,");
            query.AppendLine("  lon1,");
            query.AppendLine("  lat2,");
            query.AppendLine("  lon2,");
            query.AppendLine("  direction.heading,");
            query.AppendLine("  a,");
            query.AppendLine("  b,");
            query.AppendLine("  c");
            query.AppendLine("  FROM ( ");
            query.AppendLine("    SELECT * ");
            query.AppendLine("    FROM links");
            query.AppendLine("    WHERE node_ide IS NOT NULL");
            query.AppendLine("      AND direction = 1");
            query.AppendLine("  ) AS node1, ");
            query.AppendLine("  (");
            query.AppendLine("  SELECT * ");
            query.AppendLine("  FROM links ");
            query.AppendLine("  WHERE node_id I");

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
        }
    }
}
