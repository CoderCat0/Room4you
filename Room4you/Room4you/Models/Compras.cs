using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Room4you.Models
{
    public class Compras
    {
        public Compras()
        {
            // inicializar a lista 
            ListaComprasQuartos = new HashSet<QuartosCompra>();
        }

        /// <summary>
        /// Id da Compra
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Data da Compra
        /// </summary>
        public DateTime Data { get; set; }

        /*---------------------------------------------*/

        /// <summary>
        /// FK para o ID do Cliente
        /// </summary>
        [ForeignKey(nameof(IdCliente))]
        public int IdClienteFK { get; set; }
        public Clientes IdCliente { get; set; }

        /// <summary>
        /// declarar a lista entre Compras e QuartosCompra
        /// </summary>
        public ICollection<QuartosCompra> ListaComprasQuartos { get; set; }

    }
}
