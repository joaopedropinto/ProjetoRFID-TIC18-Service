using System;
using Cepedi.ProjetoRFID.Leitura.Domain.Entities;

namespace Cepedi.ProjetoRFID.Leitura.Domain.Interfaces;

public interface IGetTagRfidFlexService : IDisposable
{
    Task<List<TagRfidModel>> GetTagRfidFlex(int[][] antenas, string ipPorta, string filtro, int tempoLeitura, bool lerMemoriaUsuario, int potenciaPadrao);
}
