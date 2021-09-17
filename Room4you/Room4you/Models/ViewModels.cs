using System;
using System.ComponentModel.DataAnnotations;

namespace Room4you.Models
{
    /// <summary>
    /// ViewModel para recolher as opções dos clientes para efetuar uma reserva de quarto
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
        /// data de término da reserva do quarto
        /// </summary>
        [Required]
        public DateTime DataFim { get; set; }

    }




    public class ViewModels
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
