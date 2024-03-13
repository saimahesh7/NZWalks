using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SqlRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext dbContext;

        //Constructor Injection
        public SqlRegionRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Region> CreateRegionAsync(Region region)
        {
            await dbContext.AddAsync(region);
            await dbContext.SaveChangesAsync();

            return region;
        }

        public async Task<Region> DeleteRegionAsync(Guid id)
        {
            var region=await dbContext.Regions.FirstOrDefaultAsync(x=>x.Id==id);
            if (region==null)
            {
                return null;
            }
            else
            {
                dbContext.Regions.Remove(region);
                await dbContext.SaveChangesAsync();

                return region;
            }
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await dbContext.Regions.ToListAsync();
        }

        public async Task<Region> SingleRegion(Guid id)
        {
            var region=await dbContext.Regions.FirstOrDefaultAsync(x=>x.Id == id);
            if (region == null)
            {
                return null;
            }
            else
            {
                return region;
            }

        }

        public async Task<Region> UpdateRegionAsync(Guid id, Region region)
        {
            var existedRegion=await dbContext.Regions.FirstOrDefaultAsync(x=>x.Id==id);
            if(existedRegion == null)
            {
                return null;
            }

            existedRegion.Name= region.Name;
            existedRegion.Code= region.Code;
            existedRegion.RegionImageUrl= region.RegionImageUrl;

            await dbContext.SaveChangesAsync();
            return existedRegion;

        }
    }
}
