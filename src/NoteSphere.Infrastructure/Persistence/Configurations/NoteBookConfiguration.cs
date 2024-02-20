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
    public class NotebookConfiguration : IEntityTypeConfiguration<Notebook>
    {
        public void Configure(EntityTypeBuilder<Notebook> builder)
        {
            builder.ToTable("Notebooks");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasColumnName("Id");

            builder
                .HasIndex(p => p.TenantId)
                .IsUnique();

            builder.Property(p => p.TenantId)
                .IsRequired();

            builder.Property(p => p.Title)
                .HasMaxLength(50)
                .IsUnicode(true)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasMaxLength(70)
                .IsUnicode(true);

            builder.HasMany(nb => nb.Notes)
                .WithOne(n => n.Notebook)
                .HasForeignKey(n => n.NotebookId);

            builder.Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

        }
    }
}
