namespace NPExamF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mg4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DBChats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DBUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NickName = c.String(),
                        Email = c.String(),
                        PubKey = c.String(),
                        IsSecret = c.Boolean(nullable: false),
                        DBUser_Id = c.Int(),
                        DBChat_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DBUsers", t => t.DBUser_Id)
                .ForeignKey("dbo.DBChats", t => t.DBChat_Id)
                .Index(t => t.DBUser_Id)
                .Index(t => t.DBChat_Id);
            
            CreateTable(
                "dbo.DBMyMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SendingTime = c.DateTime(nullable: false),
                        Text = c.String(),
                        ChatId = c.Int(nullable: false),
                        Sender_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DBUsers", t => t.Sender_Id)
                .Index(t => t.Sender_Id);
            
            CreateTable(
                "dbo.DBTruUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NickName = c.String(),
                        Password = c.String(),
                        Address = c.String(),
                        Online = c.Boolean(nullable: false),
                        PrivKey = c.String(),
                        PubKey = c.String(),
                        Port = c.Int(nullable: false),
                        DBTruUser_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DBTruUsers", t => t.DBTruUser_Id)
                .Index(t => t.DBTruUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DBTruUsers", "DBTruUser_Id", "dbo.DBTruUsers");
            DropForeignKey("dbo.DBMyMessages", "Sender_Id", "dbo.DBUsers");
            DropForeignKey("dbo.DBUsers", "DBChat_Id", "dbo.DBChats");
            DropForeignKey("dbo.DBUsers", "DBUser_Id", "dbo.DBUsers");
            DropIndex("dbo.DBTruUsers", new[] { "DBTruUser_Id" });
            DropIndex("dbo.DBMyMessages", new[] { "Sender_Id" });
            DropIndex("dbo.DBUsers", new[] { "DBChat_Id" });
            DropIndex("dbo.DBUsers", new[] { "DBUser_Id" });
            DropTable("dbo.DBTruUsers");
            DropTable("dbo.DBMyMessages");
            DropTable("dbo.DBUsers");
            DropTable("dbo.DBChats");
        }
    }
}
