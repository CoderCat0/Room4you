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
        /// <summary>
        /// Id do Hotel
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Cidade onde se situa do Hotel
        /// </summary>
        public string Pais { get; set; }


        /// <summary>
        /// Cidade onde se situa do Hotel
        /// </summary>
        public string Cidade { get; set; }

        /// <summary>
        /// Cidade onde se situa do Hotel
        /// </summary>
        public string Rua { get; set; }


        /// <summary>
        /// Cidade onde se situa do Hotel
        /// </summary>
        public string Categoria { get; set; }


        /// <summary>
        /// Numero de Quartos que o hotel tem
        /// </summary>
        public int NumQuartos { get; set; }

        /*---------------------------------------------*/

        /// <summary>
        /// FK para o ID do Quarto
        /// </summary>
        [ForeignKey(nameof(IdFoto))]
        public int IdFotoFK { get; set; }
        public Clientes IdFoto { get; set; }

    }
}
