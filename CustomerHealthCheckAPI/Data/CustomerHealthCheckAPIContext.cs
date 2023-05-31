using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CustomerHealthCheckAPI.Models;

namespace CustomerHealthCheckAPI.Data
{
    public class CustomerHealthCheckAPIContext : DbContext
    {
        public CustomerHealthCheckAPIContext (DbContextOptions<CustomerHealthCheckAPIContext> options)
            : base(options)
        {
        }

        public DbSet<CustomerHealthCheckAPI.Models.Customer> Customer { get; set; } = default!;
    }
}
