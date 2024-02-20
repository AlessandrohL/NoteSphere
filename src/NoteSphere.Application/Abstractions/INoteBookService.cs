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
        Task<List<NotebookDto>> GetAllNotebooksAsync(NotebooksFilter filter);
        Task<NotebookDto?> GetNotebookByIdAsync(Guid id);
        Task<NotebookDto?> CreateNotebookAsync(CreateNotebookDto notebookDto);
        Task<NotebookDto?> UpdateNotebookAsync(Guid id, UpdateNotebookDto notebookDto);
        Task SoftDeleteNotebookAsync(Guid id);
        Task RecoverNotebookAsync(Guid id);
    }
}
