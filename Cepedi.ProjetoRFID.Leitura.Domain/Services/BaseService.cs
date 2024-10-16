using Cepedi.ProjetoRFID.Leitura.Domain.Interfaces;
using Cepedi.ProjetoRFID.Leitura.Domain.Entities;
using Cepedi.ProjetoRFID.Leitura.Domain.Notifications;
using FluentValidation;
using FluentValidation.Results;


namespace Cepedi.ProjetoRFID.Leitura.Domain.Services;

public abstract class BaseService<T> : IService<T> where T : Entity
{
    private readonly INotifier _notifier;
    private readonly IRepository<T> _repository;
    protected BaseService(INotifier notifier, IRepository<T> repository)
    {
        _notifier = notifier;
        _repository = repository;
    }

    protected void Notify(ValidationResult validationResult)
    {
        foreach (var error in validationResult.Errors)
        {
            Notify(error.ErrorMessage);
        }
    }

    protected void Notify(string mensagem)
    {
        _notifier.Handle(new Notification(mensagem));
    }

    protected bool RunValidation<TV, TE>(TV validacao, TE entidade) where TV : AbstractValidator<TE> where TE : Entity
    {
        var validator = validacao.Validate(entidade);

        if (validator.IsValid) return true;

        Notify(validator);

        return false;
    }

    public virtual Task<T> GetById(Guid id)
    {
        return _repository.GetById(id);
    }

    public virtual Task<List<T>> GetAll()
    {
        return _repository.GetAll();
    }

    //public virtual Task Adicionar(T entity)
    //{
    //    return _repository.Adicionar(entity);
    //}

    //public virtual Task Atualizar(T entity)
    //{
    //    return _repository.Atualizar(entity);
    //}

    //public virtual Task Remover(Guid id)
    //{
    //    return _repository.Remover(id);
    //}

    //public virtual Task<IEnumerable<T>> Buscar(Expression<Func<T, bool>> predicate)
    //{
    //    return _repository.Buscar(predicate);
    //}

    //public virtual Task<int> SaveChanges()
    //{
    //    return _repository.SaveChanges();
    //}

    public virtual void Dispose()
    {
        _repository.Dispose();
    }
}
