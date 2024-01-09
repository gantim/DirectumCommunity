using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectumCommunity.Models;

public class MeetingConfiguration : IEntityTypeConfiguration<MeetingModel>
{
    public void Configure(EntityTypeBuilder<MeetingModel> builder)
    {
        builder.Ignore(e => e.President);
        builder.Ignore(e => e.Secretary);
        builder.Ignore(e => e.Members);
    }
}