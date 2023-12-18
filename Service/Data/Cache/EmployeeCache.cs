using Domains.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Repository;

namespace Service.Data.Cache
{
    public class EmployeeCache : AppCache<Employee>
    {
        public EmployeeCache(PostCityContext db, IMemoryCache memoryCache) : base(db, memoryCache)
        {
        }

        public override IEnumerable<Employee> Get()
        {
            _cache.TryGetValue("Employees", out IEnumerable<Employee>? emploees);

            if (emploees is null)
            {
                emploees = Set();
            }
            return emploees;
        }

        public override IEnumerable<Employee> Set()
        {
            var emploees = _db.Employees
                .Include(e => e.Position)
                .Include(e => e.Office)
                .ToList(); 
            _cache.Set("Employees", emploees, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_saveTime)));
            return emploees;
        }

        public override IEnumerable<Employee> Set(IEnumerable<Employee> emploees)
        {
            _cache.Set("Employees", emploees, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_saveTime)));
            return emploees;
        }

    }
}
