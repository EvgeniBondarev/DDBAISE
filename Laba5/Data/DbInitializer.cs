using Laba4.Models;

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
            //_context.Database.EnsureCreated();
            ClearDatabase();

            InitializePublicationTypes();
            InitializePublications();
            InitializeSubscriptions();

            Console.WriteLine("Db initialize");
        }
        private void ClearDatabase()
        {
            _context.Subscriptions.RemoveRange(_context.Subscriptions);
            _context.Publications.RemoveRange(_context.Publications);

            _context.PublicationTypes.RemoveRange(_context.PublicationTypes);

            _context.SaveChanges();
        }

    

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

                for(int i = 0; i < 100; i++)
                {
                    var randomType = publicationTypes[_random.Next(publicationTypes.Count)];
                    var randomName = publicationNames[_random.Next(publicationNames.Count)];
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

        private void InitializeSubscriptions()
        {
            if (!_context.Subscriptions.Any())
            {
                var publications = _context.Publications.ToList();

                for(int i = 0; i < 100; i++)
                {
                    var randomPublication = publications[_random.Next(publications.Count)];
                    var duration = _random.Next(1, 13);
                    var startDate = $"{_random.Next(1, 13).ToString().PadLeft(2, '0')}.{_random.Next(2014, 2024)}";

                    _context.Subscriptions.Add(new Subscription
                    {
                        Publication = randomPublication,
                        Duration = duration,
                        SubscriptionStartDate = startDate
                    });
                }

                _context.SaveChanges();
            }
        }
    }
}
