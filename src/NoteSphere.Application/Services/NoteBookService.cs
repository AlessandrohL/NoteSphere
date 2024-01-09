using Application.Abstractions;
using Application.Common;
using Application.DTOs.NoteBook;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Errors;
using Domain.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class NoteBookService : INoteBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NoteBookService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<NoteBookDto>, string>> GetNoteBooksAsync(
            NoteBooksFilter request,
            Guid userId)
        {
            var notebooks = await _unitOfWork.NoteBook.FindNotebooksAsync(request, userId);

            var notebooksDto = _mapper.Map<List<NoteBookDto>>(notebooks);

            return notebooksDto;
        }

        public async Task<Result<NoteBookDto?, string>> GetNotebookAsync(
            Guid id,
            Guid userId)
        {

            var notebook = await _unitOfWork.NoteBook.FindNotebookById(id, userId, false);
            
            if (notebook is null) return NotebookErrors.NotFound;

            var notebookDto = _mapper.Map<NoteBookDto>(notebook);

            return notebookDto;
        }

        public async Task<Result<NoteBookDto?, string>> CreateNotebookAsync(
            CreateNoteBookDto noteBookDto,
            Guid userId)
        {
            var notebook = _mapper.Map<NoteBook>(noteBookDto);
            notebook.AppUserId = userId;

            _unitOfWork.NoteBook.Create(notebook);

            await _unitOfWork.SaveChangesAsync();

            var notebookDtoToResult = _mapper.Map<NoteBookDto>(notebook);

            return notebookDtoToResult;
        }

        public async Task<Result<NoteBookDto?, string>> UpdateNotebookAsync(
            Guid notebookId,
            UpdateNotebookDto notebookDto,
            Guid userId)
        {
            var notebookDB = await _unitOfWork.NoteBook.FindNotebookById(
                notebookId,
                userId,
                trackChanges: true);

            if (notebookDB is null) return NotebookErrors.NotFound;

            notebookDB = _mapper.Map(notebookDto, notebookDB);
            notebookDB.SetModified();
 
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<NoteBookDto>(notebookDB);
        }

        public async Task<Result<bool, string>> SoftDeleteNotebookAsync(Guid id, Guid userId)
        {
            var notebook = await _unitOfWork.NoteBook.FindNotebookById(id, userId, true);
            
            if (notebook is null) return NotebookErrors.NotFound;

            notebook.Delete();
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<Result<bool, string>> RecoverNotebookAsync(Guid id, Guid userId)
        {
            var notebook = await _unitOfWork.NoteBook.FindNotebookById(id, userId, true, ignoreQueryFilter: true);
            
            if (notebook is null) return NotebookErrors.NotFound;

            if (!notebook.IsDeleted) return NotebookErrors.NotInTrash;

            notebook.Restore();
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
