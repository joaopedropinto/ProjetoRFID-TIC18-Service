using System;
using Cepedi.ProjetoRFID.Leitura.Domain.Interfaces;
using Cepedi.ProjetoRFID.Leitura.Domain.Entities;

namespace Cepedi.ProjetoRFID.Leitura.Domain.Services;

public class TagRfidModelService : BaseService<TagRfidModel>, ITagRfidModelService
{
    private readonly ITagRfidModelRepository _TagRfidModelrepository;

    public TagRfidModelService(INotifier notifier, ITagRfidModelRepository TagRfidModelrepository) : base(notifier, TagRfidModelrepository)
    {
        _TagRfidModelrepository = TagRfidModelrepository;
    }

    public async Task
}
