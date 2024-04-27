using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightManagerApp.Models
{
    public class Flight
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FlightID { get; set; }

        [Display(Name = "Flight From")]
        [Required(ErrorMessage = "Please enter takeoff destination.")]
        public string FlightFrom { get; set; }

        [Display(Name = "Flight To")]
        [Required(ErrorMessage = "Please enter flight destination.")]
        public string FlightTo { get; set; }

        [Display(Name = "TakeOff Time")]
        [Required(ErrorMessage = "Please enter takeoff time.")]
        public DateTime TakeOffTime { get; set; }

        [Display(Name = "Landing Time")]
        [Required(ErrorMessage = "Please enter landing time.")]
        public DateTime LandingTime { get; set; }

        [Display(Name = "Plane Type")]
        [Required(ErrorMessage = "Please enter plane type.")]
        public string PlaneType { get; set; }

        [Display(Name = "Plane Number")]
        [Required(ErrorMessage = "Please enter plane number.")]
        [StringLength(6, ErrorMessage = "{0} must be {1} letters long.", MinimumLength = 6)]
        public string PlaneNumber { get; set; }

        [Display(Name = "Pilot Name")]
        [Required(ErrorMessage = "Please enter pilot name.")]
        public string PilotName { get; set; }

        [Display(Name = "Plane Capacity")]
        [Required(ErrorMessage = "Please enter plane capacity.")]
        public int PlaneCapacity { get; set; }

        [Display(Name = "Plane Business Class Capacity")]
        [Required(ErrorMessage = "Please enter plane business class capacity.")]
        public int PlaneBusinessClassCapacity { get; set; }

        public ICollection<Reservation> Reservations { get; } = new List<Reservation>();

    }
}
