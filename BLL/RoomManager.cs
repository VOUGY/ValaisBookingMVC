using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using System.Net.Http;
using Newtonsoft.Json;

namespace BLL
{
    public class RoomManager
    {
        static string baseUri = "http://localhost:1176/api/";

        /// <summary>
        /// Return a list of Room who has no reservation during a defined period and in relation with a hotel
        /// </summary>
        public static List<Room> GetRoomsWithNoReservationByHotel(int idHotel, DateTime arrival, DateTime departure)
        {
            string uri = baseUri + "hotels/" + idHotel + "/rooms?withReservation=0&arrival="+arrival.ToString()+"&departure="+departure.ToString();
            using (HttpClient httpClient = new HttpClient())
            {
                Task<String> response = httpClient.GetStringAsync(uri);
                return JsonConvert.DeserializeObject<List<Room>>(response.Result);
            }
        }

        /// <summary>
        /// Return a Room object
        /// </summary>
        public static Room GetRoom(int idRoom)
        {
            string uri = baseUri + "rooms/" + idRoom;
            using (HttpClient httpClient = new HttpClient())
            {
                Task<String> response = httpClient.GetStringAsync(uri);
                return JsonConvert.DeserializeObject<Room>(response.Result);
            }
        }

        /// <summary>
        /// Return a List of Picture object
        /// </summary>
        public static List<Picture> GetPicturesForRoom(int idRoom)
        {
            string uri = baseUri + "rooms/" + idRoom + "/pictures";
            using (HttpClient httpClient = new HttpClient())
            {
                Task<String> response = httpClient.GetStringAsync(uri);
                return JsonConvert.DeserializeObject<List<Picture>>(response.Result);
            }
        }
    
    }
}
