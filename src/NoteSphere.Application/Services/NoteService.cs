using Application.Abstractions;
using Application.DTOs.Notes;
using AutoMapper;
using Domain.Abstractions;
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
        private async Task<bool> ValidateNotebookExistsAsync(Guid userId, Guid notebookId)
        {
            var notebookExists = await _unitOfWork.Notebook
                .IsNotebookExistsAsync(userId, notebookId);

            if (!notebookExists)
            {
                throw new NotebookNotFoundException(notebookId);
            }

            return notebookExists;
        }

        public async Task<List<NoteDto>> GetNotesAsync(
            string identityId,
            Guid notebookId,
            NotesFilter request)
        {
            var userId = await ValidateUserByIdentityAsync(identityId);

            await ValidateNotebookExistsAsync(userId, notebookId);

            var notes = await _unitOfWork.Note.FindNotesAsync(request, notebookId);

            var notesDto = _mapper.Map<List<NoteDto>>(notes);

            return notesDto;
        }

        public async Task<NoteDto> GetNoteAsync(string identityId, Guid notebookId, Guid noteId)
        {
            var userId = await ValidateUserByIdentityAsync(identityId);

            await ValidateNotebookExistsAsync(userId, notebookId);

            var note = await _unitOfWork.Note.FindNoteAsync(
                noteId,
                notebookId,
                trackChanges: false,
                ignoreQueryFilter: false);

            if (note is null)
            {
                throw new NoteNotFoundException(noteId);
            }

            var noteDto = _mapper.Map<NoteDto>(note);

            return noteDto;
        }

        public async Task<NoteDto> CreateNoteAsync(
            string identityId, 
            Guid notebookId, 
            CreateNoteDto noteDto)
        {
            var userId = await ValidateUserByIdentityAsync(identityId);

            await ValidateNotebookExistsAsync(userId, notebookId);

            var note = _mapper.Map<Note>(noteDto);
            note.NoteBookId = notebookId;

            _unitOfWork.Note.Create(note);

            await _unitOfWork.SaveChangesAsync();

            var noteDtoToResult = _mapper.Map<NoteDto>(note);

            return noteDtoToResult;
        }

    }
}
