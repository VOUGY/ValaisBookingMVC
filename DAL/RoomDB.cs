using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;

namespace DAL
{
    public static class RoomDB
    {
        public static List<Room> GetRoomsWithNoReservationByHotel(int idHotel, DateTime arrival, DateTime departure)
        {
            List<Room> results = null;

            string connectionString = ConfigurationManager.ConnectionStrings["DatabaseDataAccess"].ConnectionString;

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = @"SELECT ro.*, re.* FROM Room ro
                                    FULL JOIN RoomReservation rr ON ro.IdRoom = rr.IdRoom
                                    LEFT JOIN Reservation re ON re.IdReservation = rr.IdReservation 
                                    WHERE 
                                        ro.IdHotel = @idHotel
                                        AND (
                                            NOT(
                                                re.Arrival BETWEEN @arrival AND @departure
                                                OR re.Departure BETWEEN @arrival AND @departure
                                                OR(re.Arrival <= @arrival AND re.Departure >= @departure)
                                            )
                                            OR re.Arrival IS NULL
                                        )"
                                    ;

                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@idHotel", idHotel);
                    cmd.Parameters.AddWithValue("@arrival", arrival.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    cmd.Parameters.AddWithValue("@departure", departure.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (results == null)
                                results = new List<Room>();

                            Room room = new Room();

                            room.IdRoom = (int)dr["IdRoom"];

                            room.Number = (int)dr["Number"];

                            room.Description = (string)dr["Description"];

                            room.Type = (int)dr["Type"];

                            room.Price = (decimal)dr["Price"];

                            room.HasTV = (bool)dr["HasTV"];

                            room.HasHairDryer = (bool)dr["HasHairDryer"];

                            room.Hotel = HotelDB.GetHotel((int)dr["IdHotel"]);

                            results.Add(room);
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

        public static List<Room> GetRoomsByHotel(int idHotel)
        {
            List<Room> results = null;

            string connectionString = ConfigurationManager.ConnectionStrings["DatabaseDataAccess"].ConnectionString;

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM Room WHERE IdHotel = @idHotel";

                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@idHotel", idHotel);
                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (results == null)
                                results = new List<Room>();

                            Room room = new Room();

                            room.IdRoom = (int)dr["IdRoom"];

                            room.Number = (int)dr["Number"];

                            room.Description = (string)dr["Description"];

                            room.Type = (int)dr["Type"];

                            room.Price = (decimal)dr["Price"];

                            room.HasTV = (bool)dr["HasTV"];

                            room.HasHairDryer = (bool)dr["HasHairDryer"];

                            room.Hotel = HotelDB.GetHotel(idHotel);

                            results.Add(room);
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

        public static Room GetRoom(int idRoom)
        {
            Room result = new Room();

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DatabaseDataAccess"].ConnectionString;

                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM Room WHERE IdRoom = @idRoom";

                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@idRoom", idRoom);
                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            result.IdRoom = (int)dr["IdRoom"];

                            result.Number = (int)dr["Number"];

                            result.Description = (string)dr["Description"];

                            result.Type = (int)dr["Type"];

                            result.Price = (decimal)dr["Price"];

                            result.HasTV = (bool)dr["HasTV"];

                            result.HasHairDryer = (bool)dr["HasHairDryer"];

                            result.Hotel = HotelDB.GetHotel((int)dr["IdHotel"]);
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

        /*
        public static List<Room> GetRoomByHotel(int idHotel)
        {
            List<Room> results = null;

            string connectionString = ConfigurationManager.ConnectionStrings["DatabaseDataAccess"].ConnectionString;

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = @"SELECT * FROM Room WHERE IdHotel = @idHotel";

                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@idHotel", idHotel);
                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (results == null)
                                results = new List<Room>();

                            Room room = new Room();

                            room.IdRoom = (int)dr["IdRoom"];

                            room.Number = (int)dr["Number"];

                            room.Description = (string)dr["Description"];

                            room.Type = (int)dr["Type"];

                            room.Price = (decimal)dr["Price"];

                            room.HasTV = (bool)dr["HasTV"];

                            room.HasHairDryer = (bool)dr["HasHairDryer"];

                            room.Hotel = null;

                            results.Add(room);
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
        */

        public static List<Room> GetRoomsOfReservation(int reservationId)
        {
            List<Room> results = null;

            string connectionString = ConfigurationManager.ConnectionStrings["DatabaseDataAccess"].ConnectionString;

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = @"SELECT ro.* FROM Room ro
                                    LEFT JOIN RoomReservation rr ON rr.IdRoom = ro.IdRoom
                                    WHERE rr.IdReservation = @reservationId";

                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@reservationId", reservationId);
                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (results == null)
                                results = new List<Room>();

                            Room room = new Room();

                            room.IdRoom = (int)dr["IdRoom"];

                            room.Number = (int)dr["Number"];

                            room.Description = (string)dr["Description"];

                            room.Type = (int)dr["Type"];

                            room.Price = (decimal)dr["Price"];

                            room.HasTV = (bool)dr["HasTV"];

                            room.HasHairDryer = (bool)dr["HasHairDryer"];

                            room.Hotel = HotelDB.GetHotel((int)dr["IdHotel"]);

                            results.Add(room);
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
    }
}
