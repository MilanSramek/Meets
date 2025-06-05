namespace Meets.Scheduler.Activities;

internal sealed class UpdateActivityInput
{
    public Optional<string> Name { get; set; }
    public Optional<string?> Description { get; set; }
}
