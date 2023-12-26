using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("ApplicationUsers");

            builder.HasKey(p => p.Id);

            builder
                .HasIndex(p => p.IdentityGuid)
                .IsUnique();

            builder.Property(p => p.IdentityGuid)
                .IsRequired();

            builder.Property(p => p.FirstNames)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.LastNames)
                .HasMaxLength(100);

            builder.Property(p => p.CreatedTime)
                .HasDefaultValueSql("GETDATE()");

            builder.HasMany(p => p.Tags)
                .WithOne(t => t.ApplicationUser)
                .HasForeignKey(p => p.AppUserId);

            builder.HasMany(p => p.NoteBooks)
                .WithOne(nb => nb.ApplicationUser)
                .HasForeignKey(p => p.AppUserId);

            builder.HasMany(p => p.Todos)
                .WithOne(t => t.ApplicationUser)
                .HasForeignKey(p => p.AppUserId);
        }
    }
}
