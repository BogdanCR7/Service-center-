using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exam.Areas.Identity.Data;
using Exam.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Exam.Data
{
    public class Context : IdentityDbContext<User>
    {
        public DbSet<Category> categories { get; set; }

        public DbSet<Good> goods { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<Client> clients { get; set; }
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
