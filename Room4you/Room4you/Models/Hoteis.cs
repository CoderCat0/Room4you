using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Room4you.Models
{
    public class Hoteis
    {
        public Hoteis()
        {
            //inicializar as listas
            ListaFotografias = new HashSet<Fotografias>();
            ListaQuartos = new HashSet<Quartos>();
        }

        /// <summary>
        /// Id do Hotel
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nome do Hotel
        /// </summary>
        [Display(Name = "Nome do Hotel")]
        public string Nome { get; set; }

        /// <summary>
        /// País onde se situa do Hotel
        /// </summary>
        [Display(Name = "País")]
        public string Pais { get; set; }


        /// <summary>
        /// Cidade onde se situa do Hotel
        /// </summary>
        public string Cidade { get; set; }

        /// <summary>
        /// Rua onde se situa do Hotel
        /// </summary>
        public string Rua { get; set; }


        /// <summary>
        /// Classificação do Hotel
        /// </summary>
        [RegularExpression("[0-5]", ErrorMessage = "Introduza uma classificaçãon de 0 a 5")]
        public int Categoria { get; set; }


        /// <summary>
        /// Numero de Quartos que o hotel tem
        /// </summary>
        [Display(Name = "Número de Quartos")]
        public int NumQuartos { get; set; }

        /*---------------------------------------------*/

        /// <summary>
        /// lista de todas as fotografias de todos os hoteis
        /// </summary>
        public ICollection<Fotografias> ListaFotografias { get; set; }

        /// <summary>
        /// lista de todos os Quartos
        /// </summary>
        public ICollection<Quartos> ListaQuartos { get; set; }

    }
}
