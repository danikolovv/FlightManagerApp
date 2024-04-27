using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlightManagerApp.Data;
using FlightManagerApp.Models;
using PagedList;

namespace FlightManagerApp.Controllers
{
    public class ReservationController : Controller
    {
        private readonly FlightManagerContext _context;

        public ReservationController(FlightManagerContext context)
        {
            _context = context;
        }

        // GET: Reservation
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.EmailSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            ViewBag.CurrentSort = sortOrder;

            ViewBag.CurrentFilter = searchString;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var reservations = from r in _context.Reservation
                           select r;

            if (!String.IsNullOrEmpty(searchString))
            {
                reservations = reservations.Where(s => s.Email.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    reservations = reservations.OrderByDescending(s => s.Email);
                    break;
                default:
                    reservations = reservations.OrderBy(s => s.Email);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(reservations.ToPagedList(pageNumber, pageSize));

            /*var viewModel = new ReservationViewModel
            {
                Reservations = reservations.ToList(),
                PagedReservations = pagedReservations
            };

            return View(viewModel);*/
        }



        // GET: Reservation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .Include(r => r.Flight)
                .FirstOrDefaultAsync(m => m.ReservationID == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        /// <summary>
        /// Displays form creating a new reservation
        /// </summary>
        /// <returns>The view for creating a new reservation</returns>
        /// 

        public IActionResult Create()
        {
            List<SelectListItem> ticketTypes = new List<SelectListItem>()
            {
            new SelectListItem {
            Text = "Regular", Value = "1"
            },
            new SelectListItem {
            Text = "Business", Value = "2"
            }};
            ViewData["TicketType"] = ticketTypes;
            var flightsFromTo = _context.Flight.Select(x => new
            {
                x.FlightID,
                FlightFromTo = x.FlightFrom + "-" + x.FlightTo
            });
            ViewData["FlightID"] = new SelectList(flightsFromTo, "FlightID", "FlightFromTo");
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Adds a new reservation based on the submitted form data to the database
        /// </summary>
        /// <param name="reservation">The new object of type Reservation, containing the data</param>
        /// <returns>
        /// If the reservation is successfully added, redirects to the Index to display the list of reservations.
        /// Otherwise, it shows again the Create view with validation error messages.
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("ReservationID,FirstName,MiddleName,LastName,EGN,Nationality,PhoneNumber,Email,FlightID,TicketType")] Reservation reservation)
        {
            if (!ModelState.IsValid)
            {

                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            List<SelectListItem> ticketTypes = new List<SelectListItem>()
            {
            new SelectListItem {
            Text = "Regular", Value = "1"
            },
            new SelectListItem {
            Text = "Business", Value = "2"
            }};
            ViewData["TicketType"] = ticketTypes;
            var flightsFromTo = _context.Flight.Select(x => new
            {
                x.FlightID,
                FlightFromTo = x.FlightFrom + "-" + x.FlightTo
            });
            ViewData["FlightID"] = new SelectList(flightsFromTo, "FlightID", "FlightFromTo", reservation.FlightID);
            return View(reservation);
        }


        // GET: Reservations/Edit/5
        /// <summary>
        /// Displays a form to edit the data of a specific reservation.
        /// </summary>
        /// <param name="id">The ID of the reservation which is to be edited</param>
        /// <returns>
        /// If the ID is not found, returns an error "Not Found".
        /// If the ID is null, returns an error "Not Found".
        /// Otherwise, it returns an Edit view.
        /// </returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            List<SelectListItem> ticketTypes = new List<SelectListItem>()
            {
            new SelectListItem {
            Text = "Regular", Value = "1"
            },
            new SelectListItem {
            Text = "Business", Value = "2"
            }};
            ViewData["TicketType"] = ticketTypes;
            var flightsFromTo = _context.Flight.Select(x => new
            {
                x.FlightID,
                FlightFromTo = x.FlightFrom + "-" + x.FlightTo
            });
            ViewData["FlightID"] = new SelectList(flightsFromTo, "FlightID", "FlightFromTo", reservation.FlightID);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Updates the details of a specific reservation
        /// </summary>
        /// <param name="id">The ID of the reservation to edit</param>
        /// <param name="reservation">The reservation object containing the updated data</param>
        /// <returns>
        /// If the ID in the form data does not match the ID, returns a "Not Found" error.
        /// If the form data is invalid, it redisplays Edit with validation error messages.
        /// If the form data is valid and the reservation is successfully updated, redirects to the Index
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReservationID,FirstName,MiddleName,LastName,EGN,Nationality,PhoneNumber,Email,FlightID,TicketType")] Reservation reservation)
        {
            if (id != reservation.ReservationID)
            {
                return NotFound();
            }

            // Check if ModelState is valid before updating
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.ReservationID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // If ModelState is not valid, return to the Edit view with validation errors
            List<SelectListItem> ticketTypes = new List<SelectListItem>()
    {
        new SelectListItem {
            Text = "Regular", Value = "1"
        },
        new SelectListItem {
            Text = "Business", Value = "2"
        }
    };
            ViewData["TicketType"] = ticketTypes;
            var flightsFromTo = _context.Flight.Select(x => new
            {
                x.FlightID,
                FlightFromTo = x.FlightFrom + "-" + x.FlightTo
            });
            ViewData["FlightID"] = new SelectList(flightsFromTo, "FlightID", "FlightFromTo", reservation.FlightID);
            return View(reservation);
        }


        // GET: Reservation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .Include(r => r.Flight)
                .FirstOrDefaultAsync(m => m.ReservationID == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservation.Remove(reservation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservation.Any(e => e.ReservationID == id);
        }
    }
}
