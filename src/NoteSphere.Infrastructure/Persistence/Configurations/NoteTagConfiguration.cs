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
    public class NoteTagConfiguration : IEntityTypeConfiguration<NoteTag>
    {
        public void Configure(EntityTypeBuilder<NoteTag> builder)
        {
            builder.HasKey(p => new { p.NoteId, p.TagId });

            builder
                .HasOne(p => p.Note)
                .WithMany(p => p.Tags)
                .HasForeignKey(p => p.NoteId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(p => p.Tag)
                .WithMany(p => p.Notes)
                .HasForeignKey(p => p.TagId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
