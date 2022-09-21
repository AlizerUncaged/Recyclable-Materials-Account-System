using System.Collections.Generic;
using Recyclable_Materials.Database;

namespace Recyclable_Materials.Models
{
    public class AdministratorModel : ICrudService<AdministratorModel>
    {
        public int Id { get; set; }
        public string Email { get; set; }

        public string FirstName { get; set; }

        // Fully implemented property.
        private string _lastName;

        public string LastName
        {
            get => _lastName;
            set => _lastName = value;
        }

        public string Address { get; set; }

        public string Password { get; set; }


        public void Create() =>
            LocalDatabase.CreateTableIfNotExist<AdministratorModel>();


        public IEnumerable<AdministratorModel> Read() =>
            LocalDatabase.ReadTable<AdministratorModel>();

        public void Update(IEnumerable<AdministratorModel> contents)
        {
            var currentTable = LocalDatabase.ReadTable<AdministratorModel>();
            foreach (var content in contents)
            {
                currentTable.RemoveAll(x => x.Id == content.Id);
                currentTable.Add(new AdministratorModel()
                {
                    Id = currentTable.Count,
                    Email = content.Email, FirstName = content.FirstName, LastName = content.LastName,
                    Address = content.Address, Password = content.Password
                });
            }

            LocalDatabase.WriteTable(currentTable);
        }

        public void Delete(AdministratorModel obj)
        {
            var currentTable = LocalDatabase.ReadTable<AdministratorModel>();
            currentTable.RemoveAll(x => x.Id == obj.Id);

            LocalDatabase.WriteTable(currentTable);
        }
    }
}