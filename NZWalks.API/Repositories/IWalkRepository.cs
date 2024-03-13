using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        Task<List<Walk>> GetAllAsync(string? filterOn=null,string? filterQuery=null,
                                     string? sortBy=null,bool isAscending=true);
        Task<Walk?> GetByIdAsync(Guid id);
        Task<Walk> CreateWalksAsync(Walk walk);
        Task<Walk?> UpdateAsync(Guid id, Walk walk);
        Task<Walk?> RemoveAsync(Guid id);
    }
}
