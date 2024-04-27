using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightManagerApp.Models
{
    public class Reservation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReservationID { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please enter your first name.")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        [Required(ErrorMessage = "Please enter your middle name.")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please enter your last name.")]
        public string LastName { get; set; }

        [Display(Name = "EGN")]
        [Required(ErrorMessage = "Please enter your EGN."),
            RegularExpression(@"^([0-9]{10})", ErrorMessage = "EGN is not valid!")]
        [StringLength(10, ErrorMessage = "The {0} must be {1} letters long.", MinimumLength = 10)]
        public string EGN { get; set; }

        [Display(Name = "Nationality")]
        [Required(ErrorMessage = "Please enter your nationality.")]
        public string Nationality { get; set; }

        [Display(Name = "Phone number")]
        [Required(ErrorMessage = "Please enter your phone number."),
            RegularExpression(@"^([0-9]{10})", ErrorMessage = "EGN is not valid!")]
        [StringLength(10, ErrorMessage = "The {0} must be {1} letters long.", MinimumLength = 10)]
        [Phone(ErrorMessage = "Phone number is not correct")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Please enter your email address.")]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$",
            ErrorMessage = "Type valid e-mail address (for example: name@domain.com)!")]
        [EmailAddress(ErrorMessage = "Email is not correct")]
        public string Email { get; set; }

        [Display(Name = "Flight number")]
        [Required(ErrorMessage = "Flight number required")]
        public int FlightID { get; set; }

        public Flight Flight { get; set; } = null!;

        [Display(Name = "Tiket type")]
        [Required(ErrorMessage = "Please enter your ticket type.")]
        public string TicketType { get; set; }
    }
}