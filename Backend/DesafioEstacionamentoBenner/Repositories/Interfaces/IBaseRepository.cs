using Infrastructure.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DesafioEstacionamentoBenner.Repositories.Interfaces;

public interface IBaseRepository<T> where T : class, IBase
{
    Task<List<T>> GetAllAsync();

    Task<T> GetByIdAsync(long id);

    Task<T> PostAsync(T entity);

    Task<T> PutAsync(T entity);

    Task<T> DeleteAsync(long id);

    DbSet<T> GetDbSet();
}
