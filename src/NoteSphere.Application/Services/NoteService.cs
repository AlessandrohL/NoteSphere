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

        private async Task<bool> ValidateNotebookExistsAsync(Guid notebookId)
        {
            var notebookExists = await _unitOfWork.Notebook.IsNotebookExistsAsync(notebookId);

            if (!notebookExists)
            {
                throw new NotebookNotFoundException(notebookId);
            }

            return notebookExists;
        }

        public async Task<List<NoteDto>> GetAllNotesAsync(Guid notebookId, NotesFilter request)
        {
            await ValidateNotebookExistsAsync(notebookId);

            var notes = await _unitOfWork.Note.FindNotesAsync(request, notebookId);

            var notesDto = _mapper.Map<List<NoteDto>>(notes);

            return notesDto;
        }

        public async Task<NoteDto> GetNoteByIdAsync(Guid notebookId, Guid noteId)
        {
            await ValidateNotebookExistsAsync(notebookId);

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

        public async Task<NoteDto> CreateNoteAsync(Guid notebookId, CreateNoteDto noteDto)
        {
            await ValidateNotebookExistsAsync(notebookId);

            var note = _mapper.Map<Note>(noteDto);
            note.NotebookId = notebookId;

            _unitOfWork.Note.Create(note);

            await _unitOfWork.SaveChangesAsync();

            var noteDtoToResult = _mapper.Map<NoteDto>(note);

            return noteDtoToResult;
        }

    }
}
