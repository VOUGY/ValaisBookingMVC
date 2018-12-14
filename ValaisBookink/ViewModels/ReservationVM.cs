using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ValaisBookink.ViewModels
{
    public class ReservationVM : IValidatableObject
    {
        [Required(ErrorMessage = "Vous devez saisir votre prénom")]
        public string Firstname { get; set; }
        [Required(ErrorMessage = "Vous devez saisir votre nom")]
        public string Lastname { get; set; }
        public DateTime Arrival { get; set; }
        public DateTime Departure { get; set; }
        [Required(ErrorMessage = "Vous devez sélectionner une chambre")]
        public int[] RoomIds { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
        
            if (Arrival <= DateTime.Today)
                yield return new ValidationResult("Vous devez saisir une date supérieur ou égal à celle d'aujourd'hui", new[] { "Arrival" });

            //Check if departure date is older than tomorrow
            if (Departure < DateTime.Today.AddDays(1))
                yield return new ValidationResult("Vous devez saisir une date supérieur ou égal à celle de demain", new[] { "Departure" });

            //Check if departure date is older than arrival date
            if (Arrival > Departure)
                yield return new ValidationResult("Vous devez saisir une date de départ supérieur à celle de l'arrivé", new[] { "Arrival", "Departure" });
        }
    }
}