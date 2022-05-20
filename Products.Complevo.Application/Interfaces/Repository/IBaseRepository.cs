using Products.Complevo.Domain.Models;
using System;
using System.Threading.Tasks;

namespace Products.Complevo.Application.Core.Interfaces.Repository
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> GetByIdAsync(Guid id);
        Task<TEntity> GetByIdWithTrackingAsync(Guid id);

        Task<int> DeleteAsync(TEntity entity);
        Task<TEntity> InsertAsync(TEntity entity);

        Task<int> SaveChangesAsync();
    }
}
