using System.ComponentModel.DataAnnotations;

namespace CadastroFornecedor.Models
{
public class Fornecedor
{
    public int ID { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string CNPJ { get; set; } = string.Empty;

    public string Segmento { get; set; } = string.Empty;

    public string CEP { get; set; } = string.Empty;

    public string Endereco { get; set; } = string.Empty;

    public string ImagemUrl { get; set; } = string.Empty;
}
}