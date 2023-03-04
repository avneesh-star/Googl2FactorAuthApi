using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using google2fa.API.Models;
using Microsoft.EntityFrameworkCore;

namespace google2fa.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }


        public DbSet<UserMaster> UserMasters { get; set; }


    }
}