using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using AutoMapper;
using FlightManagerApp.Areas.Identity.Data;
using FlightManagerApp.Data.Mapping;

namespace FlightManagerApp.Models
{
    public class AdminModel : ICustomMapping
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EGN { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public void ConfigureMapping(IMapperConfigurationExpression mapper)
        {
            mapper.CreateMap<ApplicationUser, AdminModel>().ForMember(a => a.Id, b => b.MapFrom(c => c.Id));
            mapper.CreateMap<ApplicationUser, AdminModel>().ForMember(a => a.Email, b => b.MapFrom(c => c.Email));
            mapper.CreateMap<ApplicationUser, AdminModel>().ForMember(a => a.UserName, b => b.MapFrom(c => c.UserName));
        }
    }
}
