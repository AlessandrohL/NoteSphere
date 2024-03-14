using Application.Abstractions;
using Application.DTOs.Notebook;
using AutoMapper;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Entities;
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
        private readonly INotebookRepository _notebookRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NotebookService(
            INotebookRepository notebookRepository,
            IUnitOfWork unitOfWork, 
            IMapper mapper)
        {
            _notebookRepository = notebookRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<NotebookDto>> GetAllNotebooksAsync(
            NotebooksFilter filter,
            CancellationToken cancellationToken)
        {
            var notebooks = await _notebookRepository.FindNotebooksAsync(
                filter, 
                cancellationToken);

            var notebooksDto = _mapper.Map<List<NotebookDto>>(notebooks);

            return notebooksDto;
        }

        public async Task<NotebookDto?> GetNotebookByIdAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            var notebook = await _notebookRepository.FindNotebookByIdAsync(
                id, 
                trackChanges: false, 
                cancellationToken);

            if (notebook is null)
            {
                throw new NotebookNotFoundException(id);
            }

            var notebookDto = _mapper.Map<NotebookDto>(notebook);

            return notebookDto;
        }

        public async Task<NotebookDto?> CreateNotebookAsync(
            CreateNotebookDto noteBookDto,
            CancellationToken cancellationToken)
        {
            var notebook = _mapper.Map<Notebook>(noteBookDto);
            
            _notebookRepository.Create(notebook);
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var notebookDtoToResult = _mapper.Map<NotebookDto>(notebook);

            return notebookDtoToResult;
        }

        public async Task<NotebookDto?> UpdateNotebookAsync(
            Guid id, 
            UpdateNotebookDto notebookDto,
            CancellationToken cancellationToken)
        {
            var notebookDB = await _notebookRepository.FindNotebookByIdAsync(
                id, 
                trackChanges: true, 
                cancellationToken);

            if (notebookDB is null)
            {
                throw new NotebookNotFoundException(id);
            }

            notebookDB = _mapper.Map(notebookDto, notebookDB);
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<NotebookDto>(notebookDB);
        }

        public async Task SoftDeleteNotebookAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            var notebook = await _notebookRepository.FindNotebookByIdAsync(
                id, 
                trackChanges: true, 
                cancellationToken);

            if (notebook is null)
            {
                throw new NotebookNotFoundException(id);
            }

            notebook.Delete();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task RecoverNotebookAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            var notebook = await _notebookRepository.FindNotebookByIdAsync(
                id,
                trackChanges: true,
                cancellationToken,
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
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
