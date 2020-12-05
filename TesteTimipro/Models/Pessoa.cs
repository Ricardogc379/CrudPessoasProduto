using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TesteTimipro.Models
{
    
    [Table(name: "TabelaPessoa", Schema = "dbo")]
    public class Pessoa : Tabela
    {
        /**
         * NOME
         **/
        [Display(Name = "NOME")] // NOME CAMPO
        [Required(AllowEmptyStrings = false, ErrorMessage = "O CAMPO {0} É OBRIGATÓRIO!")] // OBRIGATORIEDADE DO CAMPO
        [DataType(DataType.Text)] // TIPO DE CAMPO UTILIZADO PELO ENTITY PARA ESPELHAMENTO NO BANCO DE DADOS
        public string Nome { get; set; }

        /**
         * CPF
         **/
        [Display(Name = "CPF")] // NOME CAMPO
        [Required(AllowEmptyStrings = false, ErrorMessage = "O CAMPO {0} É OBRIGATÓRIO!")] // OBRIGATORIEDADE DO CAMPO
        [Remote("RemoteCPF", "Pessoas", AdditionalFields = "ID", ErrorMessage = "JÁ EXISTE UM CADASTRO ASSOCIADO A ESTE CPF")] // VERIFICAÇÃO ASSINCRONA
        [DataType(DataType.Text)] // TIPO DE CAMPO UTILIZADO PELO ENTITY PARA ESPELHAMENTO NO BANCO DE DADOS
        public string CPF { get; set; }

        /**
         * E-MAIL
         **/
        [Display(Name = "E-MAIL")] // NOME CAMPO
        [Required(AllowEmptyStrings = false, ErrorMessage = "O CAMPO {0} É OBRIGATÓRIO!")] // OBRIGATORIEDADE DO CAMPO
        [Remote("RemoteEmail", "Pessoas", AdditionalFields = "ID", ErrorMessage = "JÁ EXISTE UM CADASTRO ASSOCIADO A ESTE E-MAIL")] // VERIFICAÇÃO ASSINCRONA
        public string Email { get; set; }

        /**
         * PRODUTO
         **/
        [Display(Name = "PRODUTO")] // NOME CAMPO
        [Required(AllowEmptyStrings = false, ErrorMessage = "O CAMPO {0} É OBRIGATÓRIO!")] // OBRIGATORIEDADE DO CAMPO
        [Remote("RemoteProduto", "Pessoas", AdditionalFields = "ID", ErrorMessage = "JÁ EXISTE CLIENTE ASSOCIADO A ESTE PRODUTO")] // VERIFICAÇÃO ASSINCRONA
        [ForeignKey(name: "Produto")]
        public int ProdutoID { get; set; }


        // REFERENCIA VIRTUAL PARA USO DO LAZY LOAD DO ENTITY FRAMEWORK
        public virtual Produto Produto { get; set; }

    }

}