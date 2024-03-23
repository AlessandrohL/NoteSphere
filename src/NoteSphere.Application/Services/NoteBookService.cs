using Application.Abstractions;
using Application.DTOs.Notebook;
using Application.Exceptions;
using AutoMapper;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Filters;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
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
        private readonly IValidator<Notebook> _patchNotebookValidator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NotebookService(
            INotebookRepository notebookRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<Notebook> patchNotebookValidator)
        {
            _notebookRepository = notebookRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _patchNotebookValidator = patchNotebookValidator;
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

        public async Task PatchNotebookAsync(
            Guid id, 
            JsonPatchDocument<PatchNotebookDto> notebookDtoPatchDoc,
            CancellationToken cancellationToken)
        {
            var existingNotebook = await _notebookRepository.FindNotebookByIdAsync(
                id,
                trackChanges: true,
                cancellationToken);

            if (existingNotebook is null)
            {
                throw new NotebookNotFoundException(id);
            }

            var notebookPatchDocument = new JsonPatchDocument<Notebook>();
            var patchOperations = notebookDtoPatchDoc
                .Operations
                .Select(opr => new Operation<Notebook>
                {
                    op = opr.op,
                    path = opr.path,
                    value = opr.value
                });

            notebookPatchDocument.Operations.AddRange(patchOperations);
            notebookPatchDocument.ContractResolver = notebookDtoPatchDoc.ContractResolver;

            var patchErrors = new List<string>();
            notebookPatchDocument.ApplyTo(existingNotebook, e => patchErrors.Add(e.ErrorMessage));

            if (patchErrors.Count > 0)
            {
                throw new PatchOperationException(patchErrors);
            }

            var result = _patchNotebookValidator.Validate(existingNotebook);

            if (!result.IsValid)
            {
                throw new NotebookValidationException(result.Errors.Select(e => e.ErrorMessage));
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
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
