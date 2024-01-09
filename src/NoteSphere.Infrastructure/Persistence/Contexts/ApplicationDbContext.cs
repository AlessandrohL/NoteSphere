using Application.Identity;
using Domain.Entities;
using Infrastructure.Data.Configurations;
using Infrastructure.Identity;
using Infrastructure.Identity.Configuration;
using Infrastructure.Persistence.Seed;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<UserAuth>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new UserConfiguration());

            builder.ApplyConfiguration(new ApplicationUserConfiguration());
            builder.ApplyConfiguration(new NoteBookConfiguration());
            builder.ApplyConfiguration(new NoteConfiguration());
            builder.ApplyConfiguration(new TagConfiguration());
            builder.ApplyConfiguration(new NoteTagConfiguration());
            //builder.ApplyConfiguration(new TodoConfiguration());

            // Seed
            builder.ApplyConfiguration(new NotesSeed());
        }

        public DbSet<NoteBook> NoteBooks { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<NoteTag> NoteTags { get; set; }
        //public DbSet<Todo> Todos { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
