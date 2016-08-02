namespace TechFu.Nirvana.EventStoreSample.Infrastructure.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class leadPrototype : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Leads",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Address = c.String(),
                        Deleted = c.DateTime(),
                        DeletedBy = c.String(),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedBy = c.String(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Lead_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BusinessMeasures",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AnnualRevenue = c.Double(nullable: false),
                        NumberOfEmployees = c.Double(nullable: false),
                        EntytyType = c.Int(nullable: false),
                        Deleted = c.DateTime(),
                        DeletedBy = c.String(),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedBy = c.String(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_BusinessMeasure_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Leads", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.IndicatorValues",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IndicatorType = c.Int(nullable: false),
                        SerializerType = c.Int(nullable: false),
                        Deleted = c.DateTime(),
                        DeletedBy = c.String(),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedBy = c.String(),
                        Indicator_Id = c.Guid(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_IndicatorValue_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PerformanceIndicators", t => t.Indicator_Id)
                .Index(t => t.Indicator_Id);
            
            CreateTable(
                "dbo.PerformanceIndicators",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IndicatorType = c.Int(nullable: false),
                        SelectedSourceType = c.Int(nullable: false),
                        Deleted = c.DateTime(),
                        DeletedBy = c.String(),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedBy = c.String(),
                        Lead_Id = c.Guid(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_PerformanceIndicator_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Leads", t => t.Lead_Id)
                .Index(t => t.Lead_Id);
            
            AddColumn("dbo.Coupons", "CouponTypeValue", c => c.Int(nullable: false));
            DropColumn("dbo.Coupons", "CouponType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Coupons", "CouponType", c => c.Int(nullable: false));
            DropForeignKey("dbo.IndicatorValues", "Indicator_Id", "dbo.PerformanceIndicators");
            DropForeignKey("dbo.PerformanceIndicators", "Lead_Id", "dbo.Leads");
            DropForeignKey("dbo.BusinessMeasures", "Id", "dbo.Leads");
            DropIndex("dbo.PerformanceIndicators", new[] { "Lead_Id" });
            DropIndex("dbo.IndicatorValues", new[] { "Indicator_Id" });
            DropIndex("dbo.BusinessMeasures", new[] { "Id" });
            DropColumn("dbo.Coupons", "CouponTypeValue");
            DropTable("dbo.PerformanceIndicators",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_PerformanceIndicator_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.IndicatorValues",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_IndicatorValue_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.BusinessMeasures",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_BusinessMeasure_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Leads",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Lead_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
