using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using a2.Models;

namespace a2.Data{
    public class A2DbContext : DbContext{
        public A2DbContext(DbContextOptions<A2DbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Sign> Signs {get; set;}
        public DbSet<Organizer> Organizers {get; set;}
        public DbSet<Event> Events {get; set;}
    }
}
