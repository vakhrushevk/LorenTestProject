using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LorenTestProject
{
    class FromDateBase
    {
        // Создание бд, удаление если она уже созданна.
        public static void dbconnect()
        {
            if (!File.Exists("test.db"))
                SQLiteConnection.CreateFile("test.db");
            else
            {
                File.Delete("test.bd");
                SQLiteConnection.CreateFile("test.db");
            }
        }
        //запрос на выборку даннх discount
        public static Salons dbImportStruct(string name)
        {
            using (var connection = new SQLiteConnection("Data Source=test.db"))
            {
                connection.Open();
                importSalons import=null;
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT id,name,discount,discount_parent,parentid FROM AllSalon WHERE AllSalon.name = @name";
                    cmd.Parameters.AddWithValue("name", name);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read()) 
                        {
                            double s;
                            try
                            {
                                string discount = reader.GetString(2);
                                s = Convert.ToDouble(discount);
                                import = new importSalons(Convert.ToInt32(reader.GetValue(0)), reader.GetValue(1).ToString(), s, Convert.ToBoolean(reader.GetValue(3)), Convert.ToInt32(reader.GetValue(4)));

                            }
                            catch (Exception ex)
                            {
                                s = reader.GetDouble(2);
                                import = new importSalons(Convert.ToInt32(reader.GetValue(0)), reader.GetValue(1).ToString(), s, Convert.ToBoolean(reader.GetValue(3)), Convert.ToInt32(reader.GetValue(4)));

                            }
                        }
                        return import;
                    }
                    
                }
            }
        }
        // запрос на получения name по id из таблицы
        public static string dbImportId(int id)
        {
            using (var connection = new SQLiteConnection("Data Source=test.db"))
            {
                string name="";
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT name FROM AllSalon WHERE id = @id";
                    cmd.Parameters.AddWithValue("id", id);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            name = reader.GetString(0);
                        }
                        return name;
                    }
                }
            }
        }
        // Запрос к БД с выборкой всех записей из стаблицы AllSalon возвращает список
        public static List<Salons> dbimport()
        {
            List<Salons> import_salon = new List<Salons>();
            using (var connection = new SQLiteConnection("Data Source=test.db"))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "select * from AllSalon";
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            double s;
                            try
                            {
                                string discount = reader.GetString(2);
                                s = Convert.ToDouble(discount);
                            }
                            catch (Exception ex)
                            {
                                s = reader.GetDouble(2);
                            }
                            import_salon.Add(new Salons(reader.GetValue(1).ToString(), s, Convert.ToBoolean(reader.GetValue(3)), reader.GetValue(4).ToString(),reader.GetInt32(5)));

                        }
                    }
                }
            }
            return import_salon;
        }
        // Записывает в бд данные салонов. + создает таблицы 
        public static void dbexport()
        {
            using (var connection = new SQLiteConnection("Data Source=test.db"))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    //создание гл таблицы
                    cmd.CommandText = @"create table if not exists [AllSalon](
                                [id] integer not null primary key autoincrement,
                                [name] nvarchar not null,
                                [discount] real not null,
                                [discount_parent] bool not null,
                                [description] nvarchar(124) not null,
                                [parentid] integer null
                            );";
                    cmd.ExecuteNonQuery();
                    
                    foreach (Salons s in Salons.ListSalons())
                    {
                        cmd.CommandText = "insert into AllSalon (name,discount,discount_parent,description,parentid) values(@name,@discount,@discount_parent,@description,@parentid)";
                        cmd.Parameters.AddWithValue("name", $"{s.name}");
                        cmd.Parameters.AddWithValue("discount", $"{s.discount}");
                        cmd.Parameters.AddWithValue("discount_parent", $"{ s.discount_parent}");
                        cmd.Parameters.AddWithValue("description", $"{s.description}");
                        cmd.Parameters.AddWithValue("parentid", $"{s.parentid}");
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
