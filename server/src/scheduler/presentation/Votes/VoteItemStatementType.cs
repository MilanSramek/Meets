namespace Meets.Scheduler.Votes;

internal sealed class VoteItemStatementType : EnumType<VoteItemStatement>
{
    protected override void Configure(IEnumTypeDescriptor<VoteItemStatement> statement)
    {
        statement
            .Name("VoteItemStatement");

        statement
            .Value(VoteItemStatement.Yes)
            .Description("Yes");
        statement
            .Value(VoteItemStatement.No)
            .Description("No");
        statement
            .Value(VoteItemStatement.Maybe)
            .Description("Not sure");
    }
}
