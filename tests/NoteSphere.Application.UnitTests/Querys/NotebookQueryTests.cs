using Application.Querys;
using Domain.Entities;
using Domain.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteSphere.Application.UnitTests.Querys
{
    [TestFixture]
    public class NotebookQueryTests
    {
        private static List<Notebook> GetNotebooks()
        {
            return new List<Notebook>
            {
                new()
                {
                    Id = new Guid(),
                    Title = "My Note 1",
                    Description = "This is my note",
                    CreatedAt = DateTime.UtcNow.AddDays(-10),
                    AppUserId = new Guid(),
                },
                new()
                {
                    Id = new Guid(),
                    Title = "Example",
                    Description = "This is my note",
                    CreatedAt = DateTime.UtcNow.AddDays(-5),
                    AppUserId = new Guid(),
                },
                new()
                {
                    Id = new Guid(),
                    Title = "Oya",
                    Description = "This is my note",
                    CreatedAt = DateTime.UtcNow.AddDays(-3),
                    AppUserId = new Guid(),
                },
                new()
                {
                    Id = new Guid(),
                    Title = "Abstractation",
                    Description = "This is my note",
                    CreatedAt = DateTime.UtcNow.AddDays(-4),
                    AppUserId = new Guid(),
                },
                new()
                {
                    Id = new Guid(),
                    Title = "Games",
                    Description = "This is my note",
                    CreatedAt = DateTime.UtcNow.AddDays(-7),
                    AppUserId = new Guid(),
                },
                new()
                {
                    Id = new Guid(),
                    Title = "Courses",
                    Description = "This is my note",
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    AppUserId = new Guid(),
                },
                new()
                {
                    Id = new Guid(),
                    Title = "How to...",
                    Description = "This is my note",
                    CreatedAt = DateTime.UtcNow.AddDays(-4),
                    AppUserId = new Guid(),
                },
                new()
                {
                    Id = new Guid(),
                    Title = "ASP.NET Core Middlewares",
                    Description = "This is my note",
                    CreatedAt = DateTime.UtcNow.AddDays(-4),
                    AppUserId = new Guid(),
                },
                new()
                {
                    Id = new Guid(),
                    Title = "ASP.NET Core Caching",
                    Description = "This is my note",
                    CreatedAt = DateTime.UtcNow.AddDays(-4),
                    AppUserId = new Guid(),
                },
                
            };
        }


        [Test]
        public void Generate_WithSearchTerm_FiltersCorrectly()
        {
            var searchTerm = "ASP.NET";
            var filter = new NotebooksFilter { SearchTerm = searchTerm};
            var query = GetNotebooks().AsQueryable();

            var result = NotebookQuery.Generate(query, filter).ToList();

            Assert.That(result, Has.All.Property("Title").Contains(searchTerm));
        }

        [Test]
        public void Generate_OrderByCreatedAtAsc_SortsCorrectly()
        {
            var filter = new NotebooksFilter { SortColumn = "created" };
            var query = GetNotebooks().AsQueryable();

            var result = NotebookQuery.Generate(query, filter).ToList();

            Assert.That(result, Is.Ordered.By(nameof(Notebook.CreatedAt)).Ascending);
        }

        [Test]
        public void Generate_OrderByCreatedAtDesc_SortsCorrectly()
        {
            var filter = new NotebooksFilter { SortColumn = "created", SortOrder = "desc" };
            var query = GetNotebooks().AsQueryable();

            var result = NotebookQuery.Generate(query, filter).ToList();

            Assert.That(result, Is.Ordered.By(nameof(Notebook.CreatedAt)).Descending);
        }

        [Test]
        public void Generate_PaginationWithValidParameters_ReturnsCorrectPageSize()
        {
            int expectedPageSize = 5;
            var filter = new NotebooksFilter { Page = 1, PageSize = expectedPageSize };
            var query = GetNotebooks().AsQueryable();

            var result = NotebookQuery.Generate(query, filter).ToList();

            Assert.That(result, Has.Count.LessThanOrEqualTo(expectedPageSize));
        }

        [Test]
        public void Generate_PaginationWithInvalidParamenters_ReturnsDefaultCorrectParams()
        {
            int invalidPageSize = 50;
            int invalidPage = -5;

            int expectedDefaultPageSize = 20;
            int expectedDefaultPage = 1;

            var filter = new NotebooksFilter { Page = invalidPage, PageSize = invalidPageSize };
            var query = GetNotebooks().AsQueryable();

            var result = NotebookQuery.Generate(query, filter).ToList();

            Assert.Multiple(() =>
            {
                Assert.That(filter.Page, Is.EqualTo(expectedDefaultPage));
                Assert.That(filter.PageSize, Is.EqualTo(expectedDefaultPageSize));
                Assert.That(result, Has.Count.LessThanOrEqualTo(expectedDefaultPageSize));
            });
        }
    }
}
