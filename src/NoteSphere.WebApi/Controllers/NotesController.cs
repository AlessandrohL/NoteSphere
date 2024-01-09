using Application.Abstractions;
using Domain.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.ActionFilters;
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

            var result = await _noteService
                .GetNotesAsync(identityId, notebookId, request);

            return result.Match<IActionResult>(Ok, BadRequest);
        }
    }
}
