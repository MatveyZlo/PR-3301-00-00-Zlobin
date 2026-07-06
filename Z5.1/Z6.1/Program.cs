using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

class Program
{
    static string connectionString = "";

    static void Main()
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        connectionString = config.GetConnectionString("DefaultConnection")
            ?? throw new Exception("Connection string 'DefaultConnection' not found");

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("1. Добавить рецепт");
            Console.WriteLine("2. Показать рецепты");
            Console.WriteLine("3. Изменить рейтинг");
            Console.WriteLine("4. Удалить рецепт");
            Console.WriteLine("0. Выход");

            Console.Write("Выбор: ");
            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    CreateRecipe();
                    break;

                case "2":
                    ReadRecipes();
                    break;

                case "3":
                    UpdateRecipe();
                    break;

                case "4":
                    DeleteRecipe();
                    break;

                case "0":
                    return;
            }
        }
    }

    static void CreateRecipe()
    {
        Console.Write("Название: ");
        string title = Console.ReadLine() ?? "";

        Console.Write("Рейтинг: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal rating))
        {
            Console.WriteLine("Ошибка: неверный рейтинг");
            return;
        }

        using SqlConnection connection = new SqlConnection(connectionString);
        connection.Open();

        string sql = @"
INSERT INTO Recipes (Title, Rating, IsVegetarian, AuthorID, CategoryID)
VALUES (@title, @rating, 1, 1, 1)";

        using SqlCommand command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@title", title);
        command.Parameters.AddWithValue("@rating", rating);

        command.ExecuteNonQuery();

        Console.WriteLine("Рецепт добавлен.");
    }

    static void ReadRecipes()
    {
        using SqlConnection connection = new SqlConnection(connectionString);
        connection.Open();

        using SqlCommand command = new SqlCommand(
            "SELECT Title, Rating FROM Recipes",
            connection);

        using SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            Console.WriteLine($"{reader["Title"]} - {reader["Rating"]}");
        }
    }

    static void UpdateRecipe()
    {
        Console.Write("Название рецепта: ");
        string title = Console.ReadLine() ?? "";

        Console.Write("Новый рейтинг: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal rating))
        {
            Console.WriteLine("Ошибка: неверный рейтинг");
            return;
        }

        using SqlConnection connection = new SqlConnection(connectionString);
        connection.Open();

        using SqlCommand command = new SqlCommand(
            "UPDATE Recipes SET Rating=@rating WHERE Title=@title",
            connection);

        command.Parameters.AddWithValue("@rating", rating);
        command.Parameters.AddWithValue("@title", title);

        command.ExecuteNonQuery();

        Console.WriteLine("Обновлено.");
    }

    static void DeleteRecipe()
    {
        Console.Write("Название рецепта: ");
        string title = Console.ReadLine() ?? "";

        using SqlConnection connection = new SqlConnection(connectionString);
        connection.Open();

        using SqlCommand command = new SqlCommand(
            "DELETE FROM Recipes WHERE Title=@title",
            connection);

        command.Parameters.AddWithValue("@title", title);

        command.ExecuteNonQuery();

        Console.WriteLine("Удалено.");
    }
}