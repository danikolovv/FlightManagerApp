using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlightManagerApp.Data;
using FlightManagerApp.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FlightManagerApp.Areas.Identity.Data;
using AutoMapper.QueryableExtensions;

namespace FlightManagerApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public UserController(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var users = userManager.Users
                .ProjectTo<AdminModel>(mapper.ConfigurationProvider)
                .ToArray();

            return View(users);
        }

        // GET: Display delete confirmation page
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var adminModel = mapper.Map<AdminModel>(user);
            if (adminModel == null)
            {
                return NotFound();
            }

            return View(adminModel);
        }

        // POST: Delete user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                // Redirect to the index page after successful deletion
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // If deletion fails, handle the error (e.g., display error message)
                // and return to the delete confirmation page
                return RedirectToAction(nameof(Delete), new { id });
            }
        }

        // GET: Display user details
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var adminModel = mapper.Map<AdminModel>(user);
            if (adminModel == null)
            {
                return NotFound();
            }

            return View(adminModel);
        }

        // GET: Display edit form
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var adminModel = mapper.Map<AdminModel>(user);
            if (adminModel == null)
            {
                return NotFound();
            }

            return View(adminModel);
        }

        // POST: Handle form submission for editing user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, AdminModel adminModel)
        {
            if (id != adminModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                // Update user properties with values from the submitted form
                user.UserName = adminModel.UserName;
                user.FirstName = adminModel.FirstName;
                user.LastName = adminModel.LastName;
                user.EGN = adminModel.EGN;
                user.Address = adminModel.Address;
                user.Email = adminModel.Email;
                user.PhoneNumber = adminModel.PhoneNumber;

                // Update the user in the database
                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    // Redirect to the index page after successful update
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // If update fails, handle the error (e.g., display error message)
                    // and return to the edit form
                    ModelState.AddModelError("", "Failed to update user.");
                    return View(adminModel);
                }
            }

            // If model state is not valid, redisplay the edit form with validation errors
            return View(adminModel);
        }

        // GET: Display create form
        public IActionResult Create()
        {
            return View();
        }

        /*// POST: Handle form submission for creating a new user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminModel adminModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = adminModel.UserName,
                    FirstName = adminModel.FirstName,
                    LastName = adminModel.LastName,
                    EGN = adminModel.EGN,
                    Address = adminModel.Address,
                    Email = adminModel.Email,
                    PhoneNumber = adminModel.PhoneNumber
                };

                var result = await userManager.CreateAsync(user, adminModel.Password);

                if (result.Succeeded)
                {
                    // Redirect to the index page after successful creation
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // If creation fails, handle the error (e.g., display error message)
                    // and return to the create form
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(adminModel);
                }
            }

            // If model state is not valid, redisplay the create form with validation errors
            return View(adminModel);
        }*/


    }
}
