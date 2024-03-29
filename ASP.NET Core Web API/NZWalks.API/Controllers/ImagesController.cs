﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domains;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        //Post: /api/images/upload
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Uplaod([FromForm] ImageUploadRequestDto request)
        {
            ValidateFileUpload(request);
            if(ModelState.IsValid)
            {
                //convert dto to domain model

                var imageDomainModel = new Image
                {
                    File = request.File,
                    FileExtension = Path.GetExtension(request.File.FileName),
                    FileSizeInBytes = request.File.Length,
                    FileName = request.FileName,
                    FileDescription = request.FileDescription
                };
                //Use repository to uplaod image
                await imageRepository.upload(imageDomainModel);

                return Ok(imageDomainModel);
            }
            return BadRequest(ModelState);
        }
        private void ValidateFileUpload(ImageUploadRequestDto request)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            if (!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
            {
                ModelState.AddModelError("file", "Unsupported File Format.");
            }
            if(request.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size is more than 10MB, please uplaod a smaller size file.");
            }
        }
    }
}
