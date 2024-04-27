using PagedList;

namespace FlightManagerApp.Views.ViewModel
{
    public class ReservationViewModel
    {
        public IEnumerable<FlightManagerApp.Models.Reservation> Reservations { get; set; }
        public IPagedList<FlightManagerApp.Models.Reservation> PagedReservations { get; set; }
    }

}
