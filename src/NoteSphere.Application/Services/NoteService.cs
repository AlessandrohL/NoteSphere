using Application.Abstractions;
using Application.DTOs.Notes;
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
    public class NoteService : INoteService
    {
        private readonly INoteRepository _noteRepository;
        private readonly INotebookRepository _notebookRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NoteService(
            INoteRepository noteRepository,
            INotebookRepository notebookRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _noteRepository = noteRepository;
            _notebookRepository = notebookRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        private async Task<bool> ValidateNotebookExistsAsync(
            Guid notebookId,
            CancellationToken cancellationToken)
        {
            var notebookExists = await _notebookRepository
                .IsNotebookExistsAsync(notebookId, cancellationToken);

            if (!notebookExists)
            {
                throw new NotebookNotFoundException(notebookId);
            }

            return notebookExists;
        }

        public async Task<List<NoteDto>> GetAllNotesAsync(
            Guid notebookId, 
            NotesFilter request,
            CancellationToken cancellationToken)
        {
            await ValidateNotebookExistsAsync(notebookId, cancellationToken);

            var notes = await _noteRepository
                .FindNotesAsync(request, notebookId, cancellationToken);

            var notesDto = _mapper.Map<List<NoteDto>>(notes);

            return notesDto;
        }

        public async Task<NoteDto> GetNoteByIdAsync(
            Guid notebookId, 
            Guid noteId,
            CancellationToken cancellationToken)
        {
            await ValidateNotebookExistsAsync(notebookId, cancellationToken);

            var note = await _noteRepository.FindNoteByIdAsync(
                noteId,
                notebookId,
                trackChanges: false,
                ignoreQueryFilter: false,
                cancellationToken);

            if (note is null)
            {
                throw new NoteNotFoundException(noteId);
            }

            var noteDto = _mapper.Map<NoteDto>(note);

            return noteDto;
        }

        public async Task<NoteDto> CreateNoteAsync(
            Guid notebookId, 
            CreateNoteDto noteDto,
            CancellationToken cancellationToken)
        {
            await ValidateNotebookExistsAsync(notebookId, cancellationToken);

            var note = _mapper.Map<Note>(noteDto);
            note.NotebookId = notebookId;

            _noteRepository.Create(note);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var noteDtoToResult = _mapper.Map<NoteDto>(note);

            return noteDtoToResult;
        }

    }
}
