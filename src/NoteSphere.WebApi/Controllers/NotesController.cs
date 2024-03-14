using Application.Abstractions;
using Application.DTOs.Notes;
using Domain.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.ActionFilters;
using WebApi.Common;
using WebApi.ContextAcessor;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/notebooks/{notebookId:guid}/notes")]
    [Authorize]
    [ValidateGuid] 
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetNotes(
            Guid notebookId,
            [FromQuery] NotesFilter request,
            CancellationToken cancellationToken)
        {
            var notes = await _noteService
                .GetAllNotesAsync(notebookId, request, cancellationToken);

            return Ok(SuccessResponse<List<NoteDto>>.Ok(notes));
        }

        [HttpGet("{id:guid}", Name = nameof(GetNote))]
        public async Task<IActionResult> GetNote(
            Guid notebookId,
            Guid id, 
            CancellationToken cancellationToken)
        {
            var note = await _noteService
                .GetNoteByIdAsync(notebookId, id, cancellationToken);

            return Ok(SuccessResponse<NoteDto>.Ok(note));
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote(
            Guid notebookId,
            [FromBody] CreateNoteDto createNoteDto, 
            CancellationToken cancellationToken)
        {
            var note = await _noteService
                .CreateNoteAsync(notebookId, createNoteDto, cancellationToken);
            
            return CreatedAtRoute(
                nameof(GetNote),
                new { notebookId, id = note.Id },
                SuccessResponse<NoteDto>.Created(note));
        }
    }
}
