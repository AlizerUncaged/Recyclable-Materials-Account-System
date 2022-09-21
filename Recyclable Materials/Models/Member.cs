using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recyclable_Materials.Database;

namespace Recyclable_Materials.Models
{
    public class Member : IModel<Member> , ICrudService<Member>
    {
        /// <summary>
        /// Server generated member ID, for security purposes it's randomly generated.
        /// </summary>
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public long Points { get; set; }

        public IEnumerable<Member> Search(IEnumerable<Member> searchList, string query) =>
            searchList.Where(x => $"{x.Id}".ToLower().Contains("query") ||
                x.FirstName.ToLower().Contains(query.ToLower()) ||
                x.LastName.ToLower().Contains(query.ToLower()) ||
                x.Email.ToLower().Contains(query.ToLower()) ||
                x.Address.ToLower().Contains(query.ToLower()) ||
                $"{x.Points}".ToLower().Contains(query.ToLower()));


      
        public void Create() =>
            LocalDatabase.CreateTableIfNotExist<Member>();


        public IEnumerable<Member> Read() =>
            LocalDatabase.ReadTable<Member>();

        public void Update(IEnumerable<Member> contents)
        {
            var currentTable = LocalDatabase.ReadTable<Member>();
            foreach (var content in contents)
            {
                currentTable.RemoveAll(x => x.Id == content.Id);
                content.Id = currentTable.Count;
                currentTable.Add(content);
            }

            LocalDatabase.WriteTable(currentTable);
        }

        public void Delete(Member obj)
        {
            var currentTable = LocalDatabase.ReadTable<Member>();
            currentTable.RemoveAll(x => x.Id == obj.Id);

            LocalDatabase.WriteTable(currentTable);
        }
    }
}
