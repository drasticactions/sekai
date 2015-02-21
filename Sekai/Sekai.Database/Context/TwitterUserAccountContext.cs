using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;
using Sekai.Database.Models;

namespace Sekai.Database.Context
{
    public class TwitterUserAccountContext : DbContext
    {
        public DbSet<TwitterUserAccount> TwitterAccounts { get; set; }

        protected override void OnConfiguring(DbContextOptions builder)
        {
            var connection = "Filename=AccountList2.db";
            builder.UseSQLite(connection);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TwitterUserAccount>(b =>
            {
                b.Key(thread => thread.Id);
            });
            builder.Model.GetEntityType(typeof(TwitterUserAccount)).GetProperty("Id").GenerateValueOnAdd = true;
        }
    }
}
