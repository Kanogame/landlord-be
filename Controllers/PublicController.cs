using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace landlord_be.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublicController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly string _publicImagesPath;

        public PublicController(IWebHostEnvironment environment)
        {
            _environment = environment;
            _publicImagesPath = Path.Combine(
                _environment.ContentRootPath,
                "wwwroot",
                "public",
                "images"
            );

            // Ensure directory exists
            if (!Directory.Exists(_publicImagesPath))
            {
                Directory.CreateDirectory(_publicImagesPath);
            }
        }

        [HttpPost("upload-image")]
        [Authorize]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { Success = false, Message = "No file provided" });
            }

            // Validate file type
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
            {
                return BadRequest(new { Success = false, Message = "Invalid file type" });
            }

            // Validate file size (e.g., max 5MB)
            if (file.Length > 5 * 1024 * 1024)
            {
                return BadRequest(new { Success = false, Message = "File size too large" });
            }

            try
            {
                // Generate unique filename
                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(_publicImagesPath, fileName);

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Return public URL
                var publicUrl = $"/public/images/{fileName}";

                return Ok(
                    new
                    {
                        Success = true,
                        Url = publicUrl,
                        FileName = fileName,
                    }
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "Error uploading file" });
            }
        }

        [HttpDelete("delete-image/{fileName}")]
        [Authorize]
        public IActionResult DeleteImage(string fileName)
        {
            try
            {
                // Validate filename to prevent directory traversal
                if (fileName.Contains("..") || fileName.Contains("/") || fileName.Contains("\\"))
                {
                    return BadRequest(new { Success = false, Message = "Invalid filename" });
                }

                var filePath = Path.Combine(_publicImagesPath, fileName);

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound(new { Success = false, Message = "File not found" });
                }

                System.IO.File.Delete(filePath);

                return Ok(new { Success = true, Message = "File deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "Error deleting file" });
            }
        }
    }
}
