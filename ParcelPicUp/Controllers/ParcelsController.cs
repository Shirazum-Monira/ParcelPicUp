using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using parcelPicUp.Data;
using parcelPicUp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace parcelPicUp.Controllers
{
    [Authorize(Roles = "Admin,DeliveryAgent,Customer")]
    public class ParcelsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ParcelsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Parcels
        public async Task<IActionResult> Index()
        {
            var parcels = await _context.Parcel
                .Include(p => p.Sender)
                .Include(p => p.Receiver)
                .Include(p => p.DeliveryAgent)
                .ToListAsync();
            return View(parcels);
        }

        // GET: Parcels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var parcel = await _context.Parcel
                .Include(p => p.Sender)
                .Include(p => p.Receiver)
                .Include(p => p.DeliveryAgent)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (parcel == null) return NotFound();

            return View(parcel);
        }

        // GET: Parcels/Create
        public async Task<IActionResult> Create()
        {
            var users = await _userManager.Users.ToListAsync();

            var deliveryAgents = new List<ApplicationUser>();
            var customers = new List<ApplicationUser>();

            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "DeliveryAgent"))
                    deliveryAgents.Add(user);

                if (await _userManager.IsInRoleAsync(user, "Customer"))
                    customers.Add(user);
            }

            ViewBag.ReceiverId = new SelectList(customers, "Id", "FullName");
            ViewBag.DeliveryAgentId = new SelectList(deliveryAgents, "Id", "FullName");

            return View();
        }

        // POST: Parcels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Parcel parcel)
        {
            if (ModelState.IsValid)
            {
                parcel.SenderId = _userManager.GetUserId(User); // Current user as sender
                parcel.CreatedAt = DateTime.Now;
                _context.Add(parcel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Rebuild dropdowns in case of error
            var users = await _userManager.Users.ToListAsync();

            var deliveryAgents = new List<ApplicationUser>();
            var customers = new List<ApplicationUser>();

            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "DeliveryAgent"))
                    deliveryAgents.Add(user);

                if (await _userManager.IsInRoleAsync(user, "Customer"))
                    customers.Add(user);
            }

            ViewBag.ReceiverId = new SelectList(customers, "Id", "FullName", parcel.ReceiverId);
            ViewBag.DeliveryAgentId = new SelectList(deliveryAgents, "Id", "FullName", parcel.DeliveryAgentId);

            return View(parcel);
        }

        // GET: Parcels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var parcel = await _context.Parcel.FindAsync(id);
            if (parcel == null) return NotFound();

            var users = await _userManager.Users.ToListAsync();

            var deliveryAgents = new List<ApplicationUser>();
            var customers = new List<ApplicationUser>();

            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "DeliveryAgent"))
                    deliveryAgents.Add(user);

                if (await _userManager.IsInRoleAsync(user, "Customer"))
                    customers.Add(user);
            }

            ViewBag.ReceiverId = new SelectList(customers, "Id", "FullName", parcel.ReceiverId);
            ViewBag.DeliveryAgentId = new SelectList(deliveryAgents, "Id", "FullName", parcel.DeliveryAgentId);

            return View(parcel);
        }

        // POST: Parcels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Parcel parcel)
        {
            if (id != parcel.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parcel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParcelExists(parcel.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            var users = await _userManager.Users.ToListAsync();

            var deliveryAgents = new List<ApplicationUser>();
            var customers = new List<ApplicationUser>();

            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "DeliveryAgent"))
                    deliveryAgents.Add(user);

                if (await _userManager.IsInRoleAsync(user, "Customer"))
                    customers.Add(user);
            }

            ViewBag.ReceiverId = new SelectList(customers, "Id", "FullName", parcel.ReceiverId);
            ViewBag.DeliveryAgentId = new SelectList(deliveryAgents, "Id", "FullName", parcel.DeliveryAgentId);

            return View(parcel);
        }

        // GET: Parcels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var parcel = await _context.Parcel
                .Include(p => p.Sender)
                .Include(p => p.Receiver)
                .Include(p => p.DeliveryAgent)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (parcel == null) return NotFound();

            return View(parcel);
        }

        // POST: Parcels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parcel = await _context.Parcel.FindAsync(id);
            if (parcel != null)
            {
                _context.Parcel.Remove(parcel);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ParcelExists(int id)
        {
            return _context.Parcel.Any(e => e.Id == id);
        }
    }
}
