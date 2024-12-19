using PP_Teplokor_IPsp121.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace PP_Teplokor_IPsp121.Helper
{
    public class MyDbContext : DbContext
    {
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<CategoryError> CategoryError { get; set; }
        public DbSet<ApplicationDetails> ApplicationDetails { get; set; }
        public DbSet<Details> Details { get; set; }
        public DbSet<ApplicationDiagnostics> ApplicationDiagnostics { get; set; }
        public DbSet<EmployeeRole> EmployeeRole { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Distribution> Distribution { get; set; }
        public DbSet<Archive> Archive { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=C:\\Users\\user\\Desktop\\PP_Teplokor_IPsp121\\PP_Teplokor_IPsp121\\BD\\PP_TeploKor.db");
        }
        public void CreateDatabase()
        {
            try
            {
                this.Database.EnsureCreated();
                Console.WriteLine("База данных успешно создана.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при создании базы данных: " + ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
        public static List<T> GetEntities<T>(string connectionString, string sqlQuery) where T : new()
        {
            var entities = new List<T>();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = new SqliteCommand(sqlQuery, connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var entity = new T();
                        var properties = typeof(T).GetProperties();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var property = properties.FirstOrDefault(p => p.Name.Equals(reader.GetName(i), StringComparison.OrdinalIgnoreCase));
                            if (property != null && !reader.IsDBNull(i))
                            {
                                if (property.PropertyType == typeof(TimeSpan))
                                {
                                    var timeSpanString = reader.GetString(i);
                                    TimeSpan timeSpanValue;
                                    if (TimeSpan.TryParse(timeSpanString, out timeSpanValue))
                                    {
                                        property.SetValue(entity, timeSpanValue);
                                    }
                                    else
                                    {
                                        throw new FormatException($"Не удалось преобразовать '{timeSpanString}' в TimeSpan для свойства '{property.Name}'.");
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        var value = reader.GetValue(i);
                                        var convertedValue = Convert.ChangeType(value, property.PropertyType);
                                        property.SetValue(entity, convertedValue);
                                    }
                                    catch (FormatException ex)
                                    {
                                        if (property.PropertyType == typeof(decimal))
                                        {
                                            decimal decimalValue;
                                            if (decimal.TryParse(reader.GetString(i), NumberStyles.Any, CultureInfo.InvariantCulture, out decimalValue))
                                            {
                                                property.SetValue(entity, decimalValue);
                                            }
                                            else
                                            {
                                                throw new FormatException($"Ошибка преобразования decimal: Не удалось преобразовать '{reader.GetString(i)}' в decimal для свойства '{property.Name}'.");
                                            }
                                        }
                                        else
                                        {
                                            throw new FormatException($"Не удалось преобразовать значение для свойства '{property.Name}'. Тип данных: {property.PropertyType}.", ex);
                                        }
                                    }
                                }
                            }
                        }
                        entities.Add(entity);
                    }
                }
            }
            return entities;
        }
        public void DeleteEntityFromDatabase<T>(T entity) where T : class
        {
            using (var context = new MyDbContext())
            {
                var entry = context.Entry(entity);
                if (entry.State == EntityState.Detached)
                {
                    context.Attach(entity);
                }
                context.Remove(entity);
                context.SaveChanges();
            }
        }
        public void SaveEntity<T>(T entity) where T : class
        {
            using (var context = new MyDbContext())
            {
                context.Set<T>().Add(entity);
                context.SaveChanges();
            }
        }

        public void SaveEntities<T>(ObservableCollection<T> entities) where T : class
        {
            using (MyDbContext dbContext = new MyDbContext())
            {
                try
                {
                    dbContext.AddRange(entities);
                    dbContext.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    var innerException = ex.InnerException;
                    while (innerException != null)
                    {
                        Console.WriteLine(innerException.Message);
                        innerException = innerException.InnerException;
                    }
                }
            }
        }
        public void UpdateEntity<T>(T entity) where T : class
        {
            using (var context = new MyDbContext())
            {
                context.Set<T>().Update(entity);
                context.SaveChanges();
            }
        }
    }
}