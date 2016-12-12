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

            private static readonly string TableName = "links_for_search";

            public static DataTable Get()
            {
                string query = $"SELECT * FROM {TableName}";

                return DatabaseAccesser.GetResult(query);
            }
        public static DataTable GetLinkId(int Latitude, int Longitude)
        {
            string query = "SELECT LINKS.* ";
            query += $"FROM '{TableName}' ,LINKS";
            query += $"WHERE key_latitude = '{Latitude}' AND key_longitude = '{Longitude}' AND '{TableName}'.NUM = LINKS.NUM"; 
            query += $"AND '{TableName}'.LINK_ID = LINKS.LINK_ID";

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
