using Laba4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PostCity.Models;
using System.Globalization;

namespace Laba4.Data
{
    public class DbInitializer
    {
        private readonly PostCityContext _context;
        private readonly Random _random;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<PostCityUser> _userManager;


        public DbInitializer(PostCityContext context,  
                            RoleManager<IdentityRole> roleManager, 
                            UserManager<PostCityUser> userManager)
        {
            _context = context;
            _random = new Random();
            _roleManager = roleManager;
            _userManager = userManager;

        }

        public void InitializeDb()
        {
            if (!_context.Database.CanConnect())
            {
                _context.Database.Migrate();
            }


            if (!_roleManager.Roles.Any())
            {
                CreateRoles().Wait();
            }
            if (!_userManager.Users.Any())
            {
                CreateAdmin().Wait();
            };


            if (!_context.PublicationTypes.Any())
            {
                InitializePublicationTypes();
            }
            if (!_context.Publications.Any())
            {
                InitializePublications();
            }
            if (!_context.RecipientAddresses.Any())
            {
                InitializeRecipientAddresses();
            }
            if (!_context.Recipients.Any())
            {
                InitializeRecipients();
                CreateRecipient().Wait();
            }
            if (!_context.Offices.Any())
            {
                InitializeOffices();
            }
            if (!_context.EmployeePositions.Any())
            {
                InitializeEmployeePositions();
            }
            if (!_context.Employees.Any())
            {
                InitializeEmployees();
                CreateEmployees().Wait();
            }
            if (!_context.Subscriptions.Any())
            {
                InitializeSubscriptions();
            }

            _context.SaveChanges();

           
        }


        string[] lastNames = {
        "Иванов",
        "Петров",
        "Смирнов",
        "Сидоров",
        "Козлов",
        "Морозов",
        "Васильев",
        };


        string[] firstNames = {
        "Александр",
        "Иван",
        "Михаил",
        "Евгений",
        "Сергей",
        "Дмитрий"
         };

        string[] middleNames = {
        "Иванович",
        "Александрович",
        "Петрович",
        "Сергеевич",
        "Владимирович",
        "Игоревна",
        "Дмитриевич"
        };

        string[] streetNames = {
            "Пролетарская улица",
            "Ленинская улица",
            "Гагарина улица",
            "Советская улица",
            "Пушкинская улица",
            "Московская улица",
            "Кировская улица",
            "Парковая улица",
            "Садовая улица",
            "Комсомольская улица",
            "Школьная улица",
            "Жукова улица",
            "Мичурина улица",
            "Свердлова улица",
            "Октябрьская улица",
            "Горького улица"
        };

        private void InitializePublicationTypes()
        {
            if (!_context.PublicationTypes.Any())
            {
                _context.PublicationTypes.AddRange(
                    new PublicationType { Type = "газета" },
                    new PublicationType { Type = "журнал" }
                );

                _context.SaveChanges();
            }
        }
        private void InitializePublications()
        {
            if (!_context.Publications.Any())
            {
                var publicationNames = new List<string>
            {
                "Утренние новости",
                "Вечерний вестник",
                "Асахи симбун",
                "Комсомольская правда",
                "Взгляд"
            };

                var publicationTypes = _context.PublicationTypes.ToList();

                for (int i = 0; i <= 100; i++)
                {
                    var randomType = publicationTypes[_random.Next(publicationTypes.Count())];
                    var randomName = publicationNames[_random.Next(publicationNames.Count())];
                    var price = (decimal)(_random.NextDouble() * 100);

                    _context.Publications.Add(new Publication
                    {
                        Type = randomType,
                        Name = randomName,
                        Price = price
                    });

                }

                _context.SaveChanges();
            }
        }
        private void InitializeEmployeePositions()
        {
            if (!_context.EmployeePositions.Any())
            {
                _context.EmployeePositions.AddRange(
                    new EmployeePosition { Position = "Почтальон" },
                    new EmployeePosition { Position = "Кассир" },
                    new EmployeePosition { Position = "Сортировщик" },
                    new EmployeePosition { Position = "Менеджер по доставке" }
                );

                _context.SaveChanges();
            }
        }

        private void InitializeEmployees()
        {
            if (!_context.Employees.Any())
            {
                var employeePositions = _context.EmployeePositions.ToList();
                var offices = _context.Offices.ToList();

                for (int i = 0; i <= 100; i++)
                {
                    var randomPosition = employeePositions[_random.Next(employeePositions.Count())];
                    var randomOffice = offices[_random.Next(offices.Count())];

                    _context.Employees.Add(new Employee
                    {
                        Name = firstNames[_random.Next(firstNames.Length)],
                        Middlename = middleNames[_random.Next(middleNames.Length)],
                        Surname = lastNames[_random.Next(lastNames.Length)],
                        Position = randomPosition,
                        Office = randomOffice
                    });
                }

                _context.SaveChanges();
            }
        }

        private void InitializeOffices()
        {
            if (!_context.Offices.Any())
            {
                for (int i = 0; i <= 100; i++)
                {
                    var randomStreet = streetNames[_random.Next(streetNames.Length)];
                    var house = _random.Next(1, 101);

                    _context.Offices.Add(new Office
                    {
                        OwnerName = firstNames[_random.Next(firstNames.Length)],
                        OwnerMiddlename = middleNames[_random.Next(middleNames.Length)],
                        OwnerSurname = lastNames[_random.Next(lastNames.Length)],
                        StreetName = $"{randomStreet}. д {house}",
                        MobilePhone = $"+375{_random.Next(100000000, 999999999)}",
                        Email = $"{Guid.NewGuid().ToString().Substring(0, 10)}@gmail.com"
                    });
                }

                _context.SaveChanges();
            }
        }

        private void InitializeRecipientAddresses()
        {
            if (!_context.RecipientAddresses.Any())
            {

                for (int i = 0; i <= 100; i++)
                {
                    var randomStreet = streetNames[_random.Next(streetNames.Length)];
                    var house = _random.Next(1, 101);
                    var apartment = _random.Next(1, 21);

                    _context.RecipientAddresses.Add(new RecipientAddress
                    {
                        Street = randomStreet,
                        House = house,
                        Apartment = apartment
                    });
                }

                _context.SaveChanges();
            }
        }

        private void InitializeRecipients()
        {
            if (!_context.Recipients.Any())
            {
                var recipientAddresses = _context.RecipientAddresses.ToList();

                for (int i = 0; i <= 100; i++)
                {
                    var randomAddress = recipientAddresses[_random.Next(recipientAddresses.Count())];

                    _context.Recipients.Add(new Recipient
                    {
                        Name = firstNames[_random.Next(firstNames.Length)],
                        Middlename = middleNames[_random.Next(middleNames.Length)],
                        Surname = lastNames[_random.Next(lastNames.Length)],
                        Address = randomAddress,
                        MobilePhone = $"+375{_random.Next(100000000, 999999999)}",
                    });
                }

                _context.SaveChanges();
            }
        }

        private void InitializeSubscriptions()
        {
            if (!_context.Subscriptions.Any())
            {
                var recipients = _context.Recipients.ToList();
                var publications = _context.Publications.ToList();
                var offices = _context.Offices.ToList();
                var employees = _context.Employees.ToList();

                for (int i = 0; i < 100; i++)
                {
                    var randomRecipient = recipients[_random.Next(recipients.Count())];
                    var randomPublication = publications[_random.Next(publications.Count())];
                    var randomEmployee = employees[_random.Next(employees.Count())];
                    var randomOffice = offices[_random.Next(offices.Count())];
                    var duration = _random.Next(1, 13);
                    var dateStr = $"{_random.Next(1, 13).ToString().PadLeft(2, '0')}.{_random.Next(2014, 2024)}";

                    DateTime startDate;
                    if (DateTime.TryParseExact(dateStr, "MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
                    {
                        _context.Subscriptions.Add(new Subscription
                        {
                            Recipient = randomRecipient,
                            Publication = randomPublication,
                            Duration = duration,
                            Office = randomOffice,
                            Employee = randomEmployee,
                            SubscriptionStartDate = startDate
                        });
                    }
                }

                _context.SaveChanges();
            }
        }
        //Identity
        private async Task CreateRoles()
        {
            string[] roleNames = { "Admin", "Employee", "Recipient" };

            foreach (var roleName in roleNames)
            {

                var roleExist = await _roleManager.RoleExistsAsync(roleName);

                if (!roleExist)
                {
                    IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        private async Task CreateAdmin()
        {
            if (_userManager.FindByNameAsync("admin@gmail.com").Result == null)
            {
                PostCityUser user = new PostCityUser
                {
                    UserId = 1,
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com"
                };

                IdentityResult result = await _userManager.CreateAsync(user, "EQRu~ha+75hqIcr");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
            }

        }
        public async Task CreateEmployees()
        {
            var employeeToDelete = await _userManager.GetUsersInRoleAsync("Employee");
            foreach (var recipient in employeeToDelete)
            {
                var result = await _userManager.DeleteAsync(recipient);

            }

            var employees = await _context.Employees.ToListAsync();

            foreach (var employee in employees)
            {
                var existingUser = await _userManager.FindByEmailAsync($"Employee{employee.Id}@gmail.com");

                if (existingUser == null)
                {
                    PostCityUser user = new PostCityUser
                    {
                        UserId = employee.Id,
                        UserName = $"Employee{employee.Id}@gmail.com",
                        Email = $"Employee{employee.Id}@gmail.com"
                    };
                    var password = $"Employee_{employee.Id}";
                    IdentityResult result = await _userManager.CreateAsync(user, password);

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "Employee");
                    }
                }
            }
        }
        public async Task CreateRecipient()
        {
            var recipientsToDelete = await _userManager.GetUsersInRoleAsync("Recipient");
            foreach (var recipient in recipientsToDelete)
            {
                var result = await _userManager.DeleteAsync(recipient);

            }


            var recipients = await _context.Recipients.ToListAsync();

            foreach (var recipient in recipients)
            {
                var existingUser = await _userManager.FindByEmailAsync($"Recipient{recipient.Id}@gmail.com");

                if (existingUser == null)
                {
                    PostCityUser user = new PostCityUser
                    {
                        UserId = recipient.Id,
                        UserName = $"Recipient{recipient.Id}@gmail.com",
                        Email = $"Recipient{recipient.Id}@gmail.com"
                    };
                    var password = $"Recipient_{recipient.Id}";
                    IdentityResult result = await _userManager.CreateAsync(user, password);

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "Recipient");
                    }
                }
            }
        }
    }
}
