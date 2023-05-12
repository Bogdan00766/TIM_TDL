using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIM_TDL.Domain.Models;

namespace TIM_TDL.Infrastructure
{
    public class TDLDbContext : DbContext
    {
        public TDLDbContext(DbContextOptions<TDLDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

    }
}
