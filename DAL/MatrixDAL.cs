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
    class MatrixDAL
    {
  
        public static MatrixData LoadList(SqlConnection con)
        {
            var listOfPerson = new List<MatrixData>();

           {
                string sql = "SELECT * FROM [dbo].[matrix] ORDER BY [id] ASC; ";
                using (var command = new SqlCommand(sql, con))
                {
                    try
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MatrixData matrixD = new MatrixData()
                                {

                                    Lighting = Convert.ToBoolean(reader["Lighting"]),
                                    IntakeEntFan = Convert.ToBoolean(reader["IntakeEntFan"]),
                                    IntakeExitTopFan = Convert.ToBoolean(reader["IntakeExitTopFan"]),
                                    IntakeExitBottomFan = Convert.ToBoolean(reader["IntakeExitBottomFan"]),
                                    OuttakeRecircFan = Convert.ToBoolean(reader["OuttakeRecircFan"]),
                                    OuttakeExitFan = Convert.ToBoolean(reader["OuttakeExitFan"]),
                                    OuttakeEntFan = Convert.ToBoolean(reader["OuttakeEntFan"]),
                                    LightCoolingFan = Convert.ToBoolean(reader["LightCoolingFan"]),
                                };
                                return matrixD;
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

        public static MatrixData[] LoadCollection(SqlConnection con)
        {


            string sql = "SELECT * FROM [dbo].[matrix] WHERE [Lighting] = 1 ORDER BY [id] ASC; ";

            List<MatrixData> tempList = new List<MatrixData>();

          
                SqlCommand command = new SqlCommand(sql, con);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                    MatrixData matrixD = new MatrixData()
                    {
                        Id = Convert.ToInt32(reader["id"]),

                        Lighting = Convert.ToBoolean(reader["Lighting"]),
                        IntakeEntFan = Convert.ToBoolean(reader["IntakeEntFan"]),
                        IntakeExitTopFan = Convert.ToBoolean(reader["IntakeExitTopFan"]),
                        IntakeExitBottomFan = Convert.ToBoolean(reader["IntakeExitBottomFan"]),
                        OuttakeRecircFan = Convert.ToBoolean(reader["OuttakeRecircFan"]),
                        OuttakeExitFan = Convert.ToBoolean(reader["OuttakeExitFan"]),
                        OuttakeEntFan = Convert.ToBoolean(reader["OuttakeEntFan"]),
                        LightCoolingFan = Convert.ToBoolean(reader["LightCoolingFan"]),
                    };
                    tempList.Add(matrixD);
                    }
                }
                reader.Close();
            

            MatrixData[] res = tempList.ToArray();

            return res;
        }
    }
}
