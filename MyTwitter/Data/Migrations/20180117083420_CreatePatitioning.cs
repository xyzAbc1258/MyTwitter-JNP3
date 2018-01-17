using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MyTwitter.Data.Migrations
{
    public partial class CreatePatitioning : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PARTITION FUNCTION part_by_created_date(datetime2(7))
                AS RANGE RIGHT FOR VALUES
                ('2018-01-01 00:00:00.0000000','2018-02-01 00:00:00.0000000','2018-03-01 00:00:00.0000000')

                CREATE PARTITION SCHEME CreatedDatePartitionScheme 
                AS PARTITION part_by_created_date ALL TO ([PRIMARY]) 

                ALTER TABLE dbo.Posts
                DROP CONSTRAINT PK_Posts

                ALTER TABLE dbo.Posts 
                ADD CONSTRAINT PK_Posts
                PRIMARY KEY NONCLUSTERED  (Id)
                   WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, 
                         ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

                CREATE CLUSTERED INDEX IX_Posts_DateCreated 
	                ON dbo.Posts (DateCreated)
                  WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, 
                        ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) 
                  ON CreatedDatePartitionScheme(DateCreated)
            ");
        }
    }
}
