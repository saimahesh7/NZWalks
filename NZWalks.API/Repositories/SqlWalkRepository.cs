using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SqlWalkRepository:IWalkRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SqlWalkRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Walk> CreateWalksAsync(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();

            return walk;
        }

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
                                                  string? sortBy = null, bool isAscending = true)
        {
            //var walkDomain = await dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();

            //make walkDomain as Queryable so that we can perform Filtering and sorting on the walkDomain
            var walkDomain = dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            //Filtering
            if(string.IsNullOrWhiteSpace(filterOn)==false&& string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walkDomain=walkDomain.Where(x=>x.Name.Contains(filterQuery));   
                }
            }

            //Sorting
            if(string.IsNullOrWhiteSpace(sortBy)==false)
            {
                if(sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                     walkDomain=isAscending? walkDomain.OrderBy(x=>x.Name):walkDomain.OrderByDescending(x=>x.Name);
                }
                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walkDomain = isAscending ? walkDomain.OrderBy(x => x.LengthInKm) : walkDomain.OrderByDescending(x=>x.LengthInKm);
                }
            }

            return await walkDomain.ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            var walkDomain=await dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(walk => walk.Id == id);
            if (walkDomain == null)
            {
                return null;
            }
            return walkDomain;
        }

        public async Task<Walk?> RemoveAsync(Guid id)
        {
            var walkDomain= await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if(walkDomain == null)
            {
                return null;
            }
             dbContext.Walks.Remove(walkDomain);
            await dbContext.SaveChangesAsync();
            return walkDomain;
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var walkDomain=await dbContext.Walks.FirstOrDefaultAsync(x=>x.Id == id);
            if(walkDomain == null)
            {
                return null;
            }
            walkDomain.Name = walk.Name;
            walkDomain.Description = walk.Description;
            walkDomain.LengthInKm = walk.LengthInKm;
            walkDomain.WalkImageUrl = walk.WalkImageUrl;
            walkDomain.DifficultyId = walk.DifficultyId;
            walkDomain.RegionId = walk.RegionId;

            await dbContext.SaveChangesAsync(); 
            return walkDomain;

        }
    }
}
