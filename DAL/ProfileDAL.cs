 using Microsoft.Extensions.Configuration;
using PlantWellBgClient.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantWellBgClient.DAL
{
    class ProfileDAL
    {

        public static ProfileData getActiveProfile(SqlConnection con)
        {
            var listOfPerson = new List<DHTSensorData>();

            {
                string sql = "SELECT * FROM [dbo].[profile] WHERE [active] = '1';";
                using (var command = new SqlCommand(sql, con))
                {
                    try
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProfileData sensorD = new ProfileData()
                                {
                                    id = (int)reader["id"],
                                    
                                };
                               
                                return sensorD;
                            }
                        }
                    }
                    catch (SqlException e)
                    {
                        Debug.WriteLine(string.Format("Failed on SensorReadingsDAL - L44 - {0}", e.Message.ToString()));


                    }

                    catch (InvalidOperationException e)
                    {
                        Debug.WriteLine(string.Format("Failed on SensorReadingsDAL - L44 - {0}", e.Message.ToString()));


                    }
                }
            }
            return null;
        }
    }
}
