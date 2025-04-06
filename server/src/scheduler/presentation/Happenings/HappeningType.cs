using Meets.Scheduler.Polls;

namespace Meets.Scheduler.Happenings;


internal sealed class HappeningType : ObjectType<HappeningModel>
{
    protected override void Configure(IObjectTypeDescriptor<HappeningModel> descriptor)
    {
        descriptor
            .Name("Event");

        descriptor
            .Field(_ => _.Id)
            .Type<NonNullType<UuidType>>();
        descriptor
            .Field(_ => _.Name)
            .Type<NonNullType<StringType>>();
        descriptor
            .Field(_ => _.Description)
            .Type<StringType>();
        descriptor
            .Field<HappeningResolver>(_ => _.GetPollAsync(default!, default!, default))
            .Type<NonNullType<PollType>>();
    }
}
