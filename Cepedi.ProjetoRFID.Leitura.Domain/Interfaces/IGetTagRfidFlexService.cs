using System;
using Cepedi.ProjetoRFID.Leitura.Domain.Entities;

namespace Cepedi.ProjetoRFID.Leitura.Domain.Interfaces;

public interface IGetTagRfidFlexService
{
    Task<List<TagRfidModel>> GetTagRfidFlex(int[][] antenas, string ipPorta, string filtro, int tempoLeitura, bool lerMemoriaUsuario, int potenciaPadrao);
    Task<List<TagRfidModel>> GetTagRfidFlexAsync(int[][] antenas, string ipPorta, string filtro, int tempoLeitura, bool lerMemoriaUsuario, int potenciaPadrao);
    Task<bool> IsReaderOk(string ipPorta);
    Task<string> GetEcho(string value);

    // Serviço de leitura em tempo real retornando todas as tags lidas no console log.
    // Task<List<TagRfidModel>> GetTagRfidFlexAsyncLogging(int[][] antenas, string ipPorta, string filtro, int tempoLeitura, bool lerMemoriaUsuario, int potenciaPadrao);
}
