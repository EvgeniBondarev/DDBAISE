using Laba4.Models;
using PostCity.Models;
using System.Globalization;

namespace Laba4.Data
{
    public class DbInitializer
    {
        private readonly SubsCityContext _context;
        private readonly Random _random;


        public DbInitializer(SubsCityContext context)
        {
            _context = context;
            _random = new Random();
        }

        public void InitializeDb()
        {
            ClearDatabase();

            InitializePublicationTypes();
            InitializePublications();

            InitializeRecipientAddresses();
            InitializeRecipients();

            InitializeOffices();

            InitializeEmployeePositions();
            InitializeEmployees();

            InitializeSubscriptions();

            Console.WriteLine("Db initialize");
        }
        private void ClearDatabase()
        {
            _context.Recipients.RemoveRange(_context.Recipients);
            _context.Subscriptions.RemoveRange(_context.Subscriptions);
            _context.Publications.RemoveRange(_context.Publications);
            _context.Employees.RemoveRange(_context.Employees);
            _context.Offices.RemoveRange(_context.Offices);
            _context.RecipientAddresses.RemoveRange(_context.RecipientAddresses);
            _context.EmployeePositions.RemoveRange(_context.EmployeePositions);
            _context.PublicationTypes.RemoveRange(_context.PublicationTypes);

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
                        Email = $"{Guid.NewGuid().ToString().Substring(0, 10)}@gmail.com"
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
    }
}
