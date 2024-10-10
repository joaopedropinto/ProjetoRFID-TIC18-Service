using System;

namespace Cepedi.ProjetoRFID.Leitura.Domain.Entities;

/// <summary>
/// Definição do tipo "TagRfidType", o qual é utilizado para o retorno das tags RFIDs.
/// </summary>
public class TagRfidModel
{
    string epcValue = "EPC";
    string userValue = "USER";
    string antValue = "ANT";
    /// <summary>
    /// Valor do EPC
    /// </summary>
    public string EpcValue
    {
        get { return epcValue; }
        set { epcValue = value; }
    }
    /// <summary>
    /// Valor do campo User da Tag
    /// </summary>
    public string UserValue
    {
        get { return userValue; }
        set { userValue = value; }
    }
    /// <summary>
    /// Número da antena
    /// </summary>
    public string AntValue
    {
        get { return antValue; }
        set { antValue = value; }
    }
}
