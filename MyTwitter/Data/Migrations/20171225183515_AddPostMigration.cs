using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MyTwitter.Data.Migrations
{
    public partial class AddPostMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable("Posts", columns =>
                new
                {
                    Id = columns.Column<int>(nullable:false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = columns.Column<DateTime>(nullable:false),
                    DateUpdated = columns.Column<DateTime>(nullable:false),
                    Message = columns.Column<string>(maxLength:255),
                    IdApplicationUser = columns.Column<int>(nullable:false)
                }, constraints: t =>
            {
                t.PrimaryKey("PK_Posts",x => x.Id);
                t.ForeignKey("FK_ApplicationUser", x => x.IdApplicationUser, "AspNetUsers", "Id");
            });
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("Posts");
        }
    }
}
