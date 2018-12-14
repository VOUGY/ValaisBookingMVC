using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ValaisBookink.ViewModels
{
    public class ReservationIndexVM
    {
        [Required(ErrorMessage = "Vous devez saisir votre prénom")]
        public string Firstname { get; set; }
        [Required(ErrorMessage = "Vous devez saisir votre nom")]
        public string Lastname { get; set; }
        [Required(ErrorMessage = "Vous devez saisir votre numéro de réservation")]
        public int ReservationId { get; set; }
    }
}