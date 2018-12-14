using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using DTO;
using DAL;

namespace BLL
{
    public class ReservationManager
    {
        /// <summary>
        /// Check data before inserting into database
        /// </summary>
        public static int AddReservation(string firstName, string lastName, DateTime arrival, DateTime departure, int[] roomIds)
        {
            
            //Insert the reservation
            int idClient = ClientDB.AddClient(firstName, lastName);

            int idReservation = ReservationDB.AddReservation(arrival, departure, idClient);

            foreach(int roomId in roomIds)
            {
                ReservationDB.AddRoomReservation(idReservation, roomId);
            }

            return idReservation;
        }
        
        /// <summary>
        /// Return a reservation, selected with id
        /// </summary>
        public static Reservation GetReservation(int idReservation)
        {
            return ReservationDB.GetReservation(idReservation);
        }

        public static bool IsExistIsCorrect(string firstname, string lastname, int idReservation)
        {
            Reservation reservation = ReservationDB.GetReservation(idReservation);

            if (!reservation.Client.Firstname.Equals(firstname))
                return false;

            if (!reservation.Client.Lastname.Equals(lastname))
                return false;

            return true;
        }

        /// <summary>
        /// Delete a reservation:
        ///     1. delete all RoomReservation
        ///     2. delete Reservation
        ///     3. delete Client
        /// </summary>
        public static void DeleteReservation(int idReservation)
        {
            Reservation reservation = ReservationDB.GetReservation(idReservation);

            ReservationDB.DeleteRoomReservation(idReservation);

            ReservationDB.DeleteReservation(idReservation);

            ClientDB.DeleteClient(reservation.Client.IdClient);
        }

        /// <summary>
        /// Methode use for calculate the amount of the reservation with a list of Rooms
        /// </summary>
        public static decimal CalculatePrice(List<Room> rooms, DateTime arrival, DateTime departure)
        {
            decimal totalPrice = 0;

            foreach (Room room in rooms)
            {
                totalPrice += room.Price;
            }

            totalPrice = totalPrice * GetNumberOfNight(arrival, departure);

            return totalPrice;
        }

        /// <summary>
        /// Methode use for calculate the amount of the reservation with a list of int who contain room id
        /// </summary>
        public static decimal CalculatePrice(int[] roomIds, DateTime arrival, DateTime departure)
        {
            decimal totalPrice = 0;

            foreach (int id in roomIds)
            {
                Room room = RoomManager.GetRoom(id);
                totalPrice += room.Price;
            }

            totalPrice = totalPrice * GetNumberOfNight(arrival, departure);

            return totalPrice;
        }

        /// <summary>
        /// Methode who returned total of night
        /// </summary>
        public static int GetNumberOfNight(DateTime arrival, DateTime departure)
        {
            return (int)(departure - arrival).TotalDays;
        }
    }
}
