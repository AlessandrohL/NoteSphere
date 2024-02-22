using Application.Abstractions;
using Application.DTOs.Notebook;
using Domain.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.ActionFilters;
using WebApi.Common;
using WebApi.ContextAcessor;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ValidateGuid]
    public class NotebooksController : ControllerBase
    {
        private readonly INotebookService _notebookService;
        private readonly UserContextAccessor _userContext;
        public NotebooksController(
            INotebookService notebookService,
            UserContextAccessor userContext)
        {
            _notebookService = notebookService;
            _userContext = userContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<NotebookDto>>> GetNotebooks(
            [FromQuery] NotebooksFilter noteBooksFilter, 
            CancellationToken cancellationToken)
        {
            var notebooks = await _notebookService.GetAllNotebooksAsync(
                filter: noteBooksFilter,
                cancellationToken: cancellationToken);

            return Ok(SuccessResponse<List<NotebookDto>>.Ok(notebooks));
        }

        [HttpGet("{id:guid}", Name = "GetNotebook")]
        public async Task<IActionResult> GetNotebook(
            Guid id, 
            CancellationToken cancellationToken)
        {
            var notebook = await _notebookService.GetNotebookByIdAsync(
                id,
                cancellationToken);

            return Ok(SuccessResponse<NotebookDto>.Ok(notebook));
        }

        // probar con ActionResult<NotebookDto>
        [HttpPost]
        public async Task<IActionResult> CreateNotebook(
            [FromBody] CreateNotebookDto createNotebookDto,
            CancellationToken cancellationToken)
        {
            var notebookCreated = await _notebookService.CreateNotebookAsync(
                createNotebookDto,
                cancellationToken);

            return CreatedAtRoute(
                nameof(GetNotebook),
                new { id = notebookCreated?.Id },
                SuccessResponse<NotebookDto>.Created(notebookCreated));
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateNotebook(
            Guid id,
            [FromBody] UpdateNotebookDto updateNotebookDto,
            CancellationToken cancellationToken)
        {
            var updatedNotebook = await _notebookService.UpdateNotebookAsync(
                id, 
                updateNotebookDto,
                cancellationToken);

            return Ok(SuccessResponse<NotebookDto>.Ok(updatedNotebook));
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> SoftDeleteNotebook(
            Guid id,
            CancellationToken cancellationToken)
        {
            await _notebookService.SoftDeleteNotebookAsync(id, cancellationToken);

            return Ok(SuccessResponse<bool>.Ok(false));
        }

        [HttpPost("{id:guid}/recover")]
        public async Task<IActionResult> RecoverNotebook(
            Guid id, 
            CancellationToken cancellationToken)
        {
            await _notebookService.RecoverNotebookAsync(id, cancellationToken);

            return Ok(SuccessResponse<bool>.Ok(false));
        }

        [HttpDelete("{id:guid}/destroy")]
        [AllowAnonymous]
        public IActionResult DestroyNotebook(Guid id)
        {
            //var timeZone = HttpContext.Request.Headers["TimeZone"].ToString();
            string timeZone = "awdawdawd";

            bool isValid = IsValidTimeZone(timeZone);

            if (!isValid)
                return BadRequest("Not Valid");

            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone);

            var dateUtc = DateTime.UtcNow;
            var dateWithZone = TimeZoneInfo.ConvertTimeFromUtc(dateUtc, timeZoneInfo);

            var dateNow = dateWithZone.ToString();

            return Ok($"TimeZone from headers: {dateNow}");
        }

        private bool IsValidTimeZone(string timeZone)
        {
            return TimeZoneInfo.GetSystemTimeZones()
                .Any(tz => tz.Id == timeZone);
        }
    }
}

