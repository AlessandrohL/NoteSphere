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
        Task<List<NotebookDto>> GetNotebooksAsync(
            NotebooksFilter filter,
            string identityId);
        Task<NotebookDto?> GetNotebookAsync(Guid id, string identityId);
        Task<NotebookDto?> CreateNotebookAsync(
            CreateNotebookDto notebookDto,
            string identityId);
        Task<NotebookDto?> UpdateNotebookAsync(
            Guid id,
            UpdateNotebookDto notebookDto,
            string identityId);
        Task SoftDeleteNotebookAsync(Guid id, string identityId);
        Task RecoverNotebookAsync(Guid id, string identityId);
    }
}
