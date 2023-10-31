using System.Text.RegularExpressions;

var numeroNotaFiscalAtual = "7777";
var descricaoSemNota = "VALOR DISP.  P/ PROXIMO NF. REF. PROTOCOLO 1986200";
var descricaoCorretaUmaNota = "VALOR DISP.  NF 123456. REF. PROTOCOLO 1986200";
var descricaoAtualDuasNotas = "VALOR DISP.  NF 123456, 9999 REF. PROTOCOLO 1986200";
var cincoNotasMesmaDescricao = "VALOR DISP.  NF 85255. REF. VALOR DISP.  NF 85194. REF. VALOR DISP.  NF 85117. REF. VALOR DISP.  NF 85052. REF. VALOR DISP.  NF 84994. REF. VALOR DISP.  P/ PROXIMO NF. REF. PROTOCOLO 1986200";

Console.WriteLine("---------------INICIO DOS TESTES--------------------");
Console.WriteLine("\n");

SanitizaDescricao.ImprimeVersaoOriginalESanitizada(descricaoSemNota, numeroNotaFiscalAtual);
SanitizaDescricao.ImprimeVersaoOriginalESanitizada(descricaoCorretaUmaNota, numeroNotaFiscalAtual);
SanitizaDescricao.ImprimeVersaoOriginalESanitizada(descricaoAtualDuasNotas, numeroNotaFiscalAtual);
SanitizaDescricao.ImprimeVersaoOriginalESanitizada(cincoNotasMesmaDescricao, numeroNotaFiscalAtual);


public static class Patterns
{
    public const string NUMERO_PROTOCOLO_PATTERN = @"PROTOCOLO\s(\d+)";
    public const string NUMERO_NF_PATTERN = @"NF (\d+(?:,\s\d+)*)";
}

public static class SanitizaDescricao
{
    private const string _PREFIXO_NF_PADRAO = "VALOR DISP. NF ";
    private const string _PREFIXO_PROTOCOLO = " REF. PROTOCOLO ";

    public static void ImprimeVersaoOriginalESanitizada(string descricao, string numeroNotaFiscalAtual)
    {
        Console.WriteLine("Aqui esta a descrição sem a sanitização: ");
        Console.Write(nameof(descricao) + ": " + descricao);
        Console.WriteLine("\n");
        Console.WriteLine("Aqui esta a descrição após a sanitização: ");
        Console.Write(nameof(descricao) + ": " + SanitizaDescricao.SanitizarNotaFiscal(descricao, numeroNotaFiscalAtual));
        Console.WriteLine("\n");
    }
    public static string SanitizarNotaFiscal(string descricaoASerSanitizada, string numeroNotaFiscalAtual)
    {
        var msgSaida = string.Empty;
        var separador = ", ";
        var nfs = Regex.Matches(descricaoASerSanitizada, Patterns.NUMERO_NF_PATTERN);
        var protocolo = Regex.Match(descricaoASerSanitizada, Patterns.NUMERO_PROTOCOLO_PATTERN);

        var nfsExtraidas = nfs.Cast<Match>().Select(m => m.Groups[1].Value);
        var protocoloExtraido = protocolo.Success ? _PREFIXO_PROTOCOLO + protocolo.Groups[1].Value : string.Empty;

        if (nfsExtraidas.Any())
        {
            msgSaida = _PREFIXO_NF_PADRAO + string.Join(separador, nfsExtraidas) + separador + numeroNotaFiscalAtual;

            if (!string.IsNullOrEmpty(protocoloExtraido))
            {
                msgSaida += protocoloExtraido;
            }
        }
        else
        {
            msgSaida = _PREFIXO_NF_PADRAO + numeroNotaFiscalAtual;
            if (!string.IsNullOrEmpty(protocoloExtraido))
            {
                msgSaida += protocoloExtraido;
            }
        }

        return msgSaida;
    }

}





