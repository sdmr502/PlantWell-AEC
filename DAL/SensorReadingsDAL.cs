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
    class SensorReadingsDAL
    {
  
        public static DHTSensorData Load5MinuteAverage(string sensor, SqlConnection con)
        {
            var listOfPerson = new List<DHTSensorData>();

           {
                string sql = "SELECT [sensor] AS name, AVG([temp]) AS temp, AVG([hum]) AS hum FROM[dbo].[sensorReadings] WHERE [sensor] = '"+sensor+"' GROUP BY [sensor]; ";
                using (var command = new SqlCommand(sql, con))
                {
                    try
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DHTSensorData sensorD = new DHTSensorData()
                                {
                                    name = reader["name"].ToString(),
                                    temp = (float)Convert.ToDecimal(reader["temp"]),
                                    hum = (float)Convert.ToDecimal(reader["hum"])
                                };
                                Debug.WriteLine(string.Format("{0} DHT Average", sensor));
                                Debug.WriteLine(sensorD.temp);
                                Debug.WriteLine(sensorD.hum);
                                return sensorD;
                            }
                        }
                    }
                    catch (SqlException e)
                    {
                        Debug.WriteLine(string.Format("Failed on SensorReadingsDAL - L44 - {0}", e.Message.ToString()));


                    }

                    catch (InvalidOperationException e) {
                        Debug.WriteLine(string.Format("Failed on SensorReadingsDAL - L44 - {0}", e.Message.ToString()));


                    }
                }
            }
            return null;
        }
    }
}
