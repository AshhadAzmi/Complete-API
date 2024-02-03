using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilter;
using NZWalks.API.Data;
using NZWalks.API.Models.Domains;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repositories;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, 
            IMapper mapper, ILogger<RegionsController> logger)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        [HttpGet]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll() 
        {
            /* var regions = new List<Region>
             {
                 new Region
                 {
                     Id = Guid.NewGuid(),
                     Name = "Aukland Region",
                     Code = "AKL",
                     RegionImageUrl = "https://media.istockphoto.com/id/1262261871/photo/auckland-city-skyline.jpg?s=612x612&w=0&k=20&c=YN38oRgyECilKOReRuROM2W3lrQkzyZWAF4jJEiWuwk="
                 },
                 new Region
                 {
                     Id = Guid.NewGuid(),
                     Name = "Willington Region",
                     Code = "WLG",
                     RegionImageUrl = "https://media.istockphoto.com/id/1262261871/photo/auckland-city-skyline.jpg?s=612x612&w=0&k=20&c=YN38oRgyECilKOReRuROM2W3lrQkzyZWAF4jJEiWuwk="
                 }
             }; */

            /*try
            {
                throw new Exception("This is custom exception.");
                //Get Data from the DB - Domain Models
                //var regions = await dbContext.Regions.ToListAsync(); without the Repository
                var regions = await regionRepository.GetAllAsync();

                logger.LogInformation($"Finished GetAll Regions request with data: {JsonSerializer.Serialize(regions)}");
                //Map Domain models to DTOs
                //return DTOs to the client
                return Ok(mapper.Map<List<RegionDTO>>(regions));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }*/
            var regions = await regionRepository.GetAllAsync();

            return Ok(mapper.Map<List<RegionDTO>>(regions));
        }

        [HttpGet]
        [Route("{id}")]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById(Guid id)
        {
            //Getting Data from Domain Model
            //var region = dbContext.Regions.Find(id);

            //var region = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            var region = await regionRepository.GetByIdAsync(id);

            if(region == null) 
            {
                return NotFound();
            }

            //Mapping Domain model to DTO
            //var regionDto = new RegionDTO
            //{
            //    Id = region.Id,
            //    Code = region.Code,
            //    Name = region.Name,
            //    RegionImageUrl= region.RegionImageUrl,
            //};

            //Return DTO
            return Ok(mapper.Map<RegionDTO>(region));
        }

        //Creating new region using post
        [HttpPost]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //Map dto to domain model
            var regionDomain = mapper.Map<Region>(addRegionRequestDto);

            //Use Domain model to create the region
            regionDomain = await regionRepository.CreateAsync(regionDomain);

            //Map domain model to the dto and send it to the client
            var regionDto = mapper.Map<RegionDTO>(regionDomain);
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
            
        }

        //Update the region
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            //Map dto to domain model
            var regionDomain = mapper.Map<Region>(updateRegionRequestDto);
            //check if it exists
            regionDomain = await regionRepository.UpdateAsync(id, regionDomain);

            if (regionDomain == null)
            {
                return NotFound();
            }

            //domail model to dto
            return Ok(mapper.Map<RegionDTO>(regionDomain));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id) 
        {
            var regionDomain = await regionRepository.DeleteAsync(id);

            if(regionDomain == null) 
            {
                return NotFound();
            }

            //return the deleted region
            //map domain model to dto the return
            return Ok(mapper.Map<RegionDTO>(regionDomain));

        }

    }
}
