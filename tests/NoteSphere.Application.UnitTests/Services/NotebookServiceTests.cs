using Application.Abstractions;
using Application.DTOs.Notebook;
using Application.Services;
using AutoMapper;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Filters;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteSphere.Application.UnitTests.Services
{
    [TestFixture]
    public class NotebookServiceTests
    {
        private Mock<IUnitOfWork> mockUnitOfWork;
        private Mock<INotebookRepository> mockNotebookRepository;
        private Mock<IApplicationUserRepository> mockApplicationUserRepository;
        private Mock<IMapper> mockMapper;
        private INotebookService _notebookService;
        private static List<Notebook> GetTestNotebooks(Guid appUserId)
        {
            return new List<Notebook>
            {
                new()
                {
                    Id = new Guid("71f2fab8-3ad8-43af-aa6e-a7d58f9d250a"),
                    Title = "Note 1",
                    Description = "This is my note 1",
                    CreatedAt = DateTime.UtcNow,
                    AppUserId = appUserId,
                },
                new()
                {
                    Id = new Guid("683d30e1-62c2-438e-8aa8-bebc2116755e"),
                    Title = "Note 2",
                    Description = "This is my note 2",
                    CreatedAt = DateTime.UtcNow,
                    AppUserId = appUserId,
                }
            };
        }

        private static List<NotebookDto> GetNotebookDtos(Guid appUserId)
        {
            return new List<NotebookDto>()
            {
                new()
                {
                    Id = new Guid("71f2fab8-3ad8-43af-aa6e-a7d58f9d250a"),
                    Title = "Note 1",
                    Description = "This is my note 1",
                    AppUserId = appUserId
                },
                new()
                {
                    Id = new Guid("683d30e1-62c2-438e-8aa8-bebc2116755e"),
                    Title = "Note 2",
                    Description = "This is my note 2",
                    AppUserId = appUserId
                }
            };
        }


        [SetUp]
        public void ConfigureMocks()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockNotebookRepository = new Mock<INotebookRepository>();
            mockApplicationUserRepository = new Mock<IApplicationUserRepository>();
            mockMapper = new Mock<IMapper>();

            mockUnitOfWork
                .SetupGet(u => u.Notebook)
                .Returns(mockNotebookRepository.Object);

            mockUnitOfWork
                .SetupGet(u => u.ApplicationUser)
                .Returns(mockApplicationUserRepository.Object);

            _notebookService = new NotebookService(mockUnitOfWork.Object, mockMapper.Object);

        }

        [Test]
        public async Task GetNotebooksAsync_WithFilterAndIdentityId_ReturnsListOfNotebooks()
        {
            var notebookFilter = new NotebooksFilter();
            var identityId = "7d08fe0d-fce9-4155-88c1-5ad08b0ad220";
            var appUserId = new Guid("24e7e374-6486-4203-a1ba-3a3b34530745");
            var notebooksExpected = GetTestNotebooks(appUserId);
            var notebooksDtosExpected = GetNotebookDtos(appUserId);

            mockMapper
                .Setup(am => am.Map<List<NotebookDto>>(It.IsAny<List<Notebook>>()))
                .Returns(notebooksDtosExpected);

            mockApplicationUserRepository
                .Setup(a => a.FindUserIdByIdentityIdAsync(identityId))
                .ReturnsAsync(appUserId);

            mockNotebookRepository
                .Setup(n => n.FindNotebooksAsync(notebookFilter, appUserId))
                .ReturnsAsync(notebooksExpected);


            List<NotebookDto> actualResult = await _notebookService.GetNotebooksAsync(
                notebookFilter,
                identityId);


            Assert.That(
                actual: actualResult,
                expression: Has.All.Property("AppUserId").EqualTo(appUserId),
                message: $"All notebooks should have AppUserId equal to {appUserId}.");

            //unitOfWork.Verify((u) => 
            //    u.ApplicationUser.FindUserIdByIdentityIdAsync(identityId), 
            //    Times.Once);

            //unitOfWork.Verify((u) => 
            //    u.Notebook.FindNotebooksAsync(notebookFilter, appUserId), 
            //    Times.Once);
        }

        [Test]
        public void GetNotebooksAsync_NonExistentIdentityId_ThrowsApplicationUserNotFoundException()
        {
            var identityId = "ce70ef09-b3d3-420d-ac8e-8623d5c29abb";
            var exceptionExpectedMessage = "The user with the provided Id was not found.";

            mockApplicationUserRepository
                .Setup(a => a.FindUserIdByIdentityIdAsync(identityId))
                .ThrowsAsync(new ApplicationUserNotFoundException());


            var ex = Assert.ThrowsAsync<ApplicationUserNotFoundException>(
                async () => await _notebookService.GetNotebooksAsync(
                    It.IsAny<NotebooksFilter>(),
                    It.IsAny<string>()));

            Assert.That(ex.Message, Is.EqualTo(exceptionExpectedMessage));
        }

        [Test]
        public async Task GetNotebookAsync_WithNotebookIdAndIdentityId_ReturnsNotebookDto()
        {
            var identityId = "7d08fe0d-fce9-4155-88c1-5ad08b0ad220";
            var appUserId = new Guid("24e7e374-6486-4203-a1ba-3a3b34530745");
            var notebookId = new Guid("5b4e23a4-51b6-412c-a99c-109772e24476");
            var notebookExpectedFromRepository = new Notebook
            {
                Id = notebookId,
                Title = "Note 1",
                Description = "This is my note 1",
                CreatedAt = DateTime.UtcNow,
                AppUserId = appUserId
            };
            var notebookDto = new NotebookDto
            {
                Id = notebookId,
                Title = "Note 1",
                Description = "This is my note 1",
                AppUserId = appUserId
            };

            mockMapper
                .Setup(am => am.Map<NotebookDto>(notebookExpectedFromRepository))
                .Returns(notebookDto);

            mockApplicationUserRepository
                .Setup(a => a.FindUserIdByIdentityIdAsync(identityId))
                .ReturnsAsync(appUserId);

            mockNotebookRepository
                .Setup(n => n.FindNotebookById(notebookId, appUserId, false, false))
                .ReturnsAsync(notebookExpectedFromRepository);


            NotebookDto? actualResult = await _notebookService.GetNotebookAsync(notebookId, identityId);

            Assert.Multiple(() =>
            {
                Assert.That(
                    actualResult,
                    Is.Not.Null,
                    "The result should not be null.");

                Assert.That(
                    actualResult!.Id,
                    Is.EqualTo(notebookId),
                    $"The notebookId should be {notebookId}");

                Assert.That(
                    actualResult.AppUserId,
                    Is.EqualTo(appUserId),
                    $"The appUserId should be {appUserId}");
            });
        }

        [Test]
        public void GetNotebookAsync_NonExistentNotebookId_ThrowsNotebookNotFoundException()
        {
            var notebookId = new Guid("34f1bbb4-c1b3-417e-a975-562201679fab");
            var identityId = "7d08fe0d-fce9-4155-88c1-5ad08b0ad220";
            var appUserId = new Guid("24e7e374-6486-4203-a1ba-3a3b34530745");

            mockApplicationUserRepository
                .Setup(a => a.FindUserIdByIdentityIdAsync(It.IsAny<string>()))
                .ReturnsAsync(appUserId);

            mockNotebookRepository
                .Setup(n => n.FindNotebookById(notebookId, appUserId, false, false))
                .ReturnsAsync((Notebook?)null);

            Assert.ThrowsAsync<NotebookNotFoundException>(async () =>
                await _notebookService.GetNotebookAsync(notebookId, identityId));
        }

        [Test]
        public async Task CreateNotebookAsync_ValidData_ReturnsNewNotebookSuccessfullyCreated()
        {
            var identityId = "7d08fe0d-fce9-4155-88c1-5ad08b0ad220";
            var appUserId = new Guid("24e7e374-6486-4203-a1ba-3a3b34530745");
            var createNotebookDto = new CreateNotebookDto
            {
                Title = "Note 120",
                Description = "This is description for my notebook"
            };
            var notebookMapped = new Notebook
            {
                Id = new Guid(),
                Title = "Note 120",
                Description = "This is description for my notebook",
                AppUserId = appUserId,
                CreatedAt = DateTime.UtcNow,
            };
            var notebookDtoToResult = new NotebookDto
            {
                Id = notebookMapped.Id,
                Title = "Note 120",
                Description = "This is description for my notebook",
                AppUserId = appUserId
            };

            mockMapper
                .Setup(m => m.Map<Notebook>(createNotebookDto))
                .Returns(notebookMapped);

            mockApplicationUserRepository
                .Setup(a => a.FindUserIdByIdentityIdAsync(identityId))
                .ReturnsAsync(appUserId);

            mockUnitOfWork
                .Setup(u => u.SaveChangesAsync())
                .ReturnsAsync(1);

            mockMapper
               .Setup(m => m.Map<NotebookDto>(notebookMapped))
               .Returns(notebookDtoToResult);

            var result = await _notebookService.CreateNotebookAsync(createNotebookDto, identityId);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.InstanceOf<NotebookDto>());
            });
        }

        [Test]
        public async Task UpdateNotebookAsync_WithNotebookIdAndValidData_ReturnsNotebookSuccessfullyUpdated()
        {
            var notebookId = new Guid("d4cddbac-fe85-4048-819d-bb2c45f84ca7");
            var identityId = "7d08fe0d-fce9-4155-88c1-5ad08b0ad220";
            var appUserId = new Guid("24e7e374-6486-4203-a1ba-3a3b34530745");
            var updateNotebookDto = new UpdateNotebookDto
            {
                Title = "Updated title",
                Description = "Other content"
            };
            var existingNotebook = new Notebook
            {
                Id = notebookId,
                Title = "Note 1",
                Description = "This is my note 1",
                CreatedAt = DateTime.UtcNow.AddDays(-2),
                AppUserId = appUserId
            };
            var mappedNotebook =


            mockApplicationUserRepository
                .Setup(a => a.FindUserIdByIdentityIdAsync(identityId))
                .ReturnsAsync(appUserId);

            mockNotebookRepository
                .Setup(n => n.FindNotebookById(notebookId, appUserId, true, false))
                .ReturnsAsync(existingNotebook);

            mockUnitOfWork
                .Setup(u => u.SaveChangesAsync())
                .ReturnsAsync(1);

            var updatedNotebook = new Notebook
            {
                Id = notebookId,
                Title = updateNotebookDto.Title,
                Description = updateNotebookDto.Description,
                AppUserId = appUserId,
                CreatedAt = existingNotebook.CreatedAt
            };

            mockMapper
                .Setup(m => m.Map(updateNotebookDto, existingNotebook))
                .Returns(updatedNotebook);

            var notebookDtoToResult = new NotebookDto
            {
                Id = updatedNotebook.Id,
                Title = updatedNotebook.Title,
                Description = updatedNotebook.Description,
                AppUserId = appUserId,
            };

            mockMapper
                .Setup(m => m.Map<NotebookDto>(updatedNotebook))
                .Returns(notebookDtoToResult);


            var result = await _notebookService.UpdateNotebookAsync(
                notebookId,
                updateNotebookDto,
                identityId);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.InstanceOf<NotebookDto>());
                Assert.That(result!.Id, Is.EqualTo(notebookId));
            });
        }

        [Test]
        public void UpdateNotebookAsync_NonExistentNotebook_ThrowsNotebookNotFoundException()
        {
            var notebookId = new Guid();
            var identityId = "7d08fe0d-fce9-4155-88c1-5ad08b0ad220";
            var appUserId = new Guid("24e7e374-6486-4203-a1ba-3a3b34530745");
            var updateNotebookDto = new UpdateNotebookDto
            {
                Title = "Updated title",
                Description = "Other content"
            };

            mockApplicationUserRepository
                .Setup(a => a.FindUserIdByIdentityIdAsync(identityId))
                .ReturnsAsync(appUserId);

            mockNotebookRepository
                .Setup(n => n.FindNotebookById(notebookId, appUserId, true, false))
                .ReturnsAsync((Notebook?)null);

            Assert.ThrowsAsync<NotebookNotFoundException>(async () =>
                await _notebookService.UpdateNotebookAsync(notebookId, updateNotebookDto, identityId));
        }

        [Test]
        public async Task SoftDeleteNotebookAsync_ExistingNotebook_SuccessfullySoftDeletesNotebook()
        {
            var notebookId = new Guid("d4cddbac-fe85-4048-819d-bb2c45f84ca7");
            var identityId = "7d08fe0d-fce9-4155-88c1-5ad08b0ad220";
            var appUserId = new Guid("24e7e374-6486-4203-a1ba-3a3b34530745");

            var existingNotebook = new Notebook
            {
                Id = notebookId,
                Title = "Notebook 10",
                Description = "This is my notebook 100",
                AppUserId = appUserId,
                CreatedAt = DateTime.UtcNow.AddDays(-3),
                DeleteAt = null,
                IsDeleted = false
            };

            mockApplicationUserRepository
                .Setup(u => u.FindUserIdByIdentityIdAsync(identityId))
                .ReturnsAsync(appUserId);

            mockNotebookRepository
                .Setup(n => n.FindNotebookById(notebookId, appUserId, true, false))
                .ReturnsAsync(existingNotebook);

            mockUnitOfWork
                .Setup(n => n.SaveChangesAsync())
                .ReturnsAsync(1);

            await _notebookService.SoftDeleteNotebookAsync(notebookId, identityId);

            Assert.Multiple(() =>
            {
                Assert.That(existingNotebook.DeleteAt, Is.Not.Null);
                Assert.That(existingNotebook.DeleteAt, Is.EqualTo(DateTime.UtcNow).Within(3).Seconds);
                Assert.That(existingNotebook.IsDeleted, Is.True);
            });
        }

        [Test]
        public void SoftDeleteNotebookAsync_NonExistingNotebook_ThrowsNotebookNotFoundException()
        {
            var notebookId = new Guid();
            var identityId = "7d08fe0d-fce9-4155-88c1-5ad08b0ad220";
            var appUserId = new Guid("24e7e374-6486-4203-a1ba-3a3b34530745");

            mockApplicationUserRepository
                .Setup(a => a.FindUserIdByIdentityIdAsync(identityId))
                .ReturnsAsync(appUserId);

            mockNotebookRepository
                .Setup(n => n.FindNotebookById(notebookId, appUserId, true, false))
                .ReturnsAsync((Notebook?)null);

            Assert.ThrowsAsync<NotebookNotFoundException>(async () =>
                await _notebookService.SoftDeleteNotebookAsync(notebookId, identityId));
        }

        [Test]
        public async Task RecoverNotebookAsync_ExistingNotebook_SuccessfullyRecoverNotebook()
        {
            var notebookId = new Guid("d4cddbac-fe85-4048-819d-bb2c45f84ca7");
            var identityId = "7d08fe0d-fce9-4155-88c1-5ad08b0ad220";
            var appUserId = new Guid("24e7e374-6486-4203-a1ba-3a3b34530745");

            var existingNotebook = new Notebook
            {
                Id = notebookId,
                Title = "Notebook 10",
                Description = "This is my notebook 100",
                AppUserId = appUserId,
                CreatedAt = DateTime.UtcNow.AddDays(-4),
                DeleteAt = DateTime.UtcNow.AddDays(-2),
                IsDeleted = true
            };

            mockApplicationUserRepository
                .Setup(a => a.FindUserIdByIdentityIdAsync(identityId))
                .ReturnsAsync(appUserId);

            mockNotebookRepository
                .Setup(n => n.FindNotebookById(notebookId, appUserId, true, true))
                .ReturnsAsync(existingNotebook);

            mockUnitOfWork
                .Setup(u => u.SaveChangesAsync())
                .ReturnsAsync(1);

            await _notebookService.RecoverNotebookAsync(notebookId, identityId);

            Assert.Multiple(() =>
            {
                Assert.That(existingNotebook.DeleteAt, Is.Null);
                Assert.That(existingNotebook.IsDeleted, Is.False);
            });
        }

        [Test]
        public void RecoverNotebookAsync_NonExistingNotebook_ThrowsNotebookNotFoundException()
        {
            var notebookId = new Guid();
            var identityId = "7d08fe0d-fce9-4155-88c1-5ad08b0ad220";
            var appUserId = new Guid("24e7e374-6486-4203-a1ba-3a3b34530745");

            mockApplicationUserRepository
                .Setup(a => a.FindUserIdByIdentityIdAsync(identityId))
                .ReturnsAsync(appUserId);

            mockNotebookRepository
                .Setup(n => n.FindNotebookById(notebookId, appUserId, true, true))
                .ReturnsAsync((Notebook?)null);

            Assert.ThrowsAsync<NotebookNotFoundException>(async () =>
                await _notebookService.RecoverNotebookAsync(notebookId, identityId));
        }

        [Test]
        public void RecoverNotebookAsync_NotebookNotDeleted_ThrowNotebookNotDeletedException()
        {
            var notebookId = new Guid();
            var identityId = "7d08fe0d-fce9-4155-88c1-5ad08b0ad220";
            var appUserId = new Guid("24e7e374-6486-4203-a1ba-3a3b34530745");

            var existingNotebook = new Notebook
            {
                Id = notebookId,
                Title = "Notebook 100",
                Description = "This is my notebook 100",
                AppUserId = appUserId,
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                DeleteAt = null,
                IsDeleted = false
            };

            mockApplicationUserRepository
                .Setup(a => a.FindUserIdByIdentityIdAsync(identityId))
                .ReturnsAsync(appUserId);

            mockNotebookRepository
                .Setup(n => n.FindNotebookById(notebookId, appUserId, true, true))
                .ReturnsAsync(existingNotebook);

            Assert.ThrowsAsync<NotebookNotDeletedException>(async () =>
                await _notebookService.RecoverNotebookAsync(notebookId, identityId));
        }
    }
}
