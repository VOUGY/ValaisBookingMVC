using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using DTO;
using DAL;
using System.Net.Http;
using Newtonsoft.Json;

namespace BLL
{
    public class ReservationManager
    {
        static string baseUri = "http://localhost:1176/api/";

        /// <summary>
        /// Check data before inserting into database
        /// </summary>
        public static int AddReservationAsync(string firstName, string lastName, DateTime arrival, DateTime departure, int[] roomIds)
        {
            string uri;
            Boolean status;

            // Insert the reservation
            Client c = new Client();
            c.Firstname = firstName;
            c.Lastname = lastName;

            Reservation p = new Reservation();
            p.Arrival = arrival;
            p.Departure = departure;
            p.Client = c;

            uri = baseUri + "reservations";
            using (HttpClient httpClient = new HttpClient())
            {
                string pro = JsonConvert.SerializeObject(p, Formatting.Indented, new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                });
                StringContent frame = new StringContent(pro, Encoding.UTF8, "Application/json");
                Task<HttpResponseMessage> response = httpClient.PostAsync(uri, frame);

                status = response.Result.IsSuccessStatusCode;

                Task<String> resultString = response.Result.Content.ReadAsStringAsync(); 
                p = JsonConvert.DeserializeObject<Reservation>(resultString.Result);
            }

            // Insert room reservation relation
            uri = baseUri + "reservations/" + p.IdReservation + "/rooms";
            using (HttpClient httpClient = new HttpClient())
            {
                string json = JsonConvert.SerializeObject(roomIds);
                StringContent frame = new StringContent(json, Encoding.UTF8, "Application/json");
                Task<HttpResponseMessage> response = httpClient.PostAsync(uri, frame);

                status = response.Result.IsSuccessStatusCode;
            }

            return p.IdReservation;
        }
        
        /// <summary>
        /// Return a reservation, selected with id
        /// </summary>
        public static Reservation GetReservation(int idReservation)
        {
            string uri = baseUri + "reservations/" + idReservation;
            using (HttpClient httpClient = new HttpClient())
            {
                Task<String> response = httpClient.GetStringAsync(uri);
                return JsonConvert.DeserializeObject<Reservation>(response.Result);
            }
        }

        public static bool IsExistIsCorrect(string firstname, string lastname, int idReservation)
        {
            Reservation reservation;

            string uri = baseUri + "reservations/" + idReservation;
            using (HttpClient httpClient = new HttpClient())
            {
                Task<String> response = httpClient.GetStringAsync(uri);
                reservation = JsonConvert.DeserializeObject<Reservation>(response.Result);
            }

            if (!reservation.Client.Firstname.Equals(firstname))
                return false;

            if (!reservation.Client.Lastname.Equals(lastname))
                return false;

            return true;
        }

        /// <summary>
        /// Delete a reservation:
        ///     1. delete Reservation
        ///     2. delete Client
        /// </summary>
        public static void DeleteReservation(int idReservation)
        {
            Boolean status;
            Reservation reservation;

            // Get reservation
            string uri = baseUri + "reservations/" + idReservation;
            using (HttpClient httpClient = new HttpClient())
            {
                Task<String> response = httpClient.GetStringAsync(uri);
                reservation = JsonConvert.DeserializeObject<Reservation>(response.Result);
            }

            // Delete reservation
            uri = baseUri + "reservations/" + idReservation;
            using (HttpClient httpClient = new HttpClient())
            {
                Task<HttpResponseMessage> response = httpClient.DeleteAsync(uri);
                status = response.Result.IsSuccessStatusCode;
            }

            // Delete client of reservation
            uri = baseUri + "clients/" + reservation.Client.IdClient;
            using (HttpClient httpClient = new HttpClient())
            {
                Task<HttpResponseMessage> response = httpClient.DeleteAsync(uri);
                status = response.Result.IsSuccessStatusCode;
            }
        }

        /// <summary>
        /// Methode use for calculate the amount of the reservation with a list of Rooms
        /// </summary>
        public static decimal CalculatePrice(ICollection<Room> rooms, DateTime arrival, DateTime departure)
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
                Room room;
                string uri = baseUri + "rooms/" + id;
                using (HttpClient httpClient = new HttpClient())
                {
                    Task<String> response = httpClient.GetStringAsync(uri);
                    room = JsonConvert.DeserializeObject<Room>(response.Result);
                }
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
