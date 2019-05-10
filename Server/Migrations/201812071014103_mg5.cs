namespace NPExamF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mg5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DBTruUsers", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DBTruUsers", "Email");
        }
    }
}
