using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Room4you.Models
{
    public class QuartosCompra
    {
        /// <summary>
        /// PK para a tabela do relacionamento entre Quartos e Compra
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Data de entrada associada à Compra
        /// </summary>
        public DateTime DataEntrada { get; set; }

        /// <summary>
        /// Data de saída associada à Compra
        /// </summary>
        public DateTime DataSaida { get; set; }

        /// <summary>
        /// Número de pessoas à qual a compra está associada
        /// </summary>
        public int NumPessoas { get; set; }

        /*---------------------------------------------*/

        /// <summary>
        /// FK para o Quarto
        /// </summary>
        [ForeignKey(nameof(IdQuarto))]
        public int IdQuartoFK { get; set; }
        public Quartos IdQuarto { get; set; }


        /// <summary>
        /// FK para a Compra
        /// </summary>
        [ForeignKey(nameof(IdCompra))]
        public int IdCompraFK { get; set; }
        public Compras IdCompra { get; set; }

    }
}
