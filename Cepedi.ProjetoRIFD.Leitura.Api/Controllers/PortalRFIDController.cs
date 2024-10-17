using Cepedi.ProjetoRFID.Leitura.Domain.Entities;
using Cepedi.ProjetoRFID.Leitura.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
// Reference the API
using ThingMagic;

namespace Portal_WebAPI.WebAPI.Controllers
{
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

        /// Retorna a mesma string recebida.
        [HttpPost]
        [Route("api/GetEcho")]
        public string GetEcho(string value)
        {
            return string.Format("{0}", value);
        }

        /// Sinaliza se o leitor RFID e suas antenas estão OKs.
        [HttpPost]
        [Route("api/IsReaderOk")]
        public bool IsReaderOk(string ipPorta)
        {
            bool readerOk = true;

            try
            {
                Reader.SetSerialTransport("tcp", SerialTransportTCP.CreateSerialReader); //Cria a nova URI “tcp”

                // Create Reader object, connecting to physical device.
                // Wrap reader in a "using" block to get automatic
                // reader shutdown (using IDisposable interface).
                using (Reader r = Reader.Create("tcp://" + ipPorta)) //usar URI “IP do leitor:Porta 8081”
                {
                    r.Connect();//conecta com o leitor.
                }
            }
            catch (ReaderException re)
            {
                Console.WriteLine("Error: " + re.Message);
                readerOk = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                readerOk = false;
            }
            return readerOk;
        }

        [HttpGet]
        public async Task<IActionResult> GetTagsAsync(int antNum, string ipPorta, string filtro, int tempoLeitura, bool readUser, int potenciaPadrao)
        {
            int[][] antenas;
            int potencia = 0;

            antenas = new int[][] { new int[] { antNum, potencia } };

            // Chama o serviço de forma assíncrona
            List<TagRfidModel> tags = await _getTagRfidFlexService.GetTagRfidFlex(antenas, ipPorta, filtro, tempoLeitura, readUser, potenciaPadrao);

            if (tags == null)
            {
                return StatusCode(500, "Error reading tags");
            }

            return Ok(tags);
        }

        /// Retorna uma lista com todas as tags lidas seguindo os parâmetros informados

        /// <param name="antNum">Número da antena a ser utilizada: Intervalo de 1 a 4. Caso selecione um valor fora desse intervalo, todas as antenas serão utilizadas</param>
        /// <param name="ipPorta">Ip:Porta do Reader. Ex: 172.16.10.52:8081</param>
        /// <param name="tempoLeitura">Tempo que o reader deve continuar lendo as tags em milisegundos (0 a 65535)</param>
        /// <param name="readUser">True: Ler a memória de usuário das tags (Isso pode aumentar o tempo de leitura considerávelmente).</param>
        /// <param name="potenciaPadrao">Potência de Leitura padrão (0 a 3150)centi-dBm</param>
        /// <param name="potenciaAntena1">Potência de Leitura da antena 1 (0 a 3150)centi-dBm</param>
        /// <param name="potenciaAntena2">Potência de Leitura da antena 2 (0 a 3150)centi-dBm</param>
        /// <param name="potenciaAntena3">Potência de Leitura da antena 3 (0 a 3150)centi-dBm</param>
        /// <param name="potenciaAntena4">Potência de Leitura da antena 4 (0 a 3150)centi-dBm</param>
        /// <param name="filtro">Filtro para o reader aplicar na busca das tags, ex: "52000" só vai retornar tags iniciadas com 52000.</param>
        /// <returns>Lista com as tags lidas</returns>
        // [HttpPost]
        // [Route("api/GetTagsRfid")]
        // public List<TagRfidModel> GetTagsRfid(int antNum, string ipPorta, int tempoLeitura, bool readUser, int potenciaPadrao, int potenciaAntena1 = 0, int potenciaAntena2 = 0, int potenciaAntena3 = 0, int potenciaAntena4 = 0, string filtro = null)
        // {
        //     int[][] antenas = null;

        //     #region validações
        //     tempoLeitura = tempoLeitura < 0 ? 0 : tempoLeitura > 65000 ? 65000 : tempoLeitura;
        //     potenciaPadrao = potenciaPadrao <= 0 ? 100 : potenciaPadrao > 3000 ? 3000 : potenciaPadrao;
        //     potenciaAntena1 = potenciaAntena1 <= 0 ? potenciaPadrao : potenciaAntena1 > 3000 ? 3000 : potenciaAntena1;
        //     potenciaAntena2 = potenciaAntena2 <= 0 ? potenciaPadrao : potenciaAntena2 > 3000 ? 3000 : potenciaAntena2;
        //     potenciaAntena3 = potenciaAntena3 <= 0 ? potenciaPadrao : potenciaAntena3 > 3000 ? 3000 : potenciaAntena3;
        //     potenciaAntena4 = potenciaAntena4 <= 0 ? potenciaPadrao : potenciaAntena4 > 3000 ? 3000 : potenciaAntena4;
        //     #endregion


        //     if (antNum > 4 || antNum <= 0)
        //     {
        //         antenas = new int[][] { new int[] { 1, potenciaAntena1 }, new int[] { 2, potenciaAntena2 }, new int[] { 3, potenciaAntena3 }, new int[] { 4, potenciaAntena4 } };
        //     }
        //     else
        //     {
        //         var potencia = potenciaAntena1;
        //         switch (antNum)
        //         {
        //             case 2:
        //                 potencia = potenciaAntena2;
        //                 break;
        //             case 3:
        //                 potencia = potenciaAntena3;
        //                 break;
        //             case 4:
        //                 potencia = potenciaAntena4;
        //                 break;
        //         }
        //         antenas = new int[][] { new int[] { antNum, potencia } };
        //     }
        //     return GetTagRfidFlex(antenas, ipPorta, filtro, tempoLeitura, readUser, potenciaPadrao);
        // }

        // private List<TagRfidModel> GetTagRfidFlex(int[][] antenas, string ipPorta, string filtro, int tempoLeitura, bool lerMemoriaUsuario, int potenciaPadrao)
        // {
        //     List<TagRfidModel> tags = new List<TagRfidModel>(); // Instancia a lista de tags.
        //     int[] antennaList = null;
        //     string str = "";

        //     for (int i = 0; i < antenas.GetLength(0); i++)
        //     {
        //         str += antenas[i][0];
        //         if (antenas.GetLength(0) > 1 && i != antenas.GetLength(0) - 1)
        //             str += ",";
        //     }

        //     antennaList = Array.ConvertAll<string, int>(str.Split(','), int.Parse);

        //     try
        //     {
        //         Reader.SetSerialTransport("tcp", SerialTransportTCP.CreateSerialReader); //Cria a nova URI “tcp”

        //         // Create Reader object, connecting to physical device.
        //         // Wrap reader in a "using" block to get automatic
        //         // reader shutdown (using IDisposable interface).
        //         using (Reader r = Reader.Create("tcp://" + ipPorta)) //usar URI “IP do leitor:Porta 8081”
        //         {
        //             r.Connect();//conecta com o leitor.

        //             if (Reader.Region.UNSPEC == (Reader.Region)r.ParamGet("/reader/region/id"))
        //             {
        //                 Reader.Region[] supportedRegions = (Reader.Region[])r.ParamGet("/reader/region/supportedRegions");
        //                 if (supportedRegions.Length < 1)
        //                 {
        //                     throw new FAULT_INVALID_REGION_Exception();
        //                 }
        //                 r.ParamSet("/reader/region/id", supportedRegions[0]);
        //             }

        //             // Read Plan
        //             byte length;
        //             string model = r.ParamGet("/reader/version/model").ToString();
        //             if ((model.Equals("M6e Micro") || model.Equals("M6e Nano")) && antennaList == null)
        //             {
        //                 Console.WriteLine("Module doesn't has antenna detection support please provide antenna list");
        //             }
        //             if ("M6e".Equals(model)
        //                         || "M6e PRC".Equals(model)
        //                         || "M6e Micro".Equals(model)
        //                         || "Mercury6".Equals(model)
        //                         || "Astra-EX".Equals(model))
        //             {
        //                 // Specifying the readLength = 0 will return full TID for any tag read in case of M6e varients, M6 and Astra-EX reader.
        //                 length = 0;
        //             }
        //             else
        //             {
        //                 length = 2;
        //             }

        //             // Create a simplereadplan which uses the antenna list created above
        //             string filterTeste = filtro ?? string.Empty;
        //             SimpleReadPlan plan = new SimpleReadPlan(antennaList, TagProtocol.GEN2, new TagData(filterTeste), null, 1000);

        //             // Configura para conseguir ler a tag que causou problemas de leitura durante os testes.
        //             r.ParamSet("/reader/gen2/tagEncoding", Gen2.TagEncoding.FM0);
        //             r.ParamSet("/reader/gen2/session", Gen2.Session.S1);
        //             r.ParamSet("/reader/gen2/target", Gen2.Target.A);
        //             // Configura a potência de leitura (máximo: 3.000).
        //             r.ParamSet("/reader/radio/readPower", potenciaPadrao);
        //             r.ParamSet("/reader/radio/portReadPowerList", antenas);

        //             // Set the created readplan
        //             r.ParamSet("/reader/read/plan", plan);

        //             // Read tags
        //             TagReadData[] tagReads = r.Read(tempoLeitura);

        //             if (tagReads.Length == 0)
        //             {
        //                 Console.WriteLine("No tags found");
        //             }

        //             foreach (TagReadData tagLida in tagReads)
        //             {
        //                 TagRfidModel tag = new TagRfidModel();
        //                 tag.EpcValue = tagLida.EpcString;

        //                 if (lerMemoriaUsuario) // É necessário ler a memória User?
        //                 {
        //                     TagFilter filter = new TagData(tagLida.EpcString);
        //                     TagOp op = new Gen2.ReadData(Gen2.Bank.USER, 0, length);
        //                     TagReadData[] tagReadUser = null;
        //                     SimpleReadPlan planUser = new SimpleReadPlan(antennaList, TagProtocol.GEN2, filter, op, 1000);
        //                     r.ParamSet("/reader/read/plan", planUser);
        //                     tagReadUser = r.Read(tempoLeitura / 2);

        //                     if (tagReadUser.Length > 0)
        //                     {
        //                         tag.UserValue = ByteFormat.ToHex(tagReadUser[0].Data, "", "");
        //                     }
        //                     else
        //                     {
        //                         tag.UserValue = "";
        //                     }
        //                 }
        //                 else
        //                 {
        //                     tag.UserValue = "";
        //                 }

        //                 tag.AntValue = tagLida.Antenna.ToString();
        //                 tags.Add(tag);
        //             }
        //         }
        //     }
        //     catch (ReaderException re)
        //     {
        //         Console.WriteLine("Error123: " + re.Message);
        //         tags = null;
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine("Error321: " + ex.Message);
        //         tags = null;
        //     }

        //     return tags;
        // }

        //Endpoint para receber os parâmetros via QueryString para facilitar o teste.
        // [HttpPost]
        // [Route("api/GetTagsRfidFromQueryString")]
        // public List<TagRfidModel> GetTagsRfidFromQueryString(string queryString)
        // {
        //     var queryParams = System.Web.HttpUtility.ParseQueryString(queryString);

        //     int antNum = int.Parse(queryParams["antNum"]);
        //     string ipPorta = queryParams["ipPorta"];
        //     int tempoLeitura = int.Parse(queryParams["tempoLeitura"]);
        //     bool readUser = bool.Parse(queryParams["readUser"]);
        //     int potenciaPadrao = int.Parse(queryParams["potenciaPadrao"]);
        //     int potenciaAntena1 = queryParams["potenciaAntena1"] != null ? int.Parse(queryParams["potenciaAntena1"]) : 0;
        //     int potenciaAntena2 = queryParams["potenciaAntena2"] != null ? int.Parse(queryParams["potenciaAntena2"]) : 0;
        //     int potenciaAntena3 = queryParams["potenciaAntena3"] != null ? int.Parse(queryParams["potenciaAntena3"]) : 0;
        //     int potenciaAntena4 = queryParams["potenciaAntena4"] != null ? int.Parse(queryParams["potenciaAntena4"]) : 0;
        //     string filtro = queryParams["filtro"];

        //     return GetTagsRfid(antNum, ipPorta, tempoLeitura, readUser, potenciaPadrao, potenciaAntena1, potenciaAntena2, potenciaAntena3, potenciaAntena4, filtro);
        // }

    }
}
