using System;
using System.Data.SqlClient;
using MyORM;


string connectionString = @"Data Source=LAPTOP-3ICN49AT\SQLEXPRESS;Initial Catalog=StripClubdb;Integrated Security=True";

using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();

    var ORM = new MyDataContext(connection);

    //ORM.Select<Client>();
    //ORM.Delete<Client>(6);
    Console.WriteLine("--------------------------------------------------");
    //ORM.Select<Client>();
    //var entity = new Client()
    //{
    //    //id = 80,
    //    status = 2,
    //    fio = "John Kovalsky",
    //    age = 76,
    //    contacts = "896984577635",
    //    IsAnonym = false,
    //    IsBlocked = true
    //};
    //ORM.Add(entity);
    //ORM.SelectById<Client>(12);

    var entity = new Client()
    {
        status = 2,
        fio = "'New Human'",
        age = 34,
        contacts = "89613412111",
        IsAnonym = false,
        IsBlocked = false
    };
    ORM.Update(entity, 27);


    connection.Close();

}




class Client
{
    public int id { get; set; }
    public int status { get; set; }
    public string fio { get; set; }
    public int age { get; set; }
    public string contacts { get; set; }
    public bool IsBlocked { get; set; }
    public bool IsAnonym { get; set; }
}






//static class QueryMapper
//{
//    public static async Task<List<Client>> QueryAsync(this NpgsqlConnection connection, string sql, CancellationToken cancellationToken)
//    {
//        await using var command = connection.CreateCommand();
//        command.CommandText = sql;
//        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

//        var list = new List<Client>();
//        while (await reader.ReadAsync(cancellationToken))
//        {
//            list.Add(new Client()
//            {
//                fio = reader.GetString(reader.GetOrdinal("fio")),
//                id = reader.GetInt32(reader.GetOrdinal("id"))
//            });
//        }
//        return list;
//    }
//}