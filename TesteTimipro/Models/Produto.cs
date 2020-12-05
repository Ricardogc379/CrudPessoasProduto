using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TesteTimipro.Models
{

    [Table(name: "TabelaProduto", Schema = "dbo")]
    public class Produto : Tabela
    {

        [Display(Name = "NOME DO PRODUTO")] // NOME CAMPO
        [Required(AllowEmptyStrings = false, ErrorMessage = "O CAMPO {0} É OBRIGATÓRIO!")] // OBRIGATORIEDADE DO CAMPO
        public string Nome { get; set; }

        [Display(Name = "ATIVO")] // NOME CAMPO
        [Required(AllowEmptyStrings = false, ErrorMessage = "O CAMPO {0} É OBRIGATÓRIO!")] // OBRIGATORIEDADE DO CAMPO
        public bool Ativo { get; set; }

        public virtual ICollection<Pessoa> Pessoas { get; set; }

    }
}