using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderApi.Consts;
using OrderApi.Model;

namespace OrderApi.Infrastructure.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable(nameof(Customer), Constants.DefaultSchemaName);
            builder.Property(x=>x.Id)
                .ValueGeneratedNever();
        }
    }

}
