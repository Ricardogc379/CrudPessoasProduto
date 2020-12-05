using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;

namespace TesteTimipro.Models
{
    public abstract class Tabela
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "DELETADO")]
        [ScaffoldColumn(scaffold: false)]
        [DataType(DataType.DateTime)]
        public bool Deletado { get; set; }

        [Display(Name = "DATA DO CADASTRO")]
        [ScaffoldColumn(scaffold: false)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.DateTime)]
        public DateTime DataCadastro { get; set; }

        [Display(Name = "DATA DA EXCLUSÃO")]
        [ScaffoldColumn(scaffold: false)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.DateTime)]
        public DateTime? DataExclusao { get; set; }

        [Display(Name = "RESPONSÁVEL PELO CADASTRO")]
        [ScaffoldColumn(scaffold: false)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "O CAMPO {0} É OBRIGATÓRIO!")]
        public int CadastroLogID { get; set; }

        [Display(Name = "RESPONSÁVEL PELA EXCLUSÃO")]
        [ScaffoldColumn(scaffold: false)]
        public int? ExclusaoLogID { get; set; }

        public void ChangeProperties<T>(object Item)
        {
            T obj = (T)Item;
            PropertyInfo[] properties = this.GetType().GetProperties();
            PropertyInfo[] iteracao = obj.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                iteracao.Where(x => x.Name == property.Name).FirstOrDefault();
                foreach (PropertyInfo x in iteracao)
                {
                    if (x.Name == "ID" || x.Name == "Deletado" || x.Name == "DataCadastro" ||
                        x.Name == "DataExclusao" || x.Name == "CadastroLogID" || x.Name == "ExclusaoLogID")
                    {

                    }
                    else
                    {
                        if (x.Name == property.Name)
                        {
                            property.SetValue(this, x.GetValue(obj));
                        }
                    }
                }
            }

        }
    }
}