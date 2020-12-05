namespace TesteTimipro.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TabelaPessoa",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false),
                        CPF = c.String(),
                        Email = c.String(),
                        ProdutoID = c.Int(nullable: false),
                        Deletado = c.Boolean(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        DataExclusao = c.DateTime(),
                        CadastroLogID = c.Int(nullable: false),
                        ExclusaoLogID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.TabelaProduto", t => t.ProdutoID)
                .Index(t => t.ProdutoID);
            
            CreateTable(
                "dbo.TabelaProduto",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        Deletado = c.Boolean(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        DataExclusao = c.DateTime(),
                        CadastroLogID = c.Int(nullable: false),
                        ExclusaoLogID = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TabelaPessoa", "ProdutoID", "dbo.TabelaProduto");
            DropIndex("dbo.TabelaPessoa", new[] { "ProdutoID" });
            DropTable("dbo.TabelaProduto");
            DropTable("dbo.TabelaPessoa");
        }
    }
}
