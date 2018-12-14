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
    public class HotelDB
    {
        public static List<Hotel> GetHotels()
        {
            List<Hotel> results = null;

            string connectionString = ConfigurationManager.ConnectionStrings["DatabaseDataAccess"].ConnectionString;

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "Select * from Hotel";
                    SqlCommand cmd = new SqlCommand(query, cn);

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (results == null)
                                results = new List<Hotel>();

                            Hotel hotel = new Hotel();

                            hotel.IdHotel = (int)dr["IdHotel"];

                            hotel.Name = (string)dr["Name"];

                            hotel.Description = (string)dr["Description"];

                            hotel.Location = (string)dr["Location"];

                            hotel.Category = (int)dr["Category"];

                            hotel.HasWifi = (bool)dr["HasWifi"];

                            hotel.HasParking = (bool)dr["HasParking"];

                            if (dr["Phone"] != DBNull.Value)
                                hotel.Phone = (string)dr["Phone"];

                            if (dr["Email"] != DBNull.Value)
                                hotel.Email = (string)dr["Email"];

                            if (dr["Website"] != DBNull.Value)
                                hotel.Website = (string)dr["Website"];

                            results.Add(hotel);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return results;
        }

        public static Hotel GetHotel(int idHotel)
        {
            Hotel result = new Hotel();

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DatabaseDataAccess"].ConnectionString;

                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM Hotel WHERE IdHotel = @idHotel";

                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@idHotel", idHotel);
                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            result.IdHotel = idHotel;

                            result.Name = (string)dr["Name"];

                            result.Description = (string)dr["Description"];

                            result.Location = (string)dr["Location"];

                            result.Category = (int)dr["Category"];

                            result.HasWifi = (bool)dr["HasWifi"];

                            result.HasParking = (bool)dr["HasParking"];

                            if (dr["Phone"] != DBNull.Value)
                                result.Phone = (string)dr["Phone"];

                            if (dr["Email"] != DBNull.Value)
                                result.Email = (string)dr["Email"];

                            if (dr["Website"] != DBNull.Value)
                                result.Website = (string)dr["Website"];
                        }
                    }
                }
                System.Console.WriteLine("no error");
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }
    }
}
