namespace Cepedi.ProjetoRFID.Leitura.Domain.Entities;

/// Definição do tipo "TagRfidType", o qual é utilizado para o retorno das tags RFIDs.
public class TagRfidModel
{
    string epcValue = "EPC";
    string userValue = "USER";
    string antValue = "ANT";

    /// Valor do EPC da Tag
    public string EpcValue
    {
        get { return epcValue; }
        set { epcValue = value; }
    }

    /// Valor do campo User da Tag
    public string UserValue
    {
        get { return userValue; }
        set { userValue = value; }
    }

    /// Número da antena
    public string AntValue
    {
        get { return antValue; }
        set { antValue = value; }
    }
}
