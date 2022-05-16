using API.Models;
using DataAccess.Data;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class CoffeeShopService : ICoffeeShopService
	{
        private readonly ApplicationDbContext dbContext;

        public CoffeeShopService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<CoffeeShopModel> Get(int id)
        {
            return await (from shop in dbContext.CoffeeShops
                          select new CoffeeShopModel()
                          {
                              Id = shop.Id,
                              Name = shop.Name,
                              OpeningHours = shop.OpeningHours,
                              Address = shop.Address
                          }).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<CoffeeShopModel> Delete(int id)
        {
            var res = await (from shop in dbContext.CoffeeShops
                   where shop.Id == id
                   select new CoffeeShopModel
                   {
                       Id = shop.Id,
                       Address = shop.Address,
                       Name = shop.Name,
                       OpeningHours = shop.OpeningHours
                   })
                   .FirstOrDefaultAsync(x => x.Id == id);
            dbContext.Remove(res);
            await dbContext.SaveChangesAsync();
            return res;
        }

        public async Task<CoffeeShopModel> AddCoffeeShop(CoffeeShopModel shopModel)
        {
            var shop = new CoffeeShop
            {
                Id = shopModel.Id,
                Address = shopModel.Address,
                Name = shopModel.Name,
                OpeningHours = shopModel.OpeningHours
            };

            dbContext.CoffeeShops.Add(shop);
            await dbContext.SaveChangesAsync();
            return shopModel;
        }

        public async Task<List<CoffeeShopModel>> List()
        {
            var coffeeShops = await (from shop in dbContext.CoffeeShops
                                     select new CoffeeShopModel()
                                     {
                                         Id = shop.Id,
                                         Name = shop.Name,
                                         OpeningHours = shop.OpeningHours,
                                         Address = shop.Address
                                     }).ToListAsync();

            return coffeeShops;
        }
    }
}
