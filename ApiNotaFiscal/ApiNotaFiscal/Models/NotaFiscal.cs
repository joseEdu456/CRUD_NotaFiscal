using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ApiNotaFiscal.Models;

public class NotaFiscal
{
    public int Id { get; set; }
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "O campo {0} deve ser maior que 0")]
    public int Numero { get; set; }
    public DateTime DataEmissao { get; private set; }
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "O campo {0} deve ser maior que 0")]
    public decimal Valor { get; set; }
    
    public NotaFiscal() {DataEmissao = DateTime.Now;}
}