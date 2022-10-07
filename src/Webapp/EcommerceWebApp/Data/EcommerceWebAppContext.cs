using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EcommerceWebApp.Models;

namespace EcommerceWebApp.Data
{
    public class EcommerceWebAppContext : DbContext
    {
        public EcommerceWebAppContext (DbContextOptions<EcommerceWebAppContext> options)
            : base(options)
        {
        }

        public DbSet<EcommerceWebApp.Models.Catalog> Catalog { get; set; }
    }
}
