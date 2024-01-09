using Application.Common;
using Application.DTOs.NoteBook;
using Domain.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface INoteBookService
    {
        Task<Result<List<NoteBookDto>, string>> GetNoteBooksAsync(NoteBooksFilter request, Guid userId);
        Task<Result<NoteBookDto?, string>> GetNotebookAsync(Guid id, Guid userId);
        Task<Result<NoteBookDto?, string>> CreateNotebookAsync(CreateNoteBookDto notebookDto, Guid userId);
        Task<Result<NoteBookDto?, string>> UpdateNotebookAsync(Guid id, UpdateNotebookDto notebookDto, Guid userId);
        Task<Result<bool, string>> SoftDeleteNotebookAsync(Guid id, Guid userId);
        Task<Result<bool, string>> RecoverNotebookAsync(Guid id, Guid userId);
    }
}
