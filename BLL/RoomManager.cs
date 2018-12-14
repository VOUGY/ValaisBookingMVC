using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BLL
{
    public class RoomManager
    {
        /// <summary>
        /// Return a list of Room who has no reservation during a defined period and in relation with a hotel
        /// </summary>
        public static List<Room> GetRoomsWithNoReservationByHotel(int idHotel, DateTime arrival, DateTime departure)
        {
            List<Room> rooms = RoomDB.GetRoomsWithNoReservationByHotel(idHotel, arrival, departure);

            return rooms;
        }

        /// <summary>
        /// Return a Room object
        /// </summary>
        public static Room GetRoom(int idRoom)
        {
            Room room = RoomDB.GetRoom(idRoom);

            return room;
        }

        /// <summary>
        /// Return a List of Picture object
        /// </summary>
        public static List<Picture> GetPicturesForRoom(int idRoom)
        {
            List<Picture> pictures = PictureDB.GetPicturesForRoom(idRoom);

            return pictures;
        }
    
    }
}
