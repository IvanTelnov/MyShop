using MyShop.Domain.Entities;

namespace MyShop.Domain.Repositories.Interfaces
{
	public interface ICarInfoRepository
	{
		IQueryable<CarInfo> GetCarInfo();
		CarInfo? GetCarInfoById(Guid id);
		void SaveCarInfo(CarInfo entity);
		void SetQuantity(Guid id, int quantity);
		void DeleteCarInfo(Guid id);
	}
}
