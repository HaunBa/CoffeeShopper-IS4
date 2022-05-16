using API.Models;

namespace API.Services
{
	public interface ICoffeeShopService
	{
		Task<List<CoffeeShopModel>> List();
		Task<CoffeeShopModel> AddCoffeeShop(CoffeeShopModel shop);
		Task<CoffeeShopModel> Get(int id);
		Task<CoffeeShopModel> Delete(int id);
	}
}
