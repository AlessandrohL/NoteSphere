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

        private async Task<Guid> ValidateUserByIdentityAsync(string identityId)
        {
            var appUserId = await _unitOfWork.ApplicationUser
                .FindUserIdByIdentityIdAsync(identityId);

            if (appUserId == Guid.Empty)
            {
                throw new ApplicationUserNotFoundException();
            }

            return appUserId;
        }

        public async Task<List<NotebookDto>> GetNotebooksAsync(
            NotebooksFilter filter,
            string identityId)
        {
            var userId = await ValidateUserByIdentityAsync(identityId);

            var notebooks = await _unitOfWork.Notebook.FindNotebooksAsync(
                filter,
                applicationUserId: userId);

            var notebooksDto = _mapper.Map<List<NotebookDto>>(notebooks);

            return notebooksDto;
        }

        public async Task<NotebookDto?> GetNotebookAsync(
            Guid id,
            string identityId)
        {
            var userId = await ValidateUserByIdentityAsync(identityId);

            var notebook = await _unitOfWork.Notebook.FindNotebookById(
                id,
                applicationUserId: userId,
                trackChanges: false);

            if (notebook is null)
            {
                throw new NotebookNotFoundException(id);
            }

            var notebookDto = _mapper.Map<NotebookDto>(notebook);

            return notebookDto;
        }

        public async Task<NotebookDto?> CreateNotebookAsync(
            CreateNotebookDto noteBookDto,
            string identityId)
        {
            var userId = await ValidateUserByIdentityAsync(identityId);

            var notebook = _mapper.Map<Notebook>(noteBookDto);
            notebook.AppUserId = userId;

            _unitOfWork.Notebook.Create(notebook);

            await _unitOfWork.SaveChangesAsync();

            var notebookDtoToResult = _mapper.Map<NotebookDto>(notebook);

            return notebookDtoToResult;
        }

        public async Task<NotebookDto?> UpdateNotebookAsync(
            Guid id,
            UpdateNotebookDto notebookDto,
            string identityId)
        {
            var userId = await ValidateUserByIdentityAsync(identityId);

            var notebookDB = await _unitOfWork.Notebook.FindNotebookById(
                id,
                applicationUserId: userId,
                trackChanges: true);

            if (notebookDB is null)
            {
                throw new NotebookNotFoundException(id);
            }

            notebookDB = _mapper.Map(notebookDto, notebookDB);
            notebookDB.SetModified();

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<NotebookDto>(notebookDB);
        }

        public async Task SoftDeleteNotebookAsync(Guid id, string identityId)
        {
            var userId = await ValidateUserByIdentityAsync(identityId);

            var notebook = await _unitOfWork.Notebook.FindNotebookById(
                id,
                applicationUserId: userId,
                trackChanges: true);

            if (notebook is null)
            {
                throw new NotebookNotFoundException(id);
            }

            notebook.Delete();
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RecoverNotebookAsync(Guid id, string identityId)
        {
            var userId = await ValidateUserByIdentityAsync(identityId);

            var notebook = await _unitOfWork.Notebook.FindNotebookById(
                id,
                applicationUserId: userId,
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
