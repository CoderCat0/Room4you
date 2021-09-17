using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Room4you.Models
{
    public class Quartos
    {
        public Quartos()
        {
            ListaQuartosCompras = new HashSet<QuartosCompra>();
        }

        public ICollection<QuartosCompra> ListaQuartosCompras { get; set; }

        /// <summary>
        /// PK para a tabela do relacionamento entre Quartos e Compra
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Area do Quarto
        /// </summary> 
        [Required]
        public string Area { get; set; }

        /// <summary>
        /// Caractristicas do Quarto
        /// </summary>
        [Required]
        public string Comodidades { get; set; }

        ///// <summary>
        /////  Verifica se o quarto está ocupado
        ///// </summary>
        //[Required]
        //public bool Ocupado { get; set; }


        //****************************************

        // criação da FK que referencia o Hotel a quem o quarto pertence
        [ForeignKey(nameof(Hotel))]
        [Display(Name = "Hotel")]
        public int HotelFK { get; set; }
        public Hoteis Hotel { get; set; }

    }
}
