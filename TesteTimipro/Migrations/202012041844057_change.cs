namespace TesteTimipro.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TabelaPessoa", "CPF", c => c.String(nullable: false));
            AlterColumn("dbo.TabelaPessoa", "Email", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TabelaPessoa", "Email", c => c.String());
            AlterColumn("dbo.TabelaPessoa", "CPF", c => c.String());
        }
    }
}
