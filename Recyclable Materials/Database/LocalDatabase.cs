using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using Recyclable_Materials.Models;

namespace Recyclable_Materials.Database
{
    /// <summary>
    /// A static class for handling SQLite.
    /// </summary>
    public static class LocalDatabase
    {
        private static Random DbRandom = new Random();

        /// <summary>
        /// Table consisting of administrator accounts that can manage all other tables.
        /// </summary>
        public const string AdministratorAccountsTable = "Administrators";

        /// <summary>
        /// Table consisting of members.
        /// </summary>
        public const string MembersTable = "Members";

        /// <summary>
        /// Table consisting of all transactions based on members via their ID.
        /// </summary>
        public const string TransactionsTable = "Transactions";

        public const string MaterialsTable = "Materials";

        /// <summary>
        /// The main SQLite connection object.
        /// </summary>
        private static SQLiteConnection Connection;


        public static bool Connect()
        {
            if (!Directory.Exists("db"))
                Directory.CreateDirectory("db");
            return true;
        }

        public static bool InitializeTables() =>
            CreateTableIfNotExist<AdministratorModel>() &
            CreateTableIfNotExist<Material>() &
            CreateTableIfNotExist<Models.Member>() &
            CreateTableIfNotExist<Transaction>();

        public static bool CheckAdministrator(string email, string password)
        {
            var administrations = ReadTable<AdministratorModel>();

            return !(administrations.FirstOrDefault(x =>
                x.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase) &&
                x.Password == password) is null);
        }

        public static IEnumerable<Models.Member> SelectMembers()
        {
            return ReadTable<Models.Member>();
        }

        public static double GetTotalTransactions()
        {
            return ReadTable<Transaction>().Sum(x => x.Price);
        }

        public static long GetTableCount<T>()
        {
            return ReadTable<T>().Count;
        }

        public static bool InsertTransaction(long materialId, long memberId, string remarks, string price)
        {
            try
            {
                var currentTable = ReadTable<Transaction>();

                currentTable.Add(new Transaction()
                {
                    ID = currentTable.Count, Material = materialId, Member = memberId, Remarks = remarks, Price = double.Parse(price)
                });
                WriteTable(currentTable);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[localdatabase] error: {ex}");
            }

            return false;
        }

       
        public static bool Execute(string command)
        {
            try
            {
                var cmd = new SQLiteCommand(Connection);
                cmd.CommandText = command;
                cmd.ExecuteNonQuery();

                Debug.WriteLine($"[localdatabase] executed: {command}");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[localdatabase] error: {ex}");
            }

            return false;
        }

        public static bool CreateTableIfNotExist<T>()
        {
            var tType = typeof(List<T>);
            string tableName = typeof(T).FullName;
            var databaseName = $"db/{tableName}.xml";

            if (File.Exists(databaseName)) return true;

            var serializer = new XmlSerializer(tType);
            using (var writer = XmlWriter.Create(databaseName, new XmlWriterSettings()
                   {
                       Encoding = new UTF8Encoding(false), Indent = true
                   }))
            {
                serializer.Serialize(writer, new List<T>());
            }

            return true;
        }

        public static bool WriteTable<T>(IEnumerable<T> contents)
        {
            var tType = typeof(List<T>);
            string tableName = typeof(T).FullName;
            var databaseName = $"db/{tableName}.xml";

            var serializer = new XmlSerializer(tType);
            StringBuilder result = new StringBuilder();
            using (var writer = XmlWriter.Create(result, new XmlWriterSettings()
                   {
                       Encoding = new UTF8Encoding(false), Indent = true
                   }))
            {
                serializer.Serialize(writer, contents);
            }

            File.WriteAllText(databaseName, result.Replace("utf-16", "utf-8").ToString());

            return true;
        }

        public static List<T> ReadTable<T>()
        {
            var tType = typeof(List<T>);
            string tableName = typeof(T).FullName;
            var databaseName = $"db/{tableName}.xml";
            var serializer = new XmlSerializer(tType);
            using (var reader = XmlReader.Create(databaseName, new XmlReaderSettings()
                   {
                   }))
            {
                return (List<T>)serializer.Deserialize(reader);
            }
        }
    }
}