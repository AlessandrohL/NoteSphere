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
                .HasIndex(p => p.TenantId)
                .IsUnique();

            builder.Property(p => p.TenantId)
                .IsRequired();

            builder.Property(p => p.FirstNames)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.LastNames)
                .HasMaxLength(100);

            builder.Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

        }
    }
}
