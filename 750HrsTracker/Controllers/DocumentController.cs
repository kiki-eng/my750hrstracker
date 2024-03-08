using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _750HrsTracker.Controllers
{
    [Route("api/documents")]
    [ApiController]
    [AllowAnonymous]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadDocumentAsync([FromForm] DocumentUploadRequest request)
            => Ok(await _documentService.UploadDocumentAsync(request));
    }
}
