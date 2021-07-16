using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Room4you.Models
{
    public class Clientes
    {
        public Clientes()
        {
            ListaCompras = new HashSet<Compras>();
        }

        /// <summary>
        /// Id para o Cliente
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nome do Cliente
        /// </summary>
        [Required(ErrorMessage = "O Nome é de preenchimento obrigatório")]
        [StringLength(60, ErrorMessage = "O {0} não pode ter mais de {1} caracteres.")]
        public string Nome { get; set; }

        /// <summary>
        /// Email do Cliente
        /// </summary>
        [Required(ErrorMessage = "O E-Mail é de preenchimento obrigatório")]
        [RegularExpression("((((aluno)|(es((tt)|(ta)|(gt))))[0-9]{4,5})|([a-z]+(.[a-z]+)*))@ipt.pt",
                           ErrorMessage = "Só são aceites emails do IPT.")]
        public string Email { get; set; }

        /// <summary>
        /// Nacionalidade do Cliente
        /// </summary>
        [Required(ErrorMessage = "A Nacionalidade é de preenchimento obrigatório")]
        public string Nacionalidade { get; set; }

        /// <summary>
        /// NIF do Cliente
        /// </summary>
        [Required(ErrorMessage = "O NIF é de preenchimento obrigatório")]
        public string Nif { get; set; }

        /// <summary>
        /// Data de Nascimento do cliente
        /// </summary>
        [Required(ErrorMessage = "A data de nascimento é de preenchimento obrigatório")]
        public DateTime DataNasc { get; set; }

        /// <summary>
        /// Sexo do Cliente
        /// </summary>
        [Required(ErrorMessage = "O Sexo é de preenchimento obrigatório")]
        public string Sexo { get; set; }

        //************************************************************************************
        /// <summary>
        /// Funciona como Chave Forasteira no relacionamento entre os Clientes
        /// e a tabela de autenticação
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// lista de todas as fotografias de todos os hoteis
        /// </summary>
        public ICollection<Compras> ListaCompras { get; set; }

        // ############################################

        // criação da FK que referencia o Cão a quem a Foto pertence
        [ForeignKey(nameof(Compra))]
        [Display(Name = "Compra")]
        public int CompraFK { get; set; }
        public Compras Compra { get; set; }

    }
}
