using Microsoft.EntityFrameworkCore;
using MyShop.Domain.Entities;
using MyShop.Domain.Repositories.Interfaces;

namespace MyShop.Domain.Repositories.EntityFramework
{
	public class EFCarInfoRepository : ICarInfoRepository
	{
		private readonly AppDbContext context;

		public EFCarInfoRepository(AppDbContext appDbContext)
		{
			context = appDbContext;
		}

		public void DeleteCarInfo(Guid id)
		{
			context.CarInfo.Remove(new CarInfo() { Id = id });
			context.SaveChanges();
		}


		public IQueryable<CarInfo> GetCarInfo()
		{
			return context.CarInfo;
		}

		public CarInfo? GetCarInfoById(Guid id)
		{
			return context.CarInfo.FirstOrDefault(c => c.Id == id);
		}

		public void SaveCarInfo(CarInfo entity)
		{
			if (entity.Id == default)
				context.Entry(entity).State = EntityState.Added;
			else
				context.Entry(entity).State = EntityState.Modified;
			context.SaveChanges();
		}

		public void SetQuantity(Guid id, int quantity)
		{
			if(quantity >= 0 & quantity <= 100)
			{
				context.CarInfo.FirstOrDefault(c => c.Id == id).Quantity = quantity;
			}
			context.SaveChanges();
		}
	}
}
