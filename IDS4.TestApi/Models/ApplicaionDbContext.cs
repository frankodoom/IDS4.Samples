using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDS4.WebApi.Models
{
    public class ApplicaionDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicaionDbContext(DbContextOptions<ApplicaionDbContext> options)
            :base(options)
        {

        }

        public virtual DbSet<Customer> Customers { get; set; }
    }
}
