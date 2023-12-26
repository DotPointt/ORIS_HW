using System;
using System.ComponentModel;
using System.Data.SqlClient;



namespace MyORM
{
    public class MyDataContext
    {
        SqlConnection connection;
        public MyDataContext(SqlConnection connection)
        {
            this.connection = connection;
        }

        public List<T> Select<T>() where T : class, new()
        {
            var command = connection.CreateCommand();
            var list = new List<T>();

            string TableName = typeof(T).Name;
            command.CommandText = $"SELECT * FROM  {typeof(T).Name}";
            Console.WriteLine(command.CommandText);
            SqlDataReader reader = command.ExecuteReader();
            //SqlParameter TableNameParam = new SqlParameter("@TableName", typeof(T).Name);


            if (reader.HasRows)
            {
                var props = typeof(T).GetProperties();

                while (reader.Read()) // построчно считываем данные
                {
                    T instance = new T();

                    foreach (var property in props)
                    {
                        property.SetValue(instance, reader.GetValue(reader.GetOrdinal(property.Name)) == DBNull.Value ? null : reader.GetValue(reader.GetOrdinal(property.Name)));
                        //Console.WriteLine(reader.GetValue(reader.GetOrdinal(property.Name)));
                    }

                    list.Add(instance);
                }

                foreach (var obj in list)
                {
                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        Console.WriteLine(prop.GetValue(obj));
                    }
                }
            }

            reader.Close();

            return list;
        }

        public bool Add<T>(T entity)
        {
            //INSERT INTO Customers (city, cname, cnum) VALUES (‘London’, 'Hoffman', 2001);
            // "INSERT INTO typeof(T).Name (" + props.join(", ") +") VALUES ("

            var type = typeof(T);
            var tableName = type.Name; // Имя таблицы предполагается как имя класса
            var properties = type.GetProperties();
            var columnNames = string.Join(", ", properties.Select(p => p.Name));
            var values = string.Join(", ", properties.Select(p => $"'{p.GetValue(entity)}'"));
            var query = $"SET IDENTITY_INSERT {tableName} ON; INSERT INTO {tableName} ({columnNames}) VALUES ({values});";
            Console.WriteLine(query);
            SqlCommand cmd = new SqlCommand(query, connection);
            try
            {
                int result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                connection.Close();
                return false;
            }


            return true;
        }

        public bool Delete<T>(int id)
        {
            //DELETE FROM <имя таблицы >  [WHERE<предикат>] ;



            var command = connection.CreateCommand();

            command.CommandText = $"DELETE FROM {typeof(T).Name} WHERE id = {id}";

            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                connection.Close();
                return false;
            }

            return true;
        }


        public T SelectById<T>(int id) where T : class, new()
        {
            var command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM {typeof(T).Name} WHERE id = {id}";
            T instance = new T();

            var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();



                var props = typeof(T).GetProperties();

                foreach (var property in props)
                {
                    property.SetValue(instance, reader.GetValue(reader.GetOrdinal(property.Name)) == DBNull.Value ? null : reader.GetValue(reader.GetOrdinal(property.Name)));
                    //Console.WriteLine(reader.GetValue(reader.GetOrdinal(property.Name)));
                }
            }

            return instance;
        }

        public bool Update<T>(T entity, int id)
        {   //        bool Update<T>(T);//можно сделать int id,T t
            //"UPDATE {typeof(T).Name} SET    WHERE id = {id}"

            var allProperties = typeof(T).GetProperties();
            var properties = allProperties.Where(p => p.Name != "id");
            //var columnNames = string.Join(", ", properties.Select(p => p.Name));
            //var values = string.Join(", ", properties.Select(p => $"'{p.GetValue(entity)}'"));

            var query = $"UPDATE {typeof(T).Name} SET ";

            List<string> pairStr = new List<string>();

            foreach (var prop in properties)
            {
                pairStr.Add($"{prop.Name} = {prop.GetValue(entity).ToString().Replace(" ", "_")}");
            }

            query += $"{string.Join(", ", pairStr)} WHERE id = {id};";

            Console.WriteLine($"ВАШ ЗАПРОС: {query}");

            return true;
        }
    }
}