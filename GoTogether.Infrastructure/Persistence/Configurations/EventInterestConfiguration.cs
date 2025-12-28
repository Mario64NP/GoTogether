using GoTogether.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoTogether.Infrastructure.Persistence.Configurations;

public class EventInterestConfiguration : IEntityTypeConfiguration<EventInterest>
{
    public void Configure(EntityTypeBuilder<EventInterest> builder)
    {
        builder.HasKey(ei => ei.Id);

        builder
            .HasOne(ei => ei.User)
            .WithMany(u => u.EventInterests)
            .HasForeignKey(ei => ei.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(ei => ei.Event)
            .WithMany(e => e.EventInterests)
            .HasForeignKey(ei => ei.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(ei => ei.Message)
            .HasMaxLength(500);
    }
}
