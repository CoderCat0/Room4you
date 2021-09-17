using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Room4you.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Room4you.Data
{
    /// <summary>
    /// classe para recolher os dados particulares dos Utilizadores
    /// vamos deixar de usar o 'IdentityUser' e começar a usar este
    /// A adição desta classe implica:
    ///    - mudar a classe de criação da Base de Dados
    ///    - mudar no ficheiro 'startup.cs' a referência ao tipo do utilizador
    ///    - mudar em todos os ficheiros do projeto a referência a 'IdentityUser' 
    ///           para 'ApplicationUser'
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// recolhe a data de registo de um utilizador
        /// </summary>
        public DateTime DataRegisto { get; set; }
    }

    public class Proj_Context : IdentityDbContext<ApplicationUser>
    {
        // construtor da classe CriadoresCaesDB
        // indicar onde está a BD à qual estas classes (tabelas) serão associadas
        // ver o conteúdo do ficheiro 'startup.cs'
        public Proj_Context(DbContextOptions<Proj_Context> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            // insert DB seed

            //dados para definição dos 'Roles'
            modelBuilder.Entity<IdentityRole>().HasData(
               new IdentityRole { Id = "c", Name = "Cliente", NormalizedName = "CLIENTE" },
               new IdentityRole { Id = "a", Name = "Administrador", NormalizedName = "ADMINISTRADOR" }
               );

            //modelBuilder.Entity<Hoteis>().HasData(
            //    new Hoteis { Id = 1, Nome = "Hotel UM", Pais = "Portugal", Cidade = "Lisboa", Categoria = 5, Rua = "Qualquer coisa", NumQuartos = 3 },
            //    new Hoteis { Id = 2, Nome = "Hotel Dois", Pais = "Portugal", Cidade = "Lisboa", Categoria = 5, Rua = "Qualquer coisa", NumQuartos = 5 },
            //    new Hoteis { Id = 3, Nome = "Hotel tres", Pais = "Portugal", Cidade = "Lisboa", Categoria = 5, Rua = "Qualquer coisa", NumQuartos = 4 }
            //);

            //modelBuilder.Entity<Compras>().HasData(
            //    new Compras { Id = 1, Data = '20210919 10:30:00 AM', IdCliente = 1}
            //);

            //modelBuilder.Entity<Fotografias>().HasData(
            //    new Fotografias { Id = 1, Nome = "h1.jpg", HotelFK = 1 }
            //);
        }


        public DbSet<Clientes> Clientes { get; set; }
        public DbSet<Compras> Compras { get; set; }
        public DbSet<Fotografias> Fotografias { get; set; }
        public DbSet<Hoteis> Hoteis { get; set; }
        public DbSet<Quartos> Quartos { get; set; }
        public DbSet<QuartosCompra> QuartosCompra { get; set; }
        public DbSet<Room4you.Models.ReservaQuartosViewModel> ReservaQuartosViewModel { get; set; }

    }

}
