using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using System.Configuration;
using System.Data.SqlClient;

namespace DAL
{
    public class ClientDB
    {
        public static int AddClient(string firstname, string lastname)
        {
            int result = 0;

            string connectionString = ConfigurationManager.ConnectionStrings["DatabaseDataAccess"].ConnectionString;

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Client(Firstname, Lastname) VALUES(@firstname, @lastname); SELECT scope_identity();";
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@firstname", firstname);
                    cmd.Parameters.AddWithValue("@lastname", lastname);

                    cn.Open();

                    result = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }

        public static Client GetClient(int idClient)
        {
            Client result = new Client();

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DatabaseDataAccess"].ConnectionString;

                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM Client WHERE IdClient = @idClient";

                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@idClient", idClient);
                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            result.IdClient = idClient;

                            result.Firstname = (string)dr["Firstname"];

                            result.Lastname = (string)dr["Lastname"];
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }

        public static int DeleteClient(int idClient)
        {
            int result = 0;

            string connectionString = ConfigurationManager.ConnectionStrings["DatabaseDataAccess"].ConnectionString;

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Client WHERE IdClient = @idClient";
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@idClient", idClient);

                    cn.Open();

                    result = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }
    }
}
