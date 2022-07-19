using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class RouteConfiguration : IEntityTypeConfiguration<Route>
{
    public void Configure(EntityTypeBuilder<Route> builder)
    {
        builder.ToTable("Routes");

        builder.Property(p => p.Id).HasColumnName("route_id");
        builder.Property(p => p.OriginCityId).HasColumnName("origin_city_id");
        builder.Property(p => p.DestinationCityId).HasColumnName("destination_city_id");
        builder.Property(p => p.DepartureDate).HasColumnName("departure_date");

        builder.HasIndex(p => new
        {
            p.OriginCityId,
            p.DestinationCityId
        }).IsUnique(false);
    }
}