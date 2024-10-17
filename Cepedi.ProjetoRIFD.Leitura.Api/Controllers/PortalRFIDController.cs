using Cepedi.ProjetoRFID.Leitura.Domain.Entities;
using Cepedi.ProjetoRFID.Leitura.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Portal_WebAPI.WebAPI.Controllers;

/// Classe de definição do serviço Portal_RFID.
[ApiController]
[Route("api/[controller]")]
public class PortalRFIDController : ControllerBase
{
    private readonly IGetTagRfidFlexService _getTagRfidFlexService;

    public PortalRFIDController(IGetTagRfidFlexService getTagRfidFlexService)
    {
        _getTagRfidFlexService = getTagRfidFlexService;
    }

    /// Sinaliza se o leitor RFID e suas antenas estão OKs.
    /// ipPorta - Ip:Porta do Reader. Ex: 172.16.10.52:8081
    /// Retorna true se o reader estiver OK, false caso contrário   
    [HttpGet]
    [Route("api/IsReaderOk")]
    public Task<bool> IsReaderOk(string ipPorta)
    {
        return _getTagRfidFlexService.IsReaderOk(ipPorta);
    }

    /// Retorna uma lista com todas as tags lidas seguindo os parâmetros informados
    /// antNum - Número da antena a ser utilizada: Intervalo de 1 a 4. Caso selecione um valor fora desse intervalo, todas as antenas serão utilizadas
    /// ipPorta - Ip:Porta do Reader. Ex: 172.16.10.52:8081
    /// tempoLeitura - Tempo que o reader deve continuar lendo as tags em milisegundos (0 a 65535)
    /// readUser - True: Ler a memória de usuário das tags (Isso pode aumentar o tempo de leitura considerávelmente).
    /// potenciaPadrao - Potência de Leitura padrão (0 a 3150)centi-dBm
    /// potenciaAntena1 - Potência de Leitura da antena 1 (0 a 3150)centi-dBm
    /// potenciaAntena2 - Potência de Leitura da antena 2 (0 a 3150)centi-dBm
    /// potenciaAntena3 - Potência de Leitura da antena 3 (0 a 3150)centi-dBm
    /// potenciaAntena4 - Potência de Leitura da antena 4 (0 a 3150)centi-dBm
    /// filtro - Filtro para o reader aplicar na busca das tags, ex: "52000" só vai retornar tags iniciadas com 52000.

    /// Retorna uma lista com as tags lidas
    [HttpGet]
    [Route("api/GetTagsRfid")]
    public async Task<IActionResult> GetTagsRfid(int antNum, string ipPorta, int tempoLeitura, bool readUser, int potenciaPadrao, int potenciaAntena1 = 0, int potenciaAntena2 = 0, int potenciaAntena3 = 0, int potenciaAntena4 = 0, string filtro = null)
    {
        int[][] antenas = null;

        #region validações
        tempoLeitura = tempoLeitura < 0 ? 0 : tempoLeitura > 65000 ? 65000 : tempoLeitura;
        potenciaPadrao = potenciaPadrao <= 0 ? 100 : potenciaPadrao > 3000 ? 3000 : potenciaPadrao;
        potenciaAntena1 = potenciaAntena1 <= 0 ? potenciaPadrao : potenciaAntena1 > 3000 ? 3000 : potenciaAntena1;
        potenciaAntena2 = potenciaAntena2 <= 0 ? potenciaPadrao : potenciaAntena2 > 3000 ? 3000 : potenciaAntena2;
        potenciaAntena3 = potenciaAntena3 <= 0 ? potenciaPadrao : potenciaAntena3 > 3000 ? 3000 : potenciaAntena3;
        potenciaAntena4 = potenciaAntena4 <= 0 ? potenciaPadrao : potenciaAntena4 > 3000 ? 3000 : potenciaAntena4;
        #endregion


        if (antNum > 4 || antNum <= 0)
        {
            antenas = new int[][] { new int[] { 1, potenciaAntena1 }, new int[] { 2, potenciaAntena2 }, new int[] { 3, potenciaAntena3 }, new int[] { 4, potenciaAntena4 } };
        }
        else
        {
            var potencia = potenciaAntena1;
            switch (antNum)
            {
                case 2:
                    potencia = potenciaAntena2;
                    break;
                case 3:
                    potencia = potenciaAntena3;
                    break;
                case 4:
                    potencia = potenciaAntena4;
                    break;
            }
            antenas = new int[][] { new int[] { antNum, potencia } };
        }
        List<TagRfidModel> tags = await _getTagRfidFlexService.GetTagRfidFlex(antenas, ipPorta, filtro, tempoLeitura, readUser, potenciaPadrao);

        return Ok(tags);
    }
}
