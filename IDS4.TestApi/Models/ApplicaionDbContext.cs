using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDS4.TestApi.Models
{
    public class ApplicaionDbContext : DbContext
    {
        public ApplicaionDbContext(DbContextOptions<ApplicaionDbContext> options)
            :base(options)
        {

        }

        public virtual DbSet<Customer> Customers { get; set; }
    }
}
