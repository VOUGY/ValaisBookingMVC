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
    public class PictureDB
    {
        public static List<Picture> GetPicturesForHotel(int idHotel)
        {
            List<Picture> results = new List<Picture>();

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DatabaseDataAccess"].ConnectionString;

                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = @"SELECT *
                                    FROM Picture 
                                    RIGHT JOIN Room ON Room.IdRoom = Picture.IdRoom
                                    WHERE IdHotel = @idHotel";

                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@idHotel", idHotel);
                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (results == null)
                                results = new List<Picture>();

                            Picture picture = new Picture();

                            picture.IdPicture = (int)dr["IdPicture"];

                            picture.Url = (string)dr["Url"];

                            picture.Room = null;

                            results.Add(picture);
                        }
                    }
                }
                System.Console.WriteLine("no error");
            }
            catch (Exception e)
            {
                throw e;
            }

            return results;
        }

        public static List<Picture> GetPicturesForRoom(int idRoom)
        {
            List<Picture> results = new List<Picture>();

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DatabaseDataAccess"].ConnectionString;

                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = @"SELECT *
                                    FROM Picture 
                                    WHERE IdRoom = @idRoom";

                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@idRoom", idRoom);
                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (results == null)
                                results = new List<Picture>();

                            Picture picture = new Picture();

                            picture.IdPicture = (int)dr["IdPicture"];

                            picture.Url = (string)dr["Url"];

                            picture.Room = null;

                            results.Add(picture);
                        }
                    }
                }
                System.Console.WriteLine("no error");
            }
            catch (Exception e)
            {
                throw e;
            }

            return results;
        }
    }
}
