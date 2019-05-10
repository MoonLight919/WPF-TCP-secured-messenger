namespace NPExamF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mg6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DBTruUsers", "EmailPassword", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DBTruUsers", "EmailPassword");
        }
    }
}
