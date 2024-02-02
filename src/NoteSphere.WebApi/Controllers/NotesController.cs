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
        private readonly UserContextAccessor _userContext;

        public NotesController(
            INoteService noteService,
            UserContextAccessor userContextAccessor)
        {
            _noteService = noteService;
            _userContext = userContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetNotes(
            Guid notebookId,
            [FromQuery] NotesFilter request)
        {
            var identityId = _userContext.GetCurrentIdentityId()!;

            var notes = await _noteService
                .GetNotesAsync(identityId, notebookId, request);

            return Ok(SuccessResponse<List<NoteDto>>.Ok(notes));
        }

        [HttpGet("{id:guid}", Name = nameof(GetNote))]
        public async Task<IActionResult> GetNote(
            Guid notebookId,
            Guid id)
        {
            var identityId = _userContext.GetCurrentIdentityId()!;

            var note = await _noteService
                .GetNoteAsync(identityId, notebookId, id);

            return Ok(SuccessResponse<NoteDto>.Ok(note));
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote(
            Guid notebookId,
            [FromBody] CreateNoteDto createNoteDto)
        {
            var identityId = _userContext.GetCurrentIdentityId()!;

            var note = await _noteService.CreateNoteAsync(
                identityId,
                notebookId,
                createNoteDto);

            return CreatedAtRoute(
                nameof(GetNote),
                new { notebookId, id = note.Id },
                SuccessResponse<NoteDto>.Created(note));
        }
    }
}
