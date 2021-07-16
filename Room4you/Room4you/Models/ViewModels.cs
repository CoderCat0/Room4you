using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Room4you.Models
{
    public class ViewModels
    {
        /// <summary>
        /// classe para permitir o transporte do Controller para a View, e vice-versa
        /// irá transportar os dados das Fotografias e dos IDs dos hoteis que pertencem 
        /// </summary>
        public class FotosHoteis
        {
            /// <summary>
            /// lista de todas as fotografias de todos os hoteis
            /// </summary>
            public ICollection<Fotografias> ListaFotografias { get; set; }

            /// <summary>
            /// lista dos IDs das fotos que pertencem a um hotel
            /// </summary>
            public ICollection<int> ListaFotosHoteis { get; set; }

        }

    }
}
