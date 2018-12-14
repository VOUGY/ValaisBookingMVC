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
    public class ReservationDB
    {
        public static int AddReservation(DateTime arrival, DateTime departure, int idClient)
        {
            int result = 0;

            string connectionString = ConfigurationManager.ConnectionStrings["DatabaseDataAccess"].ConnectionString;

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "Insert into Reservation(arrival, departure, idClient) values(@arrival, @departure, @idClient); SELECT scope_identity();";
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@arrival", arrival);
                    cmd.Parameters.AddWithValue("@departure", departure);
                    cmd.Parameters.AddWithValue("@idClient", idClient);

                    cn.Open();

                    result = Convert.ToInt32(cmd.ExecuteScalar().ToString());

                    if (cn.State == System.Data.ConnectionState.Open)
                        cn.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }

        public static int AddRoomReservation(int idReservation, int idRoom)
        {
            int result = 0;

            string connectionString = ConfigurationManager.ConnectionStrings["DatabaseDataAccess"].ConnectionString;

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "Insert into RoomReservation(idRoom, idReservation) values(@idRoom, @idReservation)";
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@idRoom", idRoom);
                    cmd.Parameters.AddWithValue("@idReservation", idReservation);

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

        public static int DeleteRoomReservation(int idReservation)
        {
            int result = 0;

            string connectionString = ConfigurationManager.ConnectionStrings["DatabaseDataAccess"].ConnectionString;

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM RoomReservation WHERE IdReservation = @idReservation";
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@idReservation", idReservation);

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

        public static Reservation GetReservation(int idReservation)
        {
            Reservation result = new Reservation();

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DatabaseDataAccess"].ConnectionString;

                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM Reservation WHERE IdReservation = @idReservation";

                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@idReservation", idReservation);
                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            result.IdReservation = idReservation;

                            result.Arrival = Convert.ToDateTime(dr["Arrival"]);

                            result.Departure = Convert.ToDateTime(dr["Departure"]);

                            result.Rooms = RoomDB.GetRoomsOfReservation(idReservation);

                            result.Client = ClientDB.GetClient((int)dr["IdClient"]);
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

        public static int DeleteReservation(int idReservation)
        {
            int result = 0;

            string connectionString = ConfigurationManager.ConnectionStrings["DatabaseDataAccess"].ConnectionString;

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Reservation WHERE IdReservation = @idReservation";
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@idReservation", idReservation);

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
