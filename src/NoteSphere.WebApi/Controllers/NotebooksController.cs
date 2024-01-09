using Application.Abstractions;
using Application.DTOs.NoteBook;
using Domain.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.ActionFilters;
using WebApi.ContextAcessor;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ValidateGuid]
    public class NotebooksController : ControllerBase
    {
        private readonly INoteBookService _noteBookService;
        private readonly IApplicationUserService _applicationUserService;
        private readonly UserContextAccessor _userContext;
        public NotebooksController(
            INoteBookService noteBookService,
            IApplicationUserService applicationUser,
            UserContextAccessor userContext)
        {
            _noteBookService = noteBookService;
            _applicationUserService = applicationUser;
            _userContext = userContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetNoteBooks([FromQuery] NoteBooksFilter noteBooksFilter)
        {
            var identityId = _userContext.GetCurrentIdentityId()!;
            var userIdResult = await _applicationUserService.GetUserIdByIdentityIdAsync(identityId);

            if (userIdResult.IsError) return BadRequest(userIdResult.Error);

            var notebooksResult = await _noteBookService.GetNoteBooksAsync(noteBooksFilter, userIdResult.Value);

            return notebooksResult.Match<IActionResult>(Ok, BadRequest);
        }

        [HttpGet("{id:guid}", Name = "GetNoteBook")]
        public async Task<IActionResult> GetNoteBook(Guid id)
        {
            var identityId = _userContext.GetCurrentIdentityId()!;
            var userIdResult = await _applicationUserService.GetUserIdByIdentityIdAsync(identityId);

            if (userIdResult.IsError) return BadRequest(userIdResult.Error);

            var getResult = await _noteBookService.GetNotebookAsync(id, userIdResult.Value);

            return getResult.Match<IActionResult>(Ok, NotFound);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotebook([FromBody] CreateNoteBookDto noteBookDto)
        {
            var identityId = _userContext.GetCurrentIdentityId()!;
            var userIdResult = await _applicationUserService.GetUserIdByIdentityIdAsync(identityId);

            if (userIdResult.IsError) return BadRequest(userIdResult.Error);

            var createResult = await _noteBookService.CreateNotebookAsync(noteBookDto, userIdResult.Value);

            return createResult.Match<IActionResult>(
                success => CreatedAtRoute(nameof(GetNoteBook), new { id = success?.Id }, success),
                BadRequest);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateNotebook(
            Guid id,
            [FromBody] UpdateNotebookDto notebookDto)
        {
            var identityId = _userContext.GetCurrentIdentityId()!;
            var userIdResult = await _applicationUserService.GetUserIdByIdentityIdAsync(identityId);

            if (userIdResult.IsError) return BadRequest(userIdResult.Error);

            var updateResult = await _noteBookService.UpdateNotebookAsync(id, notebookDto, userIdResult.Value);

            return updateResult.Match<IActionResult>(Ok, BadRequest);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> SoftDeleteNotebook(
            Guid id)
        {
            var identityId = _userContext.GetCurrentIdentityId()!;
            var userIdResult = await _applicationUserService.GetUserIdByIdentityIdAsync(identityId);

            if (userIdResult.IsError) return BadRequest(userIdResult.Error);

            var deleteResult = await _noteBookService.SoftDeleteNotebookAsync(id, userIdResult.Value);

            return deleteResult.Match<IActionResult>(s => NoContent(), BadRequest);
        }

        [HttpPost("{id:guid}/recover")]
        public async Task<IActionResult> RecoverNotebook(Guid id)
        {
            var identityId = _userContext.GetCurrentIdentityId()!;
            var userIdResult = await _applicationUserService.GetUserIdByIdentityIdAsync(identityId);

            if (userIdResult.IsError) return BadRequest(userIdResult.Error);

            var recoverResult = await _noteBookService.RecoverNotebookAsync(id, userIdResult.Value);

            return recoverResult.Match<IActionResult>(s => Ok("Notebook recovered!"), BadRequest);
        }

        [HttpDelete("{id:guid}/destroy")]
        [AllowAnonymous]
        public IActionResult DestroyNotebook(Guid id)
        {
            return Ok();
        }
    }
}

