using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Diagnostics;

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
            try
            {
                string cs = "Data Source=Database.db";
                string stm = "SELECT SQLITE_VERSION()";

                Connection = new SQLiteConnection(cs);
                Connection.Open();

                var cmd = new SQLiteCommand(stm, Connection);
                string version = cmd.ExecuteScalar().ToString();

                Debug.WriteLine($"[localdatabase] version: {version}");

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[localdatabase] error: {ex}");
            }

            return false;
        }

        public static bool InitializeTables() =>
             CreateTableIfNotExist(AdministratorAccountsTable,
                 "email TEXT, " +
                 "fname TEXT, " +
                 "lname TEXT, " +
                 "address TEXT, " +
                 "password TEXT") &&
            CreateTableIfNotExist(MembersTable,
                "id INTEGER PRIMARY KEY," +
                "fname TEXT," +
                "lname TEXT," +
                "email TEXT," +
                "address TEXT," +
                "points INTEGER") &&
            CreateTableIfNotExist(TransactionsTable,
                "id INTEGER PRIMARY KEY," +
                "material INTEGER," +
                "member INTEGER," +
                "remarks TEXT," +
                "price REAL") &&
            CreateTableIfNotExist(MaterialsTable,
                "id INTEGER PRIMARY KEY," +
                "name TEXT," +
                "biodegradable INTEGER" /* bool: 0 = false, 1 = true */);

        public static bool CheckAdministrator(string email, string password)
        {
            try
            {
                var cmd = new SQLiteCommand(Connection);
                cmd.CommandText = $"SELECT count(*) FROM {AdministratorAccountsTable} WHERE email=@email AND password=@password";
                cmd.Parameters.AddWithValue("email", email);
                cmd.Parameters.AddWithValue("password", password);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count >= 1;

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[localdatabase] error: {ex}");
            }
            return false;
        }

        public static IEnumerable<Models.Member> SelectMembers()
        {

            List<Models.Member> members = new List<Models.Member>();

            SQLiteDataReader rdr = new SQLiteCommand($"SELECT * FROM {MembersTable}",
                Connection).ExecuteReader();

            while (rdr.Read())
                members.Add(new Models.Member
                {
                    ID = GetElseDefault<long>(rdr, "id"),
                    Address = GetElseDefault<string>(rdr, "address"),
                    Email = (string)rdr.GetValue(rdr.GetOrdinal("email")),
                    FirstName = (string)rdr.GetValue(rdr.GetOrdinal("fname")),
                    LastName = (string)rdr.GetValue(rdr.GetOrdinal("lname")),
                    Points = GetElseDefault<long>(rdr, "points"),
                });

            return members;
        }

        public static double GetTotalTransactions()
        {
            try
            {
                var cmd = new SQLiteCommand(Connection);
                cmd.CommandText = $"SELECT SUM(price) FROM {TransactionsTable}";

                return Convert.ToInt32(cmd.ExecuteScalar());

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[localdatabase] error: {ex}");
            }
            return 0;

        }

        public static long GetTableCount(string tableName)
        {
            try
            {
                var cmd = new SQLiteCommand(Connection);
                cmd.CommandText = $"SELECT count(*) FROM {tableName}";

                return Convert.ToInt32(cmd.ExecuteScalar());

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[localdatabase] error: {ex}");
            }
            return 0;
        }

        public static T GetElseDefault<T>(SQLiteDataReader reader, string name)
        {
            var objVal = reader.GetValue(reader.GetOrdinal(name));
            if (objVal is null)
                return default(T);
            try
            {
                return (T)objVal;
            }
            catch { }

            return default(T);
        }

        public static bool InsertTransaction(long materialId, long memberId, string remarks, string price)
        {
            try
            {
                var cmd = new SQLiteCommand(Connection);
                cmd.CommandText = $"INSERT INTO {TransactionsTable}(id, material, member, remarks, price) " +
                    $"VALUES(@id,@material,@member,@remarks,@price)";

                cmd.Parameters.AddWithValue("id", DbRandom.Next());
                cmd.Parameters.AddWithValue("material", materialId);
                cmd.Parameters.AddWithValue("member", memberId);
                cmd.Parameters.AddWithValue("remarks", remarks);
                cmd.Parameters.AddWithValue("price", price);

                // Start points at 0.
                cmd.Parameters.AddWithValue("points", 0);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[localdatabase] error: {ex}");
            }
            return false;
        }

        public static bool DeleteMember(long id)
        {
            try
            {
                var cmd = new SQLiteCommand(Connection);
                cmd.CommandText = $"DELETE FROM {MembersTable} WHERE id=@id";
                cmd.Parameters.AddWithValue("id", id);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[localdatabase] error: {ex}");
            }
            return false;
        }

        public static bool InsertMember(string address, string email, string fname, string lname, long id = -1)
        {
            try
            {
                var cmd = new SQLiteCommand(Connection);
                if (id == -1)
                    cmd.CommandText = $"INSERT OR REPLACE INTO {MembersTable}(id, fname, lname, email, address, points) VALUES(@id,@fname,@lname,@email,@address,@points)";
                else cmd.CommandText = $"UPDATE {MembersTable} SET fname=@fname, lname=@lname, email=@email, address=@address, points=@points WHERE id=@id";

                id = id == -1 ? DbRandom.Next() : id;
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("email", email);
                cmd.Parameters.AddWithValue("fname", fname);
                cmd.Parameters.AddWithValue("lname", lname);
                cmd.Parameters.AddWithValue("address", address);

                // Start points at 0.
                cmd.Parameters.AddWithValue("points", 0);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[localdatabase] error: {ex}");
            }
            return false;
        }

        public static bool InsertAdministrators(string email, string fname, string lname, string address, string password)
        {
            try
            {
                var cmd = new SQLiteCommand(Connection);
                cmd.CommandText = $"INSERT INTO {AdministratorAccountsTable}(email, fname, lname, address, password) VALUES(@email,@fname,@lname,@address,@password)";
                cmd.Parameters.AddWithValue("email", email);
                cmd.Parameters.AddWithValue("fname", fname);
                cmd.Parameters.AddWithValue("lname", lname);
                cmd.Parameters.AddWithValue("address", address);
                cmd.Parameters.AddWithValue("password", password);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
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

        public static bool CreateTableIfNotExist(string tableName, string typeAndNames) =>
            Execute($"CREATE TABLE IF NOT EXISTS {tableName}({typeAndNames})");


    }
}
