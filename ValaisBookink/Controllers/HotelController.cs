using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ValaisBookink.ViewModels;
using DTO;
using BLL;

namespace ValaisBookink.Controllers
{
    public class HotelController : Controller
    {
        /// <summary>
        /// Page who start to search a hotel
        /// </summary>
        public ActionResult Index()
        {
            //if TempData["ReservationId"] is not null, is not a end of reservation so we don't need to display reservation info
            if (TempData["ReservationId"] != null)
            {
                ViewBag.ReservationId = TempData["ReservationId"];
                ViewBag.Firstname = TempData["Firstname"];
                ViewBag.Lastname = TempData["Lastname"];
            }

            return View();
        }

        /// <summary>
        /// This action display hotel, 
        /// Filter the hotels list with get parameter (location, arrival, departure, person, category, hasParking, hasWifi, hasTv, hasHairDryer)
        /// </summary>
        [HttpGet]
        public ActionResult Search(string location,  DateTime arrival, DateTime departure, int? person, int? category, 
                                  bool hasParking = false, bool hasWifi = false, bool hasTv = false, bool hasHairDryer = false)
        {
            List<HotelWithInfo> hotels = HotelManager.GetHotelsWithInfo(arrival, departure);

            if (!String.IsNullOrEmpty(location))
                hotels = hotels.Where(m => m.Hotel.Location.IndexOf(location, StringComparison.OrdinalIgnoreCase) != -1).ToList();

            if (hasParking)
                hotels = hotels.Where(m => m.Hotel.HasParking == hasParking).ToList();

            if (hasWifi)
                hotels = hotels.Where(m => m.Hotel.HasWifi == hasWifi).ToList();

            if (category != null)
            {
                if (category <= 5 && category > 0)
                    hotels = hotels.Where(m => m.Hotel.Category == category).ToList();
            }

            if (hasTv)
                hotels = hotels.Where(m => m.HotelAvailability.HasAvailableRoomWithTv == hasTv).ToList();

            if (hasHairDryer)
                hotels = hotels.Where(m => m.HotelAvailability.HasAvailableRoomWithHairDryer == hasHairDryer).ToList();

            if (person != null)
                hotels = hotels.Where(m => m.HotelAvailability.NumberOfAvailablePerson >= person).ToList();

            return View(hotels);
        }

        /// <summary>
        /// Display details for a hotel
        /// </summary>
        public ActionResult Details(int id)
        {
            HotelDetailsVM hotelDetailsVM = new HotelDetailsVM();
            hotelDetailsVM.Hotel = HotelManager.GetHotel(id);
            hotelDetailsVM.HotelCapacity = HotelManager.GetCapacity(id);
            hotelDetailsVM.Pictures = HotelManager.GetPictures(id);

            return View(hotelDetailsVM);
        }

    }
}
