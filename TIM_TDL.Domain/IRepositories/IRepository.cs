using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIM_TDL.Domain.Models;

namespace TIM_TDL.Domain.IRepositories
{
    public interface IRepository<T> where T : Entity
    {
        T Create(T e);
        T Update(T e);
        T Delete(T e);
        Task<T> FindByIdAsync(int id);
        Task<List<T>> FindAllAsync();
        Task SaveAsync();
        Task DisposeAsync();
    }
}
