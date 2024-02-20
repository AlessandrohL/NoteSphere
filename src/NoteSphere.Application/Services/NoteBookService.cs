using Application.Abstractions;
using Application.Common;
using Application.DTOs.Notebook;
using Application.Errors;
using AutoMapper;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Errors;
using Domain.Exceptions;
using Domain.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class NotebookService : INotebookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NotebookService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<NotebookDto>> GetAllNotebooksAsync(
            NotebooksFilter filter)
        {
            var notebooks = await _unitOfWork.Notebook.FindNotebooksAsync(filter);

            var notebooksDto = _mapper.Map<List<NotebookDto>>(notebooks);

            return notebooksDto;
        }

        public async Task<NotebookDto?> GetNotebookByIdAsync(Guid id)
        {
            var notebook = await _unitOfWork.Notebook.FindNotebookById(id, trackChanges: false);

            if (notebook is null)
            {
                throw new NotebookNotFoundException(id);
            }

            var notebookDto = _mapper.Map<NotebookDto>(notebook);

            return notebookDto;
        }

        public async Task<NotebookDto?> CreateNotebookAsync(CreateNotebookDto noteBookDto)
        {
            var notebook = _mapper.Map<Notebook>(noteBookDto);
            
            _unitOfWork.Notebook.Create(notebook);
            await _unitOfWork.SaveChangesAsync();

            var notebookDtoToResult = _mapper.Map<NotebookDto>(notebook);

            return notebookDtoToResult;
        }

        public async Task<NotebookDto?> UpdateNotebookAsync(Guid id, UpdateNotebookDto notebookDto)
        {
            var notebookDB = await _unitOfWork.Notebook.FindNotebookById(id, trackChanges: true);

            if (notebookDB is null)
            {
                throw new NotebookNotFoundException(id);
            }

            notebookDB = _mapper.Map(notebookDto, notebookDB);
            
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<NotebookDto>(notebookDB);
        }

        public async Task SoftDeleteNotebookAsync(Guid id)
        {
            var notebook = await _unitOfWork.Notebook.FindNotebookById(id, trackChanges: true);

            if (notebook is null)
            {
                throw new NotebookNotFoundException(id);
            }

            notebook.Delete();
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RecoverNotebookAsync(Guid id)
        {
            var notebook = await _unitOfWork.Notebook.FindNotebookById(
                id,
                trackChanges: true,
                ignoreQueryFilter: true);

            if (notebook is null)
            {
                throw new NotebookNotFoundException(id);
            }

            if (!notebook.IsDeleted)
            {
                throw new NotebookNotDeletedException(id);
            }

            notebook.Restore();
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
