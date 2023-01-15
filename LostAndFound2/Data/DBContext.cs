using LostAndFound2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics;

namespace LostAndFound2.Data
{
    public class DBContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Item> Item { get; set; }
        private static DBContext? _Singleton;
        private DBContext(DbContextOptions o) : base(o) { }
        private static DBContext Instantiate_DBContext()
        {
            var optionBuilder = new DbContextOptionsBuilder<DBContext>();
            try
            {
                optionBuilder.UseSqlite("Data Source=C:\\Users\\firas\\Desktop\\lostandfound.db;");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("there is an exception " + ex.Message);
            }
            return new DBContext(optionBuilder.Options);
        }
        public static DBContext Instance
        {
            get
            {
                if (_Singleton == null)
                {
                    _Singleton = Instantiate_DBContext();
                }
                return _Singleton;
            }
        }
    }
}
