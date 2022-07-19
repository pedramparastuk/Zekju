using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class FlightConfiguration : IEntityTypeConfiguration<Flight>
{
    public void Configure(EntityTypeBuilder<Flight> builder)
    {
        builder.ToTable("Flights");

        builder.Property(p => p.Id).HasColumnName("flight_id");
        builder.Property(p => p.RouteId).HasColumnName("route_id");
        builder.Property(p => p.DepartureTime).HasColumnName("departure_time");
        builder.Property(p => p.ArrivalDate).HasColumnName("arrival_time");
        builder.Property(p => p.AirlineId).HasColumnName("airline_id");

        builder.HasOne<Route>(flight => flight.Route)
            .WithMany(route => route.Flights)
            .HasForeignKey(flight => flight.RouteId);


        builder.HasIndex(p => p.DepartureTime).IsUnique(false);
        builder.HasIndex(p => p.RouteId).IsUnique(false);
    }
}