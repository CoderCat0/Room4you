using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public int Area { get; set; }

        /// <summary>
        /// Caractristicas do Quarto
        /// </summary>
        public string Comodidades { get; set; }

    }
}
