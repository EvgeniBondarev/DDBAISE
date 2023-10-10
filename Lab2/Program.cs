namespace Lab2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DbManager dbManager = new DbManager();

            dbManager.SelectPublicationTypes();
            dbManager.SelectPublicationTypesByType("Газета");
            dbManager.SelectTotalSubscriptions();
            dbManager.SelectEmployee();
            dbManager.SelectEmployeeByOffice("Космическая улица");

            PublicationType publication = new PublicationType()
            {
                Type = "Учебное издание"
            };
            dbManager.InsertPublicationType(publication);

            Employee employee = new Employee()
            {
                Name = "Иван",
                Middlename = "Иванович",
                Surname = "Иванов",
                OfficeId = 1,
                PositionId = 1
            };
            dbManager.InsertEmployee(employee);
            dbManager.DeletePublicationType(publication);
            dbManager.DeleteEmployees(employee);
            dbManager.UpdatePublicationTypes("Учебное издание", "Новое издание");
        }
    }
}