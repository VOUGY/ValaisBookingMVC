using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ValaisBookink.ViewModels;
using BLL;
using DTO;

namespace ValaisBookink.Controllers
{
    public class ReservationController : Controller
    {
        /// <summary>
        /// Display a form for search a reservation 
        /// </summary>
        public ActionResult Index()
        {
            //Retrieve Modelstate for display error
            if (TempData["ViewData"] != null)
            {
                ViewData = (ViewDataDictionary)TempData["ViewData"];
            }

            if (TempData["IsDelete"] != null)
            {
                ViewBag.IsDelete = Convert.ToBoolean(TempData["IsDelete"]);
            }
            else
            {
                ViewBag.IsDelete = false;
            }


            return View();
        }
        /// <summary>
        /// Catch post request from Index
        /// </summary>
        [HttpPost]
        public ActionResult Index(ReservationIndexVM getReservation)
        {
            //check if data is valid
            if (ModelState.IsValid)
            {
                //check if reservation exist and is correct then create a session and redirect to Details Reservation
                if(ReservationManager.IsExistIsCorrect(getReservation.Firstname, getReservation.Lastname, getReservation.ReservationId))
                {
                    Session["IsAuthorized"] = true;

                    return RedirectToAction("Details", new { id = getReservation.ReservationId });
                }
                else
                {
                    ModelState.AddModelError("error", "Information non valide");
                }
            }

            TempData["ViewData"] = ViewData;

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Display reservation details (Hotle, room, reservation info, ...) when the user is authorized
        /// </summary>
        public ActionResult Details(int id)
        {
            //Check if session exist
            if (Session["IsAuthorized"] != null)
            {
                bool isAuthorized = Convert.ToBoolean(Session["IsAuthorized"]);

                //check session status, if true get data and display it
                if (isAuthorized)
                {
                    Reservation reservation = ReservationManager.GetReservation(id);

                    ReservationValidateVM reservationValidate = new ReservationValidateVM();
                    reservationValidate.Arrival = reservation.Arrival;
                    reservationValidate.Departure = reservation.Departure;
                    reservationValidate.TotalNight = ReservationManager.GetNumberOfNight(reservation.Arrival, reservation.Departure);
                    reservationValidate.FirstName = reservation.Client.Firstname;
                    reservationValidate.LastName = reservation.Client.Lastname;
                    reservationValidate.Rooms = reservation.Rooms;
                    reservationValidate.TotalPrice = ReservationManager.CalculatePrice(reservation.Rooms, reservation.Arrival, reservation.Departure);
                    reservationValidate.RoomNumber = reservation.Rooms.Count();

                    ViewBag.ReservationValidate = reservationValidate;

                    return View();
                }
            }

            return RedirectToAction("Index");
        }
        
        /// <summary>
        /// Catch data from Hotel form, when data is valid display the validation page
        /// </summary>
        [HttpPost]
        public ActionResult Hotel(int id, ReservationVM reservation)
        {
            if (ModelState.IsValid)
            {
                TempData["Reservation"] = reservation;
                return RedirectToAction("Validate");
            }

            TempData["ViewData"] = ViewData;
            
            return RedirectToAction("Hotel");
        }

        /// <summary>
        /// Display the form for reservation and display in form the available rooms
        /// </summary>
        [HttpGet]
        public ActionResult Hotel(int id, string arrival, string departure, string firstname, string lastname)
        {
            //Retrieve Modelstate for display error
            if (TempData["ViewData"] != null)
            {
                ViewData = (ViewDataDictionary)TempData["ViewData"];
            }
            

            List<ReservationHotelVM> roomsPictures = new List<ReservationHotelVM>();

            DateTime arrivalDate;
            if (!String.IsNullOrEmpty(arrival))
            {
                arrivalDate = Convert.ToDateTime(arrival);
            }
            else
            {
                arrivalDate = DateTime.Today;
            }

            DateTime departureDate;
            if (!String.IsNullOrEmpty(departure))
            {
                departureDate = Convert.ToDateTime(departure);
            }
            else
            {
                departureDate = DateTime.Today;
            }

            List<Room> rooms = RoomManager.GetRoomsWithNoReservationByHotel(id, arrivalDate, departureDate);

            foreach (Room room in rooms)
            {
                ReservationHotelVM roomPictures = new ReservationHotelVM();
                roomPictures.Room = room;
                roomPictures.Pictures = RoomManager.GetPicturesForRoom(room.IdRoom);
                roomsPictures.Add(roomPictures);
            }

            ViewBag.RoomsPictures = roomsPictures;
            
            return View();
        }

        /// <summary>
        /// User can control is reservation and accept when all is ok
        /// </summary>
        [HttpGet]
        public ActionResult Validate()
        {
            ReservationVM reservation = (ReservationVM)TempData["Reservation"];
            TempData["Reservation"] = reservation;

            List<Room> rooms = new List<Room>();

            foreach (int id in reservation.RoomIds)
            {
                Room room = RoomManager.GetRoom(id);
                rooms.Add(room);
            }

            ReservationValidateVM reservationValidate = new ReservationValidateVM();
            reservationValidate.Arrival = reservation.Arrival;
            reservationValidate.Departure = reservation.Departure;
            reservationValidate.TotalNight = ReservationManager.GetNumberOfNight(reservation.Arrival, reservation.Departure);
            reservationValidate.FirstName = reservation.Firstname;
            reservationValidate.LastName = reservation.Lastname;
            reservationValidate.Rooms = rooms;
            reservationValidate.TotalPrice = ReservationManager.CalculatePrice(reservation.RoomIds, reservation.Arrival, reservation.Departure);
            reservationValidate.RoomNumber = rooms.Count();

            ViewBag.ReservationValidate = reservationValidate;

            return View(reservation);
        }

        /// <summary>
        /// Catch data from hiden form in validate page and save this in database
        /// </summary>
        [HttpPost]
        public ActionResult Create(ReservationVM reservation)
        {
            if (ModelState.IsValid)
            {
                TempData["ReservationId"] = ReservationManager.AddReservationAsync(
                    reservation.Firstname,
                    reservation.Lastname,
                    reservation.Arrival,
                    reservation.Departure,
                    reservation.RoomIds
                );

                TempData["Firstname"] = reservation.Firstname;
                TempData["Lastname"] = reservation.Lastname;

                return RedirectToAction("Index", "Hotel");
            }

            TempData["ViewData"] = ViewData;

            return RedirectToAction("Hotel");
        }

        /// <summary>
        /// Delete a reservation when the user is authorized
        /// </summary>
        public ActionResult Delete(int id)
        {
            if (Session["IsAuthorized"] != null)
            {
                bool isAuthorized = Convert.ToBoolean(Session["IsAuthorized"]);

                //check session status, if true get data and display it
                if (isAuthorized)
                {
                    ReservationManager.DeleteReservation(id);

                    TempData["IsDelete"] = true;
                }
            }

            return RedirectToAction("Index");
        }
    }
}
