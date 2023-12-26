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
    public class NoteBookConfiguration : IEntityTypeConfiguration<NoteBook>
    {
        public void Configure(EntityTypeBuilder<NoteBook> builder)
        {
            builder.ToTable("Notebooks");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasColumnName("NotebookId");

            builder.Property(p => p.Title)
                .HasMaxLength(50)
                .IsUnicode(true)
                .IsRequired();

            builder.HasMany(nb => nb.Notes)
                .WithOne(n => n.NoteBook)
                .HasForeignKey(n => n.NoteBookId);

            builder.Property(p => p.CreatedTime)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
