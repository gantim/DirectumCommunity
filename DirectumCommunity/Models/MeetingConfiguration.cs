using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectumCommunity.Models;

public class MeetingConfiguration : IEntityTypeConfiguration<Meeting>
{
    public void Configure(EntityTypeBuilder<Meeting> builder)
    {
        builder.Ignore(e => e.President);
        builder.Ignore(e => e.Secretary);
        builder.Ignore(e => e.Members);
    }
}