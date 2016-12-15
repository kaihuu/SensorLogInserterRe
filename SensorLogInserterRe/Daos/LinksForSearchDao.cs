using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Daos
{
    class LinksForSearchDao
    {

            private static readonly string TableName = "links_lookup";

            public static DataTable Get()
            {
                string query = $"SELECT * FROM {TableName}";

                return DatabaseAccesser.GetResult(query);
            }
        public static DataTable GetLinkId(int Latitude, int Longitude)
        {
            int maxLatitude = Latitude + 20;
            int minLatitude = Latitude - 20;
            int maxLongitude = Longitude + 20;
            int minLongitude = Longitude - 20;
            string query = "SELECT LINKS.* ";
            query += $"FROM '{TableName}' ,LINKS";
            query += $"WHERE key_latitude >= '{minLatitude}' AND key_longitude >= '{minLongitude}' AND key_latitude <= '{maxLatitude}' AND key_longitude <= '{maxLongitude}' AND";
            query += $"'{TableName}'.NUM = LINKS.NUM AND '{TableName}'.LINK_ID = LINKS.LINK_ID";

            return DatabaseAccesser.GetResult(query);
        }


        public static DataTable Get(string linkId)
            {
                string query = "SELECT * ";
                query += $"FROM '{TableName}' ";
                query += $"WHERE link_id = '{linkId}' ";

                return DatabaseAccesser.GetResult(query);
            }

        }
    
}
