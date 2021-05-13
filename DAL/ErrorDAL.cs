using Microsoft.Extensions.Configuration;
using PlantWell.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace PlantWell.DAL
{
    public class ErrorDAL
    {
        private string _connectionString;
        public ErrorDAL(IConfiguration iconfiguration)
        {
            _connectionString = iconfiguration.GetConnectionString("Default");
        }
        public List<ErrorData> GetList(string queryString)
        {
            var listErrorModel = new List<ErrorData>();
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

                        /**
                         * listErrorModel.Add(new EnvironmentData
                            {
                                Id = Convert.ToInt32(rdr[0]),
                                Country = rdr[1].ToString(),
                                Version = Convert.ToBoolean(rdr[2])
                            });
                         *
                         **/
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listErrorModel;
        }


        public void writeToDatabase()
        {

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"INSERT INTO ErrorLog(klant_id,naam,voornaam) 
                            VALUES(@param1,@param2,@param3)";

                    //cmd.Parameters.AddWithValue("@param1", klantId);
                    //cmd.Parameters.AddWithValue("@param2", klantNaam);
                    //cmd.Parameters.AddWithValue("@param3", klantVoornaam);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                       //MessgeBox.Show(e.Message.ToString(), "Error Message");
                    }

                }
            }



        }
    }

}