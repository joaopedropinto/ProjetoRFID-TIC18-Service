using System;
//using Cepedi.ProjetoRFID.Leitura.Domain.DTOs;
using Cepedi.ProjetoRFID.Leitura.Domain.Entities; 
namespace Cepedi.ProjetoRFID.Leitura.Domain.Interfaces;

public interface IRepository<T> : IDisposable where T : Entity
    {
        Task Insert(T entity);
        Task<T> GetById(Guid id);
        Task<List<T>> GetAll();
        //Task<List<T>> GetAll(Expression<Func<T, object>> order);
        Task Update(T entity);
        Task Delete(Guid id);
        // Task<IEnumerable<T>> Search(Expression<Func<T, bool>> predicate);
        // public Task<IPagedList<T>> GetPagedList(Expression<Func<T, bool>> predicate, PagedListParameters parameters);
        Task<int> SaveChanges();
    }
