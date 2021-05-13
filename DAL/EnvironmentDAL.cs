using Microsoft.Extensions.Configuration;
using PlantWellBgClient.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace PlantWellBgClient.DAL
{
    public sealed class EnvironmentDAL
    {
        private static IConfiguration _iconfiguration;
        private string _connectionString;
        public EnvironmentDAL()
        {
      //      _connectionString = iconfiguration.GetConnectionString("Default");
        }
        /**
        public List<EnvironmentData> GetList(string queryString)
        {
            var listEnvironmentModel = new List<EnvironmentData>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand(queryString, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                    
                         listEnvironmentModel.Add(new EnvironmentData
                            {
                                Id = Convert.ToInt32(rdr[0]),
                                Country = rdr[1].ToString(),
                                Version = Convert.ToBoolean(rdr[2])
                            });
                         
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listEnvironmentModel;
        }
        **/
    }
}  

