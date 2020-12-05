using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TesteTimipro.Models
{
    public class TimiproDB : DbContext
    {

        public DbSet<Pessoa> TabelaPessoas { get; set; }

        public DbSet<Produto> TabelaProdutos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //------------------------------------------------------------------------------
            // POR DEFAULT ATIVADO, DESATIVE APENAS EM CASO DE BANCO DE DADOS QUE POSSUI MUITOS 
            // RELACIONAMENTOS CASO DESEJE MELHORAR A PERFORMANCE DE CARREGAMENTO DOS REGISTROS
            Configuration.LazyLoadingEnabled = true;
            //------------------------------------------------------------------------------
            // DESATIVA A EXCLUSÃO EM CASCATA DE REGISTROS
            // ENTRE PARES  1..N E N..N
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //------------------------------------------------------------------------------
        }

        public override int SaveChanges()
        {
            int OperationLogID = 0;
            //------------------------------------------------------------------------------
            // CAPTURA TODOS OS REGISTROS QUE REALIZARÃO ALGUMA OPERAÇÃO DE
            // (INSERT, UPDATES E DELETES) NO BANCO DE DADOS
            //------------------------------------------------------------------------------
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is Tabela && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified
                        || e.State == EntityState.Deleted));

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    // ESTÉ PONTO PODE SER DEFINIDO O ID DO USUÁRIO QUE ESTÁ CRIANDO O REGISTRO
                    OperationLogID = 1;
                    if (OperationLogID == 0)
                        throw new DbEntityValidationException();
                    ((Tabela)entityEntry.Entity).CadastroLogID = OperationLogID;
                    ((Tabela)entityEntry.Entity).DataCadastro = DateTime.Now;
                }
                if (entityEntry.State == EntityState.Deleted)
                {
                    // ESTÉ PONTO PODE SER DEFINIDO O ID DO USUÁRIO QUE ESTÁ EXCLUINDO O REGISTRO
                    OperationLogID = 1;
                    if (OperationLogID == 0)
                        throw new DbEntityValidationException();
                    // REGISTRO QUE DEVERIAM SER EXCLUIDOS, SÃO APENAS MODIFICADOS PARA O ESTADO DELETADO
                    entityEntry.State = EntityState.Modified;
                    //------------------------------------------------------------------------------
                    //------------------------------------------------------------------------------
                    // DESATIVA O REGISTO ESPEFICO
                    ((Tabela)entityEntry.Entity).ExclusaoLogID = OperationLogID;
                    ((Tabela)entityEntry.Entity).Deletado = true;
                    ((Tabela)entityEntry.Entity).DataExclusao = DateTime.Now;
                    //------------------------------------------------------------------------------
                }
            }
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync()
        {
            int OperationLogID = 0;
            //------------------------------------------------------------------------------
            // CAPTURA TODOS OS REGISTROS QUE REALIZARÃO ALGUMA OPERAÇÃO DE
            // (INSERT, UPDATES E DELETES) NO BANCO DE DADOS
            //------------------------------------------------------------------------------
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is Tabela && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified
                        || e.State == EntityState.Deleted));

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    // ESTÉ PONTO PODE SER DEFINIDO O ID DO USUÁRIO QUE ESTÁ CRIANDO O REGISTRO
                    OperationLogID = 1;
                    if (OperationLogID == 0)
                        throw new DbEntityValidationException();
                    ((Tabela)entityEntry.Entity).CadastroLogID = OperationLogID;
                    ((Tabela)entityEntry.Entity).DataCadastro = DateTime.Now;
                }
                if (entityEntry.State == EntityState.Deleted)
                {
                    // ESTÉ PONTO PODE SER DEFINIDO O ID DO USUÁRIO QUE ESTÁ EXCLUINDO O REGISTRO
                    OperationLogID = 1; 
                    if (OperationLogID == 0)
                        throw new DbEntityValidationException();
                    // REGISTRO QUE DEVERIAM SER EXCLUIDOS, SÃO APENAS MODIFICADOS PARA O ESTADO DELETADO
                    entityEntry.State = EntityState.Modified;
                    //------------------------------------------------------------------------------
                    //------------------------------------------------------------------------------
                    // DESATIVA O REGISTO ESPEFICO
                    ((Tabela)entityEntry.Entity).ExclusaoLogID = OperationLogID;
                    ((Tabela)entityEntry.Entity).Deletado = true;
                    ((Tabela)entityEntry.Entity).DataExclusao = DateTime.Now;
                    //------------------------------------------------------------------------------
                }
            }
            return base.SaveChangesAsync();
        }

    }
}