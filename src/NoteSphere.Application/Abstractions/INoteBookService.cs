using Application.Common;
using Application.DTOs.Notebook;
using Domain.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface INotebookService
    {
        Task<List<NotebookDto>> GetAllNotebooksAsync(
            NotebooksFilter filter,
            CancellationToken cancellationToken);
        Task<NotebookDto?> GetNotebookByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<NotebookDto?> CreateNotebookAsync(
            CreateNotebookDto notebookDto,
            CancellationToken cancellationToken);
        Task<NotebookDto?> UpdateNotebookAsync(
            Guid id, 
            UpdateNotebookDto notebookDto,
            CancellationToken cancellationToken);
        Task SoftDeleteNotebookAsync(Guid id, CancellationToken cancellationToken);
        Task RecoverNotebookAsync(Guid id, CancellationToken cancellationToken);
    }
}
