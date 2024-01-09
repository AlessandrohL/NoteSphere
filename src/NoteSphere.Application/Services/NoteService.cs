using Application.Abstractions;
using Application.Common;
using Application.DTOs.Notes;
using AutoMapper;
using Domain.Abstractions;
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
    public class NoteService : INoteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NoteService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<NoteDto>, string>> GetNotesAsync(
            string identityId,
            Guid notebookId,
            NotesFilter request)
        {
            var userId = await _unitOfWork.ApplicationUser
                .FindUserIdByIdentityIdAsync(identityId);

            if (userId == Guid.Empty) 
                return ApplicationUserErrors.NotFound;

            var notebookExists = await _unitOfWork.NoteBook
                .IsNotebookExistsAsync(userId, notebookId);

            if (!notebookExists) 
                return NotebookErrors.NotFound;

            var notes = await _unitOfWork.Note.FindNotesAsync(request, notebookId);

            var notesDto = _mapper.Map<List<NoteDto>>(notes);

            return notesDto;
        }

    }
}
