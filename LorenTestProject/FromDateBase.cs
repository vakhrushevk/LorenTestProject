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
        public static Salons dbImportId(string name)
        {
            using (var connection = new SQLiteConnection("Data Source=test.db"))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT AllSalon.id, AllSalon.name,AllSalon.discount,parent_table.parentid FROM AllSalon,parent_table WHERE AllSalon.id=parent_table.idsalon AND ALLSalon.name=@name";
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
                               
                            }
                            catch (Exception ex)
                            {
                                s = reader.GetDouble(2);
                                return new Salons(reader.GetValue(1).ToString(), s, Convert.ToBoolean(reader.GetValue(3)), reader.GetValue(4).ToString());

                            }
                        }
                    }
                    return null;
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
                             import_salon.Add(new Salons(reader.GetValue(1).ToString(), s, Convert.ToBoolean(reader.GetValue(3)), reader.GetValue(4).ToString()));
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
                                [description] nvarchar(124) not null
                            );";
                    cmd.ExecuteNonQuery();
                    // Создание таблицы для связей
                    cmd.CommandText = @"create table if not exists [parent_table](
                                [id] integer not null primary key autoincrement,
                                [idsalon] integer not null,
                                [parentid] integer
                            );";
                    cmd.ExecuteNonQuery();
                    foreach (Salons s in Salons.ListSalons())
                    {
                        cmd.CommandText = "insert into AllSalon (name,discount,discount_parent,description) values(@name,@discount,@discount_parent,@description)";
                        cmd.Parameters.AddWithValue("name", $"{s.name}");
                        cmd.Parameters.AddWithValue("discount", $"{s.discount}");
                        cmd.Parameters.AddWithValue("discount_parent", $"{ s.discount_parent}");
                        cmd.Parameters.AddWithValue("description", $"{s.description}");
                        cmd.ExecuteNonQuery();

                    }

                    var sl = Salons.ListSalons();
                    for (int i = 0; i < sl.Count; i++)
                    {
                        int list_iterator = i+1;
                        cmd.CommandText = "insert into parent_table (idsalon,parentid) values(@idsalon,@parentid)";

                        if (sl[i].name == "Амелия")
                        {
                            cmd.Parameters.AddWithValue("idsalon", list_iterator);
                            cmd.Parameters.AddWithValue("parentid", 1);
                        }
                        else if (sl[i].name == "Тест2")
                        {
                            cmd.Parameters.AddWithValue("idsalon", list_iterator);
                            cmd.Parameters.AddWithValue("parentid", 2);
                        }
                        else if (sl[i].name == "Тест1")
                        {
                            cmd.Parameters.AddWithValue("idsalon", list_iterator);
                            cmd.Parameters.AddWithValue("parentid", 3);
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "insert into parent_table (idsalon,parentid) values(@idsalon,@parentid)";
                            cmd.Parameters.AddWithValue("idsalon", list_iterator);
                            cmd.Parameters.AddWithValue("parentid", 4);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("idsalon", list_iterator);
                            cmd.Parameters.AddWithValue("parentid", null);
                        }
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
