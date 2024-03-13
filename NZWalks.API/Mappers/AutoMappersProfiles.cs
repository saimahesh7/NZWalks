using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;

namespace NZWalks.API.Mappers
{
    public class AutoMappersProfiles:Profile
    {
        public AutoMappersProfiles()
        {
            CreateMap<Region,RegionDto>().ReverseMap();
            CreateMap<CreateRegionDto,Region>().ReverseMap();
            CreateMap<UpdateRegionDto, Region>().ReverseMap();

            CreateMap<Walk,WalkDto>().ReverseMap();
            CreateMap<AddWalksRequestDto,Walk>().ReverseMap();
            CreateMap<UpdateWalkDto,Walk>().ReverseMap();

            CreateMap<Difficulty,DifficultyDto>().ReverseMap();
           
        }
    }
}
