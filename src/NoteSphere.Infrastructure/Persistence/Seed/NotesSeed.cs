using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Seed
{
    public class NotesSeed : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.HasData(new List<Note>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Nota SK",
                    Content = "Contenido de ejemplo",
                    NoteBookId = Guid.Parse("CDC4756E-B9BB-4C08-D303-08DC0E447268"),
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Segunda Nota",
                    Content = "Contenido de la segunda nota",
                    NoteBookId = Guid.Parse("CDC4756E-B9BB-4C08-D303-08DC0E447268"),
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Tercera Nota",
                    Content = "Contenido de la tercera nota",
                    NoteBookId = Guid.Parse("CDC4756E-B9BB-4C08-D303-08DC0E447268"),
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Cuarta Nota",
                    Content = "Contenido de la cuarta nota",
                    NoteBookId = Guid.Parse("CDC4756E-B9BB-4C08-D303-08DC0E447268"),
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Quinta Nota",
                    Content = "Contenido de la quinta nota",
                    NoteBookId = Guid.Parse("CDC4756E-B9BB-4C08-D303-08DC0E447268"),
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Sexta Nota",
                    Content = "Contenido de la sexta nota",
                    NoteBookId = Guid.Parse("CDC4756E-B9BB-4C08-D303-08DC0E447268"),
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Séptima Nota",
                    Content = "Contenido de la séptima nota",
                    NoteBookId = Guid.Parse("CDC4756E-B9BB-4C08-D303-08DC0E447268"),
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Octava Nota",
                    Content = "Contenido de la octava nota",
                    NoteBookId = Guid.Parse("CDC4756E-B9BB-4C08-D303-08DC0E447268"),
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Novena Nota",
                    Content = "Contenido de la novena nota",
                    NoteBookId = Guid.Parse("CDC4756E-B9BB-4C08-D303-08DC0E447268"),
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Décima Nota",
                    Content = "Contenido de la décima nota",
                    NoteBookId = Guid.Parse("CDC4756E-B9BB-4C08-D303-08DC0E447268"),
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Undécima Nota",
                    Content = "Contenido de la undécima nota",
                    NoteBookId = Guid.Parse("CDC4756E-B9BB-4C08-D303-08DC0E447268"),
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Duodécima Nota",
                    Content = "Contenido de la duodécima nota",
                    NoteBookId = Guid.Parse("CDC4756E-B9BB-4C08-D303-08DC0E447268"),
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Decimotercera Nota",
                    Content = "Contenido de la decimotercera nota",
                    NoteBookId = Guid.Parse("CDC4756E-B9BB-4C08-D303-08DC0E447268"),
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Decimocuarta Nota",
                    Content = "Contenido de la decimocuarta nota",
                    NoteBookId = Guid.Parse("CDC4756E-B9BB-4C08-D303-08DC0E447268"),
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Decimoquinta Nota",
                    Content = "Contenido de la decimoquinta nota",
                    NoteBookId = Guid.Parse("CDC4756E-B9BB-4C08-D303-08DC0E447268"),
                },
            });
        }
    }
}
