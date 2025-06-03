namespace Meets.Scheduler.Activities;

internal sealed class UpdateActivityRequest
{
    public Optional<string> Name { get; set; }
    public Optional<string?> Description { get; set; }
}
