using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoItemsApp.Models
{
    public class IndexViewModel
    {
        public List<Item> IncompleteItems { get; set; }
        public List<Category> Categories { get; set; }
    }
    public class AddItemViewModel
    {
        public List<Category> Categories { get; set; }
    }
    public class CompletedItemsViewModel
    {
        public List<Item> CompletedItems { get; set; }
        public List<Category> Categories { get; set; }
    }
    public class CategoriesViewModel
    {
        public List<Category> Categories { get; set; }
    }
    public class EditCategoryViewModel
    {
        public Category Category { get; set; }
    }
    public class GetByCategoryViewModel
    {
        public List<Item> ItemsByCategory { get; set; }
        public Category Category { get; set; }
    }
    public class Item
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime DateDue { get; set; }
        public DateTime? DateCompeted { get; set; }
        public int CategoryId { get; set; }
    }
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class ToDoItemsDb
    {
        private readonly string _connectionString;
        public ToDoItemsDb(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<Item> GetIncompleteItems()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM Items i WHERE i.DateCompleted IS NULL";
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                List<Item> items = new List<Item>();
                while (reader.Read())
                {
                    items.Add(new Item
                    {
                        Id = (int)reader["Id"],
                        Title = (string)reader["Title"],
                        DateDue = (DateTime)reader["DateDue"],
                        CategoryId = (int)reader["CategoryId"]
                    });
                }
                return items;
            }
        }
        public List<Category> GetCategories()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM Categories c";
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                List<Category> categories = new List<Category>();
                while (reader.Read())
                {
                    categories.Add(new Category
                    {
                        Id = (int)reader["Id"],
                        Name = (string)reader["Name"]
                    });
                }
                return categories;
            }
        }
        public void AddCategory(Category c)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO Categories VALUES (@name)";
                cmd.Parameters.AddWithValue("@name", c.Name);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void AddItem(Item i)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO Items (Title, CategoryId, DateDue) VALUES (@title, @categoryId, @dateDue)";
                cmd.Parameters.AddWithValue("@title", i.Title);
                cmd.Parameters.AddWithValue("@categoryId", i.CategoryId);
                cmd.Parameters.AddWithValue("@dateDue", i.DateDue);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void MarkItemAsCompleted(int id, DateTime dateCompleted)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "UPDATE Items SET DateCompleted = @dateCompleted WHERE Id = @id";
                cmd.Parameters.AddWithValue("@dateCompleted", dateCompleted);
                cmd.Parameters.AddWithValue("@Id", id);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public List<Item> GetCompletedItems()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM Items i WHERE i.DateCompleted IS NOT NULL";
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                List<Item> items = new List<Item>();
                while (reader.Read())
                {
                    items.Add(new Item
                    {
                        Id = (int)reader["Id"],
                        Title = (string)reader["Title"],
                        DateDue = (DateTime)reader["DateDue"],
                        DateCompeted = (DateTime)reader["DateCompleted"],
                        CategoryId = (int)reader["CategoryId"]
                    });
                }
                return items;
            }
        }
        public Category GetCategoryById(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM Categories c WHERE c.Id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                Category category = new Category();
                while (reader.Read())
                {
                    category.Name = (string)reader["Name"];
                    category.Id = (int)reader["Id"];
                }
                return category;
            }
        }
        public void EditCategory(Category c)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "UPDATE Categories SET Name = @name WHERE Id = @id";
                cmd.Parameters.AddWithValue("@name", c.Name);
                cmd.Parameters.AddWithValue("@id", c.Id);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public List<Item> GetItemsByCategory(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM Items i WHERE i.CategoryId = @id";
                cmd.Parameters.AddWithValue("@id", id);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                List<Item> items = new List<Item>();
                while (reader.Read())
                {
                    items.Add(new Item
                    {
                        Id = (int)reader["Id"],
                        Title = (string)reader["Title"],
                        DateDue = (DateTime)reader["DateDue"],
                        DateCompeted = reader.GetOrNull<DateTime?>("DateCompleted"),
                        CategoryId = (int)reader["CategoryId"]
                    });
                }
                return items;
            }
        }
    }
    public static class Extensions
    {
        public static T GetOrNull<T>(this SqlDataReader reader, string column)
        {
            object value = reader[column];
            if (value == DBNull.Value)
            {
                return default(T);
            }
            return (T)value;
        }
    }
}
