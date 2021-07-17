using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Room4you.Models
{
    public class Fotografias
    {
        /// <summary>
        /// Id da Foto
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nome do ficheiro com a fotografia do hotel
        /// </summary>
        public string Nome { get; set; }


        // criação da FK que referencia as fotos ao Hotel a que pertencem
        [ForeignKey(nameof(Hotel))]
        [Display(Name = "Hotel")]
        public int HotelFK { get; set; }
        public Hoteis Hotel { get; set; }

    }
}
