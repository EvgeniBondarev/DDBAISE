using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    internal class DbManager
    {
        public void SelectPublicationTypes()
        {
            Console.WriteLine("1. Выборка данных из таблицы на стороне отношения 'один'");
            Console.WriteLine(new string('-', 50));
            using (var dbContext = new SubsCityContext())
            {
                var publicationTypes = dbContext.PublicationTypes.ToList();

                foreach (var publicationType in publicationTypes)
                {
                    Console.WriteLine($"Id: {publicationType.Id}, Type: {publicationType.Type}");
                }
            }
            Console.WriteLine("\n\n");
        }
        public void SelectPublicationTypesByType(string targetType)
        {
            Console.WriteLine("2. Выборка данных из таблицы на стороне отношения 'один' с фильтрацией данных");
            Console.WriteLine(new string('-', 50));

            using (var dbContext = new SubsCityContext())
            {
                var filteredPublicationTypes = dbContext.PublicationTypes
                    .Where(pt => pt.Type == targetType)
                    .ToList();

                foreach (var publicationType in filteredPublicationTypes)
                {
                    Console.WriteLine($"Id: {publicationType.Id}, Type: {publicationType.Type}");
                }
            }
            Console.WriteLine("\n\n");
        }
        public void SelectTotalSubscriptions()
        {
            Console.WriteLine("3. Выборка данных из таблицы на стороне отношения 'многие' с агрегацией данных");
            Console.WriteLine(new string('-', 50));
            using (var dbContext = new SubsCityContext())
            {
  
                var groupedSubscriptions = dbContext.Subscriptions
                    .GroupBy(s => s.Duration)
                    .Select(g => new
                    {
                        Duration = g.Key,
                        TotalSubscriptions = g.Count()
                    })
                    .ToList();

                foreach (var group in groupedSubscriptions)
                {
                    Console.WriteLine($"Duration: {group.Duration}, Total Subscriptions: {group.TotalSubscriptions}");
                }
            }
            Console.WriteLine("\n\n");
        }
        public void SelectEmployee()
        {
            Console.WriteLine("4. Выборка данных из таблиц на стороне отношения 'один-многие' с join");
            Console.WriteLine(new string('-', 50));

            using (var dbContext = new SubsCityContext())
            {
                var query = dbContext.Offices
                    .Join(dbContext.Employees,
                        office => office.Id,
                        employee => employee.OfficeId,
                        (office, employee) => new
                        {
                            OfficeName = office.StreetName,
                            EmployeeName = $"{employee.Surname} {employee.Name} {employee.Middlename}"
                        });

                foreach (var result in query)
                {
                    Console.WriteLine($"OfficeName: {result.OfficeName}, EmployeeName: {result.EmployeeName}");
                }
            }
            Console.WriteLine("\n\n");
        }

        public void SelectEmployeeByOffice(string targetOfficeName)
        {
            Console.WriteLine("5. Выборка данных из таблиц на стороне отношения 'один-многие' с фильтрацией данных по определенному условию");
            Console.WriteLine(new string('-', 50));

            using (var dbContext = new SubsCityContext())
            {
                var query = dbContext.Offices
                    .Join(dbContext.Employees,
                        office => office.Id,
                        employee => employee.OfficeId,
                        (office, employee) => new { Office = office, Employee = employee })
                    .Where(joinResult => joinResult.Office.StreetName == targetOfficeName)
                    .Select(joinResult => new
                    {
                        EmployeeName = $"{joinResult.Employee.Surname} {joinResult.Employee.Name} {joinResult.Employee.Middlename}",
                        OfficeName = joinResult.Office.StreetName
                    });

                foreach (var result in query)
                {
                    Console.WriteLine($"OfficeName: {result.OfficeName}, EmployeeName: {result.EmployeeName}");
                }
            }
            Console.WriteLine("\n\n");
        }

        public void InsertPublicationType(PublicationType newPublicationType)
        {
            Console.WriteLine("6. Вставку данных в таблицу, стоящей на стороне отношения 'Один'");
            using (var dbContext = new SubsCityContext())
            {
       


                dbContext.PublicationTypes.Add(newPublicationType);

                dbContext.SaveChanges();

                Console.WriteLine($"Запись '{newPublicationType.Type}' с Id = {newPublicationType.Id} успешно добавлена в таблицу PublicationType.");
            }
            Console.WriteLine("\n\n");
        }
        public void InsertEmployee(Employee newEmployee)
        {
            Console.WriteLine("7. Вставку данных в таблицу, стоящей на стороне отношения 'Многие'");
            using (var dbContext = new SubsCityContext())
            {
 
                dbContext.Employees.Add(newEmployee);

                dbContext.SaveChanges();

                Console.WriteLine($"Запись '{newEmployee.Surname} {newEmployee.Name} {newEmployee.Middlename}' c Id = {newEmployee.Id} успешно добавлена в таблицу Employee.");
            }
            Console.WriteLine("\n\n");
        }
        public void DeletePublicationType(PublicationType publicationTypeToDelete)
        {
            Console.WriteLine("8. Удаление данных из таблицы, стоящей на стороне отношения 'Один'");
            using (var dbContext = new SubsCityContext())
            {

                var existingPublicationType = dbContext.PublicationTypes
            .FirstOrDefault(pt => pt.Id == publicationTypeToDelete.Id && pt.Type == publicationTypeToDelete.Type);

                if (existingPublicationType != null)
                {

                    dbContext.PublicationTypes.Remove(existingPublicationType);
                    dbContext.SaveChanges();

                    Console.WriteLine($"Запись '{publicationTypeToDelete.Type}' c Id = {publicationTypeToDelete.Id} успешно удалена.");
                }
            }
            Console.WriteLine("\n\n");
        }
        public void DeleteEmployees(Employee employeeToDelete)
        {
            Console.WriteLine("9. Удаление данных из таблицы, стоящей на стороне отношения 'Многие'");
            using (var dbContext = new SubsCityContext())
            {
                var existingEmployee = dbContext.Employees
                    .FirstOrDefault(e => e.Id == employeeToDelete.Id &&
                                         e.Name == employeeToDelete.Name &&
                                         e.Middlename == employeeToDelete.Middlename);

                        if (existingEmployee != null)
                        {
     
                            dbContext.Employees.Remove(existingEmployee);
                            dbContext.SaveChanges();

                            Console.WriteLine($"Запись '{existingEmployee.Surname} {existingEmployee.Name} {existingEmployee.Middlename}' c Id = {existingEmployee.Id} успешно удалена.");
                        }
            }
            Console.WriteLine("\n\n");
        }
        public void UpdatePublicationTypes(string condition, string newType)
        {
            Console.WriteLine("10. Обновления данных в таблице");
            using (var dbContext = new SubsCityContext())
            {
                var publicationTypesToUpdate = dbContext.PublicationTypes
                            .Where(pt => pt.Type == condition);

                foreach (var publicationType in publicationTypesToUpdate)
                {
                    publicationType.Type = newType;
                }

                dbContext.SaveChanges();

                Console.WriteLine("Записи успешно обновлены в таблице PublicationType.");
            }

            Console.WriteLine("\n\n");
        }
    }
}














