using Meets.Common.Application;

namespace Meets.Scheduler.Activities;

public sealed record UpdateActivityModel
(
    Opt<string> Name,
    Opt<string?> Description
);
