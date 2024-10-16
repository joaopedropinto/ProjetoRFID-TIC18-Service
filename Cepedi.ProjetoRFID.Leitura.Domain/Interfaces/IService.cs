
using Cepedi.ProjetoRFID.Leitura.Domain.Entities;
namespace Cepedi.ProjetoRFID.Leitura.Domain.Interfaces;

public interface IService<T> : IDisposable where T : Entity
    {
        //Task Adicionar(T entity);
        Task<T> GetById(Guid id);
        Task<List<T>> GetAll();
        //Task Atualizar(T entity);
        //Task Remover(Guid id);
        //Task<IEnumerable<T>> Buscar(Expression<Func<T, bool>> predicate);
        //Task<int> SaveChanges();
    }