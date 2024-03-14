using Application.Abstractions;
using Application.DTOs.Notebook;
using Application.Validators.Notebook;
using Domain.Filters;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebApi.ActionFilters;
using WebApi.Common;
using WebApi.ContextAcessor;
using WebApi.Extensions;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    [Authorize]
    [ValidateGuid]
    [ApiController]
    public class NotebooksController : ControllerBase
    {
        private readonly INotebookService _notebookService;
        private readonly IValidator<CreateNotebookDto> _createValidator;
        private readonly IValidator<UpdateNotebookDto> _updateValidator;
        private readonly UserContextAccessor _userContext;
        public NotebooksController(
            INotebookService notebookService,
            UserContextAccessor userContext,
            IValidator<CreateNotebookDto> createValidator,
            IValidator<UpdateNotebookDto> updateValidator)
        {
            _notebookService = notebookService;
            _userContext = userContext;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
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
            var result = _createValidator.Validate(createNotebookDto);

            if (!result.IsValid)
            {
                result.AddValidationToModelState(ModelState);
                return ValidationProblem(ModelState);
            }

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
            var result = _updateValidator.Validate(updateNotebookDto);

            if (!result.IsValid)
            {
                result.AddValidationToModelState(ModelState);
                return ValidationProblem(ModelState);
            }

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
        public IActionResult DestroyNotebook(Guid id)
        {
            return BadRequest("Not implemented.");
        }

    }
}

