using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper,IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        //Get All the Walks
        //GET ://https:localhost:1234/api/walks?filerOn=Name&filterQuery=words&sortBy=Name&isAscending=true
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, 
                                                [FromQuery]string? sortBy, [FromQuery]bool? isAscending)
        {
            //Get the Domain Data from the Repository
            var walkDomain=await walkRepository.GetAllAsync(filterOn,filterQuery,sortBy,isAscending ?? true);

            //Throw Custom Exception
            throw new Exception("This is custom Exception");
            //Convert the Domain Data into Dto Data using Automapper and return to the client
            var walkDto=mapper.Map<List<WalkDto>>(walkDomain);
            return Ok(walkDto);
        }

        //Get a single Walks
        //GET ://https:localhost:1234/api/walks/{id}
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            //Get the Domain data from the repository
            var walkDomain=await walkRepository.GetByIdAsync(id);
            if(walkDomain == null)
            {
                return NotFound();
            }
            //Convert the domain data to Dto data by Automappers and return to the client
            var walkDto=mapper.Map<WalkDto>(walkDomain);
            return Ok(walkDto);
        }

        //Post Create a Walk
        //POST ://https:localhost:1234/api/walks
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateWalks([FromBody]AddWalksRequestDto addWalksRequestDto)
        {
            
                //Convert the DTO data into Domain Data using AutoMapper
                var walksDomain = mapper.Map<Walk>(addWalksRequestDto);

                //Add the domain data to the repository method
                walksDomain = await walkRepository.CreateWalksAsync(walksDomain);

                //Convert Domain Data into Dto Data using AutoMapper and return to client
                var walkDto = mapper.Map<WalkDto>(walksDomain);
                return CreatedAtAction(nameof(GetById), new { Id = walkDto.Id }, walkDto); 
           
            
        }

        //Put update the given Walk
        //PUT ://https:localhost:1234/api/walks/{id}
        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute]Guid id, [FromBody]UpdateWalkDto updateWalkDto)
        {
                //Convert the Dto data to Domain data using Automappers
                var walkDomain = mapper.Map<Walk>(updateWalkDto);
                //Update the domain to the repository
                walkDomain = await walkRepository.UpdateAsync(id, walkDomain);
                if (walkDomain == null)
                {
                    return NotFound();
                }
                //Convert the Domain data to Dto Data and return to the client
                var walkDto = mapper.Map<WalkDto>(walkDomain);
                return Ok(walkDto);

        }

        //Delete remove the given Walk
        //DELETE ://https:localhost:1234/api/walks/{id}
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Remove([FromRoute]Guid id)
        {
            //Find the given walk From repository
            var walkDomain=await walkRepository.RemoveAsync(id);
            if(walkDomain == null)
            {
                return NotFound();
            }
            //Convert the Domain Data to Dto Data and return to the client
            var walkDto= mapper.Map<WalkDto>(walkDomain);
            return Ok(walkDto);
        }
    }
}
