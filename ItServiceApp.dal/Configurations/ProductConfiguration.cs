using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItServiceApp.entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItServiceApp.dal.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.HasKey(m => m.ProductId);

            builder.Property(m => m.Name).IsRequired().HasMaxLength(100);

            builder.Property(m => m.DateAdded).HasDefaultValueSql("getdate()");  // mssql
            
        }
    }
}
