using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Zarinpal.Data.Entities;

namespace Zarinpal.Data.Context
{
    public class Zarinpal_Db_Context:DbContext
    {
        public Zarinpal_Db_Context(DbContextOptions<Zarinpal_Db_Context> options) : base(options)
        {
            
        }

        public DbSet<Payment> Payments { get; set; }

    }
}
