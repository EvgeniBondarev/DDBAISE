using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Repository.Models;
using Utils;
using Domains.Models;

namespace PostCity.Controllers
{
    public class RecipientAddressesController : Controller
    {
        private readonly PostCityContext _context;
        private readonly SessionLogger _logger;

        public RecipientAddressesController(PostCityContext context, SessionLogger logger)
        {
            _context = context;
            _logger = logger;
        }


        [Authorize(Roles = "Admin")]
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

        
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Street,House,Apartment")] RecipientAddress recipientAddress)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recipientAddress);
                _logger.LogInformation($"Add new recipient address ({recipientAddress.FulAddress})");
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


        [Authorize(Roles = "Admin")]
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


        [Authorize(Roles = "Admin")]
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
                    _logger.LogInformation($"Edit recipient address ({recipientAddress.FulAddress})");
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


        [Authorize(Roles = "Admin")]
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


        [Authorize(Roles = "Admin")]
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
                _logger.LogInformation($"Delete recipient address ({recipientAddress.FulAddress})");
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
