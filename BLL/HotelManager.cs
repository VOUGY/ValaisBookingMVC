using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BLL
{
    public class HotelManager
    {
        /// <summary>
        /// Return a List of Hotel object
        /// </summary>
        public static List<Hotel> GetHotels()
        {
            List<Hotel> hotels = HotelDB.GetHotels();

            return hotels;
        }

        /// <summary>
        /// Return a Hotel object
        /// </summary>
        public static Hotel GetHotel(int idHotel)
        {
            Hotel hotel = HotelDB.GetHotel(idHotel);

            return hotel;
        }

        /// <summary>
        /// Look for all the free rooms during a period of a hotel to know the number of free room, free space and the occupation rate
        /// </summary>
        public static HotelAvailability GetAvailability(int idHotel, DateTime arrival, DateTime departure)
        {
            HotelAvailability hotelAvailability = new HotelAvailability();
            int availablePlace = 0;
            decimal minPrice = decimal.MaxValue;

            List<Room> rooms = RoomDB.GetRoomsWithNoReservationByHotel(idHotel, arrival, departure);

            foreach (Room room in rooms)
            {
                availablePlace += room.Type;

                if (room.Price < minPrice)
                {
                    minPrice = room.Price;
                }
            }

            double totalRoom = GetCapacity(idHotel).TotalNumberRoom;

            double occupancyRate = 1.0 - (rooms.Count() / totalRoom);

            hotelAvailability.NumberOfAvailableRoom = rooms.Count();
            hotelAvailability.NumberOfAvailablePerson = availablePlace;
            hotelAvailability.OcucupancyRate = occupancyRate;
            hotelAvailability.HasAvailableRoomWithTv = rooms.Any(room => room.HasTV == true);
            hotelAvailability.HasAvailableRoomWithHairDryer = rooms.Any(room => room.HasHairDryer == true);
            hotelAvailability.MinPrice = minPrice;

            return hotelAvailability;
        }

        /// <summary>
        /// Return a random picture in relation with the hotel
        /// </summary>
        public static string GetUrlPicture(int idHotel)
        {
            List<Picture> pictures = PictureDB.GetPicturesForHotel(idHotel);

            Random rnd = new Random();
            int r = rnd.Next(pictures.Count);
            return pictures[r].Url;
        }

        /// <summary>
        /// Return a list of pictures in relation with the hotel
        /// </summary>
        public static List<Picture> GetPictures(int idHotel)
        {
            List<Picture> pictures = PictureDB.GetPicturesForHotel(idHotel);

            return pictures;
        }

        /// <summary>
        /// Return all hotel info as we need in a list
        /// Is use in Index Hotel and Search Hotel action
        /// </summary>
        public static List<HotelWithInfo> GetHotelsWithInfo(DateTime arrival, DateTime departure)
        {
            List<HotelWithInfo> hotelsWithInfo = new List<HotelWithInfo>();
            List<Hotel> hotels = HotelDB.GetHotels();

            foreach (Hotel hotel in hotels)
            {
                HotelWithInfo hotelWithInfo = new HotelWithInfo();

                hotelWithInfo.Hotel = hotel;
                hotelWithInfo.HotelAvailability = GetAvailability(hotel.IdHotel, arrival, departure);
                hotelWithInfo.HotelCapacity = GetCapacity(hotel.IdHotel);
                hotelWithInfo.UrlPicture = GetUrlPicture(hotel.IdHotel);
                hotelsWithInfo.Add(hotelWithInfo);
            }

            return hotelsWithInfo;
        }

        /// <summary>
        /// Return a object with the total of room and total of person of a hotel
        /// </summary>
        public static HotelCapacity GetCapacity(int idHotel)
        {
            HotelCapacity hotelCapacity = new HotelCapacity();
            int place = 0;

            List<Room> rooms = RoomDB.GetRoomsByHotel(idHotel);

            foreach (Room room in rooms)
            {
                place += room.Type;
            }

            hotelCapacity.TotalNumberRoom = rooms.Count();
            hotelCapacity.TotalNumberPerson = place;


            return hotelCapacity;
        }
    }
}
