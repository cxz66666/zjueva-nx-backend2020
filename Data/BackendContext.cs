using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using _2020_backend.Models;
using Org.BouncyCastle.Utilities;

namespace _2020_backend.Data
{
    public class BackendContext : DbContext
    {
        public BackendContext (DbContextOptions<BackendContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }

        public DbSet<Record> Record { get; set; }
        public DbSet<Notes> Note { get; set; }
        public DbSet<InterviewTime> Time { get; set; }

        public DbSet<SMS> Sms { get; set; }
        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Record>()
                .HasKey(r => r.rid);
            mb.Entity<User>()
                .HasKey(u => u.Uid);
            mb.Entity<Notes>()
                .HasKey(u => u.ID);
            mb.Entity<InterviewTime>()
                .HasKey(u => u.ID);
            mb.Entity<SMS>()
                .HasKey(u => u.ID);
        }
    }
}
