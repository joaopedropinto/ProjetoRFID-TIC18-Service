using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PortalRFID
{
    /// <summary>
    /// Interface das funcionalidades disponibilizadas por este serviço (Portal_RFID).
    /// </summary>
    [ServiceContract]
    public interface ILeitor
    {
        [OperationContract]
        string GetEcho(string value);

        [OperationContract]
        bool IsReaderOk(string ipPorta);


        /// <summary>
        /// Retorna uma lista com todas as tags lidas seguindo os parâmetros informados
        /// </summary>
        /// <param name="antNum">Número da antena a ser utilizada: Intervalo de 1 a 4. Caso selecione um valor fora desse intervalo, todas as antenas serão utilizadas</param>
        /// <param name="ipPorta">Ip:Porta do Reader. Ex: 172.16.10.52:8081</param>
        /// <param name="filtro">Filtro para o reader aplicar na busca das tags, ex: "52000" só vai retornar tags iniciadas com 52000.</param>
        /// <param name="tempoLeitura">Tempo que o reader deve continuar lendo as tags em milisegundos (0 a 65535)</param>
        /// <param name="readUser">True: Ler a memória de usuário das tags (Isso pode aumentar o tempo de leitura considerávelmente).</param>
        /// <param name="potenciaPadrao">Potência de Leitura padrão (0 a 3150)centi-dBm</param>
        /// <param name="potenciaAntena1">Potência de Leitura da antena 1 (0 a 3150)centi-dBm</param>
        /// <param name="potenciaAntena2">Potência de Leitura da antena 2 (0 a 3150)centi-dBm</param>
        /// <param name="potenciaAntena3">Potência de Leitura da antena 3 (0 a 3150)centi-dBm</param>
        /// <param name="potenciaAntena4">Potência de Leitura da antena 4 (0 a 3150)centi-dBm</param>
        /// <returns>Lista com as tags lidas</returns>
        [OperationContract]
        List<TagRfidType> GetTagsRfid(int antNum, string ipPorta, string filtro, int tempoLeitura, bool readUser, int potenciaPadrao, int potenciaAntena1 = 0, int potenciaAntena2 = 0, int potenciaAntena3 = 0, int potenciaAntena4 = 0);
        
        //[OperationContract]
        //List<TagRfidType> GetTagRfidFlex(int[,] antenas, string ipPorta, string filtro, int tempoLeitura, bool lerMemoriaUsuario, int potenciaPadrao);
    }

    /// <summary>
    /// Definição do tipo "TagRfidType", o qual é utilizado para o retorno das tags RFIDs.
    /// </summary>
    [DataContract]
    public class TagRfidType
    {
        string epcValue = "EPC";
        string userValue = "USER";
        string antValue = "ANT";

        [DataMember]
        public string EpcValue
        {
            get { return epcValue; }
            set { epcValue = value; }
        }

        [DataMember]
        public string UserValue
        {
            get { return userValue; }
            set { userValue = value; }
        }

        [DataMember]
        public string AntValue
        {
            get { return antValue; }
            set { antValue = value; }
        }
    }
}
