using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightManagerApp.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    [PersonalData]
    [DisplayName("First Name")]
    [Required(ErrorMessage = "Please enter your first name.")]
    public string FirstName { get; set; }

    [PersonalData]
    [DisplayName("Last Name")]
    [Required(ErrorMessage = "Please enter your last name.")]
    public string LastName { get; set; }

    [PersonalData]
    [Required(ErrorMessage = "Please enter your EGN."),
        RegularExpression(@"^([0-9]{10})", ErrorMessage = "EGN must be a 10-digit number.")]
    public string EGN { get; set; }

    [PersonalData]
    [Required(ErrorMessage = "Please enter your home address.")]
    public string Address { get; set; }

    [DisplayName("Phone Number")]
    [Required(ErrorMessage = "Please enter your phone number."),
        RegularExpression(@"^([0-9]{10})", ErrorMessage = "Phone number must be a 10-digit number.")]
    public string PhoneNumber { get; set; }
}

