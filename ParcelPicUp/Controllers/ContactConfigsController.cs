using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ParcelPicUp.Models;
using parcelPicUp.Data;

namespace ParcelPicUp.Controllers
{
    public class ContactConfigsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactConfigsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ContactConfigs
        public async Task<IActionResult> Index()
        {
            return View(await _context.ContactConfig.ToListAsync());
        }

        // GET: ContactConfigs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactConfig = await _context.ContactConfig
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactConfig == null)
            {
                return NotFound();
            }

            return View(contactConfig);
        }

        // GET: ContactConfigs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ContactConfigs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FormName,FormEmail,FormEmailPassword,ToName,ToEmail,SMTPHost,SMTPPort,IsActive,CTime,CreatedBy,MTime,ModifiedBy")] ContactConfig contactConfig)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contactConfig);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contactConfig);
        }

        // GET: ContactConfigs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactConfig = await _context.ContactConfig.FindAsync(id);
            if (contactConfig == null)
            {
                return NotFound();
            }
            return View(contactConfig);
        }

        // POST: ContactConfigs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FormName,FormEmail,FormEmailPassword,ToName,ToEmail,SMTPHost,SMTPPort,IsActive,CTime,CreatedBy,MTime,ModifiedBy")] ContactConfig contactConfig)
        {
            if (id != contactConfig.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contactConfig);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactConfigExists(contactConfig.Id))
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
            return View(contactConfig);
        }

        // GET: ContactConfigs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactConfig = await _context.ContactConfig
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactConfig == null)
            {
                return NotFound();
            }

            return View(contactConfig);
        }

        // POST: ContactConfigs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contactConfig = await _context.ContactConfig.FindAsync(id);
            if (contactConfig != null)
            {
                _context.ContactConfig.Remove(contactConfig);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactConfigExists(int id)
        {
            return _context.ContactConfig.Any(e => e.Id == id);
        }
    }
}
