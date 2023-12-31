﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Laba4.Models;
using PostCity.Models;
using PostCity.ViewModels.Filters.FilterModel;
using PostCity.ViewModels.Sort;
using PostCity.ViewModels;
using System.Net;
using Laba4.ViewModels.Sort;
using PostCity.Data.Cache;
using PostCity.Data.Cookies;
using PostCity.Infrastructure.Filters;
using Laba4.Data.Cache;
using Laba4.ViewModels.Filters.FilterModel;
using PostCity.ViewModels.Filters;
using Newtonsoft.Json;
using Laba4.Data;
using Laba4.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Laba4.Controllers
{
    
    public class RecipientsController : Controller, ISortOrderController<Recipient, RecipientSortState>
    {
        private readonly PostCityContext _context;
        private readonly RecipientCache _cache;
        private readonly CookiesManeger _cookies;
        private readonly FilterBy<Recipient> _filter;
        private readonly UserRegistrationManager _userRegistrationManager;
        private readonly UserManager<PostCityUser> _userManager;
        private readonly CacheUpdater _cacheUpdater;
        private readonly SessionLogger _logger;

        public RecipientsController(PostCityContext context, 
                                    RecipientCache recipientCache,
                                    CookiesManeger cookiesManeger,
                                    FilterBy<Recipient> filterBy,
                                    UserRegistrationManager userRegistrationManager,
                                    UserManager<PostCityUser> userManager,
                                    UserCache userCache,
                                    CacheUpdater cacheUpdater,
                                    SessionLogger logger)
        {
            _context = context;
            _cache = recipientCache;
            _cookies = cookiesManeger;
            _filter = filterBy;
            _userRegistrationManager = userRegistrationManager;
            _userManager = userManager;
            _cacheUpdater = cacheUpdater;
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(RecipientSortState sortOrder = RecipientSortState.StandardState, int page = 1)
        {
            var postCityContext = _cache.Get();

            RecipientFilterModel filterData = _cookies.GetFromCookies<RecipientFilterModel>(Request.Cookies, "RecipientFilterData");

            SetSortOrderViewData(sortOrder);
            postCityContext = ApplySortOrder(postCityContext, sortOrder);

            int pageSize = 10;
            _cache.Set(postCityContext);

            var pageViewModel = new PageViewModel<Recipient, RecipientFilterModel>(postCityContext, page, pageSize, filterData);
            return View(pageViewModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Index(RecipientFilterModel filterData, int page = 1)
        {
            _cache.Update();
            _cookies.SaveToCookies(Response.Cookies, "RecipientFilterData", filterData);

            var data = _cache.Get();

            data = _filter.FilterByString(data, n => n.Name, filterData.Name);
            data = _filter.FilterByString(data, s => s.Surname, filterData.Surname);
            data = _filter.FilterByString(data, m => m.Middlename, filterData.Middlename);
            data = _filter.FilterByString(data, m => m.MobilePhone, filterData.MobilePhone);
            data = _filter.FilterByString(data, a => a.Address.FulAddress, filterData.Address);

            int pageSize = 10;
            _cache.Set(data);

            var pageViewModel = new PageViewModel<Recipient, RecipientFilterModel>(data, page, pageSize, filterData);
            return View(pageViewModel);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Recipients == null)
            {
                return NotFound();
            }

            var recipient = _cache.Get().FirstOrDefault(m => m.Id == id);
            
            if (recipient == null)
            {
                return NotFound();
            }

            return View(recipient);
        }

        public IActionResult Register()
        {
            if (Request.Cookies.TryGetValue("RecipientFields", out string recipientFieldsJson))
            {
                var recipientFields = JsonConvert.DeserializeAnonymousType(recipientFieldsJson, new
                {
                    Name = "",
                    Middlename = "",
                    Surname = "",
                    MobilePhone = ""
                });

                ViewData["Name"] = recipientFields.Name;
                ViewData["Middlename"] = recipientFields.Middlename;
                ViewData["Surname"] = recipientFields.Surname;
                ViewData["MobilePhone"] = recipientFields.MobilePhone;
            }
            if (Request.Cookies.TryGetValue("UserCredentials", out string userCredentialsJson))
            {
                var userCredentials = JsonConvert.DeserializeAnonymousType(userCredentialsJson, new
                {
                    Email = "",
                    Password = "",
                    Address = new RecipientAddress()
                });

                if (userCredentials.Address.Street != null)
                {
                    ViewData["Address"] = userCredentials.Address.FulAddress;
                }
            }

                return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> SaveFields([Bind("Id,Name,Middlename,Surname, MobilePhone")] Recipient recipient)
        {
            if (ModelState.IsValid)
            {
                var credentials = new { Name=recipient.Name, 
                                        Middlename=recipient.Middlename, 
                                        Surname=recipient.Surname,
                                        MobilePhone = recipient.MobilePhone,
                                       };
                var credentialsJson = JsonConvert.SerializeObject(credentials);
                Response.Cookies.Append("RecipientFields", credentialsJson);

                return RedirectToAction("Create", "RecipientAddresses");
            }

            ViewData["AddressId"] = new SelectList(_context.RecipientAddresses, "Id", "FulAddress", recipient.AddressId);
            return View(recipient);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,Name,Middlename,Surname, MobilePhone")] Recipient recipient)
        {
            if (ModelState.IsValid)
            {
                if (Request.Cookies.TryGetValue("UserCredentials", out string credentialsJson))
                {
                    var credentials = JsonConvert.DeserializeAnonymousType(credentialsJson, new { Email = "", 
                                                                                                  Password = "", 
                                                                                                  Address = new RecipientAddress() });

                    using var transaction = await _context.Database.BeginTransactionAsync();

                    if(credentials.Address.Street != null)
                    {
                        try
                        {
                            var newRecipient = new Recipient()
                            {
                                Name = recipient.Name,
                                Middlename = recipient.Middlename,
                                Surname = recipient.Surname,
                                MobilePhone = recipient.MobilePhone,
                                AddressId = credentials.Address.Id
                            };

                            _context.Add(newRecipient);
                            await _context.SaveChangesAsync();

                            var result = await _userRegistrationManager.RegisterUserWithRole(new PostCityUserModel()
                            {
                                Role = "Recipient",
                                Email = credentials.Email,
                                Password = credentials.Password,
                                UserId = newRecipient.Id
                            });

                            if (result.Succeeded)
                            {
                                transaction.Commit();
                                _cacheUpdater.Update(_cache);
                                Response.Cookies.Delete("RecipientFields");
                                Response.Cookies.Delete("UserCredentials");
                                _logger.LogInformation($"Add new recipient ({newRecipient.FullName})");
                                return Redirect("/Identity/Account/Login");

                            }
                            else
                            {
                                transaction.Rollback();
                                _context.Recipients.Remove(recipient);
                                _context.RecipientAddresses.Remove(credentials.Address);

                                foreach (var error in result.Errors)
                                {
                                    ViewData["AddressId"] = new SelectList(_context.RecipientAddresses, "Id", "FulAddress", recipient.AddressId);
                                    return View(recipient);
                                }
                            }
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            _context.Recipients.Remove(recipient);
                            _context.RecipientAddresses.Remove(credentials.Address);
                        }
                    }
                    else
                    {
                        ViewData["ErrorMessage"] = "Address is required. Please add an address before registering.";
                        return View(recipient);
                    }
                }
            }
            

            ViewData["AddressId"] = new SelectList(_context.RecipientAddresses, "Id", "FulAddress", recipient.AddressId);
            return View(recipient);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Recipients == null)
            {
                return NotFound();
            }

            var recipient = await _context.Recipients.FindAsync(id);
            if (recipient == null)
            {
                return NotFound();
            }
            ViewData["AddressId"] = new SelectList(_context.RecipientAddresses, "Id", "FulAddress", recipient.AddressId);
            return View(recipient);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Middlename,Surname,AddressId,MobilePhone")] Recipient recipient)
        {
            if (id != recipient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    await _context.SaveChangesAsync();
                    _cacheUpdater.Update(_cache);
                    _logger.LogInformation($"Edit recipient ({recipient.FullName})");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipientExists(recipient.Id))
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
            ViewData["AddressId"] = new SelectList(_context.RecipientAddresses, "Id", "FulAddress", recipient.AddressId);
            return View(recipient);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Recipients == null)
            {
                return NotFound();
            }

            var recipient = _cache.Get().FirstOrDefault(m => m.Id == id);
            if (recipient == null)
            {
                return NotFound();
            }

            return View(recipient);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Recipients == null)
            {
                return Problem("Entity set 'SubsCityContext.Recipients'  is null.");
            }
            var recipient = await _context.Recipients.FindAsync(id);
            if (recipient != null)
            {
                _context.Recipients.Remove(recipient);
                _logger.LogInformation($"Delete recipient ({recipient.FullName})");
            }
            
            int isDelete = await _context.SaveChangesAsync();
            if (isDelete == 1)
            {
                PostCityUser user = _context.FindUserByUserId(recipient.Id);
                IdentityResult result = await _userManager.DeleteAsync(user);
            }
            _cacheUpdater.Update(_cache);
            return RedirectToAction(nameof(Index));
        }

        private bool RecipientExists(int id)
        {
          return (_context.Recipients?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        public void SetSortOrderViewData(RecipientSortState sortOrder)
        {
            ViewData["NameSort"] = sortOrder == RecipientSortState.NameAsc
                ? RecipientSortState.NameDesc
                : RecipientSortState.NameAsc;

            ViewData["MiddlenameSort"] = sortOrder == RecipientSortState.MiddlenameAsc
                ? RecipientSortState.MiddlenameDesc
                : RecipientSortState.MiddlenameAsc;

            ViewData["SurnameSort"] = sortOrder == RecipientSortState.SurnameAsc
                ? RecipientSortState.SurnameDesc
                : RecipientSortState.SurnameAsc;

            ViewData["AddressSort"] = sortOrder == RecipientSortState.AddressAsc
                ? RecipientSortState.AddressDesc
                : RecipientSortState.AddressAsc;
        }

        public IEnumerable<Recipient> ApplySortOrder(IEnumerable<Recipient> postCityContext, RecipientSortState sortOrder)
        {
            return sortOrder switch
            {
                RecipientSortState.NameAsc => postCityContext.OrderByDescending(n => n.Name),
                RecipientSortState.NameDesc => postCityContext.OrderBy(n => n.Name),

                RecipientSortState.MiddlenameAsc => postCityContext.OrderByDescending(m => m.Middlename),
                RecipientSortState.MiddlenameDesc => postCityContext.OrderBy(m => m.Middlename),

                RecipientSortState.SurnameAsc => postCityContext.OrderByDescending(s => s.Surname),
                RecipientSortState.SurnameDesc => postCityContext.OrderBy(s => s.Surname),

                RecipientSortState.AddressAsc => postCityContext.OrderByDescending(a => a.Address.Street),
                RecipientSortState.AddressDesc => postCityContext.OrderBy(a => a.Address.Street),

                RecipientSortState.StandardState => postCityContext
            };
        }
    }
}
