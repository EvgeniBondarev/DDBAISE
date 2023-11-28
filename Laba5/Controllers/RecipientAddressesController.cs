using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Laba4.Models;
using PostCity.Models;
using Newtonsoft.Json;

namespace Laba4.Controllers
{
    public class RecipientAddressesController : Controller
    {
        private readonly PostCityContext _context;

        public RecipientAddressesController(PostCityContext context)
        {
            _context = context;
        }

        // GET: RecipientAddresses
        public async Task<IActionResult> Index()
        {
              return _context.RecipientAddresses != null ? 
                          View(await _context.RecipientAddresses.ToListAsync()) :
                          Problem("Entity set 'PostCityContext.RecipientAddresses'  is null.");
        }

        // GET: RecipientAddresses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RecipientAddresses == null)
            {
                return NotFound();
            }

            var recipientAddress = await _context.RecipientAddresses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipientAddress == null)
            {
                return NotFound();
            }

            return View(recipientAddress);
        }

        // GET: RecipientAddresses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RecipientAddresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Street,House,Apartment")] RecipientAddress recipientAddress)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recipientAddress);
                int result = await _context.SaveChangesAsync();
                if(result == 1)
                {
                    if (Request.Cookies.TryGetValue("UserCredentials", out string credentialsJson))
                    {
                        var credentials = JsonConvert.DeserializeAnonymousType(credentialsJson, new { Email = "",
                                                                                                      Password = "",
                                                                                                      Address = new RecipientAddress() });

                        var updatedCredentials = new { Email = credentials.Email, Password = credentials.Password, Address = recipientAddress };

                        var updatedCredentialsJson = JsonConvert.SerializeObject(updatedCredentials);

                        Response.Cookies.Append("UserCredentials", updatedCredentialsJson);

                    }
                    return RedirectToAction("Register", "Recipients");
                }   
            }
            return View(recipientAddress);
        }

        // GET: RecipientAddresses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RecipientAddresses == null)
            {
                return NotFound();
            }

            var recipientAddress = await _context.RecipientAddresses.FindAsync(id);
            if (recipientAddress == null)
            {
                return NotFound();
            }
            return View(recipientAddress);
        }

        // POST: RecipientAddresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Street,House,Apartment")] RecipientAddress recipientAddress)
        {
            if (id != recipientAddress.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipientAddress);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipientAddressExists(recipientAddress.Id))
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
            return View(recipientAddress);
        }

        // GET: RecipientAddresses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RecipientAddresses == null)
            {
                return NotFound();
            }

            var recipientAddress = await _context.RecipientAddresses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipientAddress == null)
            {
                return NotFound();
            }

            return View(recipientAddress);
        }

        // POST: RecipientAddresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RecipientAddresses == null)
            {
                return Problem("Entity set 'PostCityContext.RecipientAddresses'  is null.");
            }
            var recipientAddress = await _context.RecipientAddresses.FindAsync(id);
            if (recipientAddress != null)
            {
                _context.RecipientAddresses.Remove(recipientAddress);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipientAddressExists(int id)
        {
          return (_context.RecipientAddresses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
