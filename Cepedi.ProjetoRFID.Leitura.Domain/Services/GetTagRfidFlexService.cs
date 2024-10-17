using Cepedi.ProjetoRFID.Leitura.Domain.Entities;
using Cepedi.ProjetoRFID.Leitura.Domain.Interfaces;
using ThingMagic;

namespace Cepedi.ProjetoRFID.Leitura.Domain.Services;

public abstract class GetTagRfidFlexService : IGetTagRfidFlexService
{
    public GetTagRfidFlexService()
    {
    }

    public Task<List<TagRfidModel>> GetTagRfidFlex(int[][] antenas, string ipPorta, string filtro, int tempoLeitura, bool lerMemoriaUsuario, int potenciaPadrao)
    {
        List<TagRfidModel> tags = new List<TagRfidModel>(); // Instancia a lista de tags.
        int[] antennaList = null;
        string str = "";

        for (int i = 0; i < antenas.GetLength(0); i++)
        {
            str += antenas[i][0];
            if (antenas.GetLength(0) > 1 && i != antenas.GetLength(0) - 1)
                str += ",";
        }

        antennaList = Array.ConvertAll<string, int>(str.Split(','), int.Parse);

        try
        {
            Reader.SetSerialTransport("tcp", SerialTransportTCP.CreateSerialReader); //Cria a nova URI “tcp”

            // Create Reader object, connecting to physical device.
            // Wrap reader in a "using" block to get automatic
            // reader shutdown (using IDisposable interface).
            using (Reader r = Reader.Create("tcp://" + ipPorta)) //usar URI “IP do leitor:Porta 8081”
            {
                r.Connect();//conecta com o leitor.

                if (Reader.Region.UNSPEC == (Reader.Region)r.ParamGet("/reader/region/id"))
                {
                    Reader.Region[] supportedRegions = (Reader.Region[])r.ParamGet("/reader/region/supportedRegions");
                    if (supportedRegions.Length < 1)
                    {
                        throw new FAULT_INVALID_REGION_Exception();
                    }
                    r.ParamSet("/reader/region/id", supportedRegions[0]);
                }

                // Read Plan
                byte length;
                string model = r.ParamGet("/reader/version/model").ToString();
                if ((model.Equals("M6e Micro") || model.Equals("M6e Nano")) && antennaList == null)
                {
                    Console.WriteLine("Module doesn't has antenna detection support please provide antenna list");
                }
                if ("M6e".Equals(model)
                            || "M6e PRC".Equals(model)
                            || "M6e Micro".Equals(model)
                            || "Mercury6".Equals(model)
                            || "Astra-EX".Equals(model))
                {
                    // Specifying the readLength = 0 will return full TID for any tag read in case of M6e varients, M6 and Astra-EX reader.
                    length = 0;
                }
                else
                {
                    length = 2;
                }

                // Create a simplereadplan which uses the antenna list created above
                string filterTeste = filtro ?? string.Empty;
                SimpleReadPlan plan = new SimpleReadPlan(antennaList, TagProtocol.GEN2, new TagData(filterTeste), null, 1000);

                // Configura para conseguir ler a tag que causou problemas de leitura durante os testes.
                r.ParamSet("/reader/gen2/tagEncoding", Gen2.TagEncoding.FM0);
                r.ParamSet("/reader/gen2/session", Gen2.Session.S1);
                r.ParamSet("/reader/gen2/target", Gen2.Target.A);
                // Configura a potência de leitura (máximo: 3.000).
                r.ParamSet("/reader/radio/readPower", potenciaPadrao);
                r.ParamSet("/reader/radio/portReadPowerList", antenas);

                // Set the created readplan
                r.ParamSet("/reader/read/plan", plan);

                // Read tags
                TagReadData[] tagReads = r.Read(tempoLeitura);

                if (tagReads.Length == 0)
                {
                    Console.WriteLine("No tags found");
                }

                foreach (TagReadData tagLida in tagReads)
                {
                    TagRfidModel tag = new TagRfidModel();
                    tag.EpcValue = tagLida.EpcString;

                    if (lerMemoriaUsuario) // É necessário ler a memória User?
                    {
                        TagFilter filter = new TagData(tagLida.EpcString);
                        TagOp op = new Gen2.ReadData(Gen2.Bank.USER, 0, length);
                        TagReadData[] tagReadUser = null;
                        SimpleReadPlan planUser = new SimpleReadPlan(antennaList, TagProtocol.GEN2, filter, op, 1000);
                        r.ParamSet("/reader/read/plan", planUser);
                        tagReadUser = r.Read(tempoLeitura / 2);

                        if (tagReadUser.Length > 0)
                        {
                            tag.UserValue = ByteFormat.ToHex(tagReadUser[0].Data, "", "");
                        }
                        else
                        {
                            tag.UserValue = "";
                        }
                    }
                    else
                    {
                        tag.UserValue = "";
                    }

                    tag.AntValue = tagLida.Antenna.ToString();
                    tags.Add(tag);
                }
            }
        }
        catch (ReaderException re)
        {
            Console.WriteLine("Error123: " + re.Message);
            return Task.FromResult<List<TagRfidModel>>(null);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error321: " + ex.Message);
            return Task.FromResult<List<TagRfidModel>>(null);
        }

        return Task.FromResult(tags);
    }
}
