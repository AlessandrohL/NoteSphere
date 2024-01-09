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
                .HasColumnName("Id");

            builder.Property(p => p.Title)
                .HasMaxLength(50)
                .IsUnicode(true)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasMaxLength(70)
                .IsUnicode(true);

            builder.HasMany(nb => nb.Notes)
                .WithOne(n => n.NoteBook)
                .HasForeignKey(n => n.NoteBookId);

            builder.Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasQueryFilter(nb => !nb.IsDeleted);

            //builder.HasData(
            //    new List<NoteBook>
            //    {
            //        new()
            //        {
            //            AppUserId = Guid.Parse("E1B1A067-C543-4A10-2691-08DC058725BA"),
            //            CreatedAt = DateTime.UtcNow,
            //            Id = Guid.NewGuid(),
            //            Title = "Notebook 1"
            //        },
            //        new() {
            //            AppUserId = Guid.Parse("E1B1A067-C543-4A10-2691-08DC058725BA"),
            //            CreatedAt = DateTime.UtcNow,
            //            Id = Guid.NewGuid(),
            //            Title = "Notebook 2"
            //        },
            //        new()
            //        {
            //            AppUserId = Guid.Parse("E1B1A067-C543-4A10-2691-08DC058725BA"),
            //            CreatedAt = DateTime.UtcNow,
            //            Id = Guid.NewGuid(),
            //            Title = "Notebook 3"
            //        },
            //        new()
            //        {
            //            AppUserId = Guid.Parse("E1B1A067-C543-4A10-2691-08DC058725BA"),
            //            CreatedAt = DateTime.UtcNow,
            //            Id = Guid.NewGuid(),
            //            Title = "Notebook 4"
            //        },
            //        new()
            //        {
            //            AppUserId = Guid.Parse("E1B1A067-C543-4A10-2691-08DC058725BA"),
            //            CreatedAt = DateTime.UtcNow,
            //            Id = Guid.NewGuid(),
            //            Title = "Notebook 5"
            //        },
            //        new()
            //        {
            //            AppUserId = Guid.Parse("E1B1A067-C543-4A10-2691-08DC058725BA"),
            //            CreatedAt = DateTime.UtcNow,
            //            Id = Guid.NewGuid(),
            //            Title = "Notebook 6"
            //        },
            //        new()
            //        {
            //            AppUserId = Guid.Parse("E1B1A067-C543-4A10-2691-08DC058725BA"),
            //            CreatedAt = DateTime.UtcNow,
            //            Id = Guid.NewGuid(),
            //            Title = "Notebook 7"
            //        },
            //        new()
            //        {
            //            AppUserId = Guid.Parse("E1B1A067-C543-4A10-2691-08DC058725BA"),
            //            CreatedAt = DateTime.UtcNow,
            //            Id = Guid.NewGuid(),
            //            Title = "Notebook 8"
            //        },
            //        new()
            //        {
            //            AppUserId = Guid.Parse("E1B1A067-C543-4A10-2691-08DC058725BA"),
            //            CreatedAt = DateTime.UtcNow,
            //            Id = Guid.NewGuid(),
            //            Title = "Notebook 9"
            //        },
            //        new()
            //        {
            //            AppUserId = Guid.Parse("E1B1A067-C543-4A10-2691-08DC058725BA"),
            //            CreatedAt = DateTime.UtcNow,
            //            Id = Guid.NewGuid(),
            //            Title = "Notebook 10"
            //        },
            //        new()
            //        {
            //            AppUserId = Guid.Parse("E1B1A067-C543-4A10-2691-08DC058725BA"),
            //            CreatedAt = DateTime.UtcNow,
            //            Id = Guid.NewGuid(),
            //            Title = "Notebook 11"
            //        },
            //        new()
            //        {
            //            AppUserId = Guid.Parse("E1B1A067-C543-4A10-2691-08DC058725BA"),
            //            CreatedAt = DateTime.UtcNow,
            //            Id = Guid.NewGuid(),
            //            Title = "Notebook 12"
            //        },

            //    });
        }
    }
}
