using Laba4.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Lab6.Controllers
{
    [EnableCors("AllowSpecificOrigin")]
    [ApiController]
    [Route("[controller]")]
    public class SubsCityController : ControllerBase
    {
        private readonly SubsCityContext _context;

        public SubsCityController(SubsCityContext subsCityContext)
        {
            _context = subsCityContext;
        }
        [HttpGet]
        public IEnumerable<Subscription> Get()
        {
            var data = _context.Subscriptions.Include(e => e.Employee).ThenInclude(p => p.Position)
                                            .Include(p => p.Publication).ThenInclude(t => t.Type).ToList();
                                            
            return data;
        }
        [HttpGet("{id}")]
        public Subscription Get(int id)
        {
            var data = _context.Subscriptions.Include(e => e.Employee).ThenInclude(p => p.Position)
                                            .Include(p => p.Publication).ThenInclude(t => t.Type).FirstOrDefault(s => s.Id == id);

            return data;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Subscription subscription)
        {
            if (subscription == null)
            {
                return BadRequest();
            }

            _context.Subscriptions.Add(subscription);
            var result = _context.SaveChanges();
            return Ok(subscription);
        }
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Subscription updatedSubscription)
        {
            if (updatedSubscription == null)
            {
                return BadRequest();
            }

            var existingSubscription = _context.Subscriptions.FirstOrDefault(e => e.Id == id);

            if (existingSubscription == null)
            {
                return NotFound();
            }

            existingSubscription.RecipientId = updatedSubscription.RecipientId;
            existingSubscription.PublicationId = updatedSubscription.PublicationId;
            existingSubscription.Duration = updatedSubscription.Duration;
            existingSubscription.OfficeId = updatedSubscription.OfficeId;
            existingSubscription.EmployeeId = updatedSubscription.EmployeeId;
            existingSubscription.SubscriptionStartDate = updatedSubscription.SubscriptionStartDate;

            _context.SaveChanges();

            return Ok(existingSubscription);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Subscription insuranceCase = _context.Subscriptions.FirstOrDefault(e => e.Id == id);
            if (insuranceCase == null)
            {
                return NotFound();
            }

            _context.Subscriptions.Remove(insuranceCase);
            _context.SaveChanges();
            return Ok(insuranceCase);
        }
        
    }
}