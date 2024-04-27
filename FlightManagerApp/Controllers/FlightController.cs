using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlightManagerApp.Data;
using FlightManagerApp.Models;
using X.PagedList;

namespace FlightManagerApp.Controllers
{
    public class FlightController : Controller
    {
        private readonly FlightManagerContext _context;

        public FlightController(FlightManagerContext context)
        {
            _context = context;
        }

        // GET: Flight
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.FromSortParm = String.IsNullOrEmpty(sortOrder) ? "from_desc" : "";
            ViewBag.ToSortParm = sortOrder == "To" ? "to_desc" : "To";
            ViewBag.TakeOffTimeSortParm = sortOrder == "TakeOffTime" ? "takeoff_desc" : "TakeOffTime";
            ViewBag.LandingTimeSortParm = sortOrder == "LandingTime" ? "landing_desc" : "LandingTime";
            ViewBag.CapacitySortParm = sortOrder == "Capacity" ? "capacity_desc" : "Capacity";
            ViewBag.BusinessCapacitySortParm = sortOrder == "BusinessCapacity" ? "business_capacity_desc" : "BusinessCapacity";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var flights = from f in _context.Flight
                          select f;

            if (!String.IsNullOrEmpty(searchString))
            {
                flights = flights.Where(s => s.FlightFrom.Contains(searchString) || s.FlightTo.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "from_desc":
                    flights = flights.OrderByDescending(s => s.FlightFrom);
                    break;
                case "To":
                    flights = flights.OrderBy(s => s.FlightTo);
                    break;
                case "to_desc":
                    flights = flights.OrderByDescending(s => s.FlightTo);
                    break;
                case "TakeOffTime":
                    flights = flights.OrderBy(s => s.TakeOffTime);
                    break;
                case "takeoff_desc":
                    flights = flights.OrderByDescending(s => s.TakeOffTime);
                    break;
                case "LandingTime":
                    flights = flights.OrderBy(s => s.LandingTime);
                    break;
                case "landing_desc":
                    flights = flights.OrderByDescending(s => s.LandingTime);
                    break;
                case "Capacity":
                    flights = flights.OrderBy(s => s.PlaneCapacity);
                    break;
                case "capacity_desc":
                    flights = flights.OrderByDescending(s => s.PlaneCapacity);
                    break;
                case "BusinessCapacity":
                    flights = flights.OrderBy(s => s.PlaneBusinessClassCapacity);
                    break;
                case "business_capacity_desc":
                    flights = flights.OrderByDescending(s => s.PlaneBusinessClassCapacity);
                    break;
                default:
                    flights = flights.OrderBy(s => s.FlightFrom);
                    break;
            }

            int pageSize = 3; // Change the page size as needed
            int pageNumber = (page ?? 1);
            return View(await flights.ToPagedListAsync(pageNumber, pageSize));
        }


        // GET: Flight/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flight
                .Include(f => f.Reservations) // Ensure reservations are loaded
                .FirstOrDefaultAsync(m => m.FlightID == id);

            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }


        // GET: Flight/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Flight/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FlightID,FlightFrom,FlightTo,TakeOffTime,LandingTime,PlaneType,PlaneNumber,PilotName,PlaneCapacity,PlaneBusinessClassCapacity")] Flight flight)
        {
            if (flight.TakeOffTime >= flight.LandingTime)
            {
                ModelState.AddModelError(nameof(flight.TakeOffTime), "Take Off time can't be after or the same as landing time.");
                ModelState.AddModelError(nameof(flight.LandingTime), "Landing time can't be before or the same as take Off time.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(flight);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(flight);
        }

        // GET: Flight/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flight.FindAsync(id);
            if (flight == null)
            {
                return NotFound();
            }
            return View(flight);
        }

        // POST: Flight/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FlightID,FlightFrom,FlightTo,TakeOffTime,LandingTime,PlaneType,PlaneNumber,PilotName,PlaneCapacity,PlaneBusinessClassCapacity")] Flight flight)
        {
            if (id != flight.FlightID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(flight);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlightExists(flight.FlightID))
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
            return View(flight);
        }

        // GET: Flight/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flight
                .FirstOrDefaultAsync(m => m.FlightID == id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        // POST: Flight/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flight = await _context.Flight.FindAsync(id);
            if (flight != null)
            {
                _context.Flight.Remove(flight);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FlightExists(int id)
        {
            return _context.Flight.Any(e => e.FlightID == id);
        }
    }
}
