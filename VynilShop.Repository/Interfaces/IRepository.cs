using System;
using System.Collections.Generic;
using System.Text;
using VynilShop.Domain.DomainModels;

namespace VynilShop.Repository.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        List<T> GetAll();
        T Get(Guid? id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
