using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Room4you.Models
{
    /// <summary>
    /// ViewModel para recolher as op��es dos clientes para efetuar uma reserva de quarto
    /// </summary>
   public class ReservaQuartosViewModel
    {

        public int ID { get; set; }

        /// <summary>
        /// ID do hotel do quarto a reservar
        /// </summary>
        public int Hotel { get; set; }

        /// <summary>
        /// data de entrada no quarto
        /// </summary>
        [Required]
        public DateTime DataInicio { get; set; }

        /// <summary>
        /// data de t�rmino da reserva do quarto
        /// </summary>
        [Required]
        public DateTime DataFim { get; set; }

    }

    public class DadosReserva
    {
        //id do cliente que faz a reserva
        public int IdCliente { get; set; }

        //data de inicio da reserva
        public DateTime DataInicio { get; set; }

        //data de fim da reserva
        public DateTime DataFim { get; set; }

        //lista dos quartos associados � reserva
        public List<Quartos> Quartos { get; set; }
    }


    public class ViewModels
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
