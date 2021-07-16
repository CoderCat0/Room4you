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
            ListaFotografias = new HashSet<Fotografias>();
        }

        /// <summary>
        /// Id do Hotel
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nome do Hotel
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// País onde se situa do Hotel
        /// </summary>
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
        public string Categoria { get; set; }


        /// <summary>
        /// Numero de Quartos que o hotel tem
        /// </summary>
        public int NumQuartos { get; set; }

        /*---------------------------------------------*/

        /// <summary>
        /// lista de todas as fotografias de todos os hoteis
        /// </summary>
        public ICollection<Fotografias> ListaFotografias { get; set; }

    }
}
