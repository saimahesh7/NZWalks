using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly ILogger<RegionsController> logger;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        //Constructor Injection
        public RegionsController(NZWalksDbContext dbContext,
            ILogger<RegionsController> logger,
            IRegionRepository regionRepository, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        //Get All Regions
        //GET : https://localhost:1234/api/regions
        [HttpGet]
        public async Task<IActionResult> GetAllRegions()
        {
            try
            {
                throw new Exception("This is a Custom Exception");
                //Get the Domain Models Data From the Database
                var regionsDomain = await regionRepository.GetAllAsync();

                //Convert the Domain data into DTO format using Automapper
                var regionDto = mapper.Map<List<RegionDto>>(regionsDomain);

                //Return Dto data to the Client
                return Ok(regionDto);
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex,ex.Message);
                throw;
            }
        }

        //Get single Region(Get Rgion By ID)
        //GET : https://localhost:1234/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
        {
            //Get the Domain Models Data From the Database
            var region = await regionRepository.SingleRegion(id);

            //Check whether the given region is present or not
            if (region == null)
            {
                return NotFound();
            }

            //Convert Domain data into DTO format
            var regionDto = mapper.Map<RegionDto>(region);

            //Return the Dto data to the client
            return Ok(regionDto);
        }

        //Post To create new Region 
        //POST : https://localhost:1234/api/regions
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateRegion([FromBody] CreateRegionDto createRegionDto)
        {
            //Convert the given DTO format data into Domain data using Automapper
            var regionDomain = mapper.Map<Region>(createRegionDto);
            //Add the domain data to the database using dbContext and save the changes
            regionDomain = await regionRepository.CreateRegionAsync(regionDomain);

            //Convert the Domain data to Dto Data using Automapper
            var regionDto = mapper.Map<RegionDto>(regionDomain);

            //Return the data that has been added
            return CreatedAtAction(nameof(GetRegionById), new { Id = regionDto.Id }, regionDto);
        }

        //Put To Update existing Region 
        //PUT : https://localhost:1234/api/regions/{id}
        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionDto updateRegionDto)
        {
            //Convert the Dto data into domain data and Updata the data in the databse through dbContext and save changes
            var regionDomain = mapper.Map<Region>(updateRegionDto);

            //check whether the provided Id is present are not throw Repository
            regionDomain = await regionRepository.UpdateRegionAsync(id, regionDomain);
            if (regionDomain == null)
            {
                return NotFound();
            }

            //Convert the Domain Data to Dto Data and return to the client
            var regionDto = mapper.Map<RegionDto>(regionDomain);
            return Ok(regionDto);
        }

        //Delete To Remove existing Region 
        //PELETE : https://localhost:1234/api/regions/{id}
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
        {
            //Check whether the provided Id is present are not
            var regionDomain = await regionRepository.DeleteRegionAsync(id);
            if (regionDomain == null)
            {
                return NotFound();
            }

            //If it is presented then remove the region from database using dbcontext
            //Convert the Domain data into Dto data and return to the client
            var regionDto = mapper.Map<RegionDto>(regionDomain);
            return Ok(regionDto);
        }
    }
}
