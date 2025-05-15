using Meets.Common.Application;

namespace Meets.Scheduler.Activities;

public sealed record UpdateActivityInput
(
    Opt<string> Name,
    Opt<string?> Description
);
