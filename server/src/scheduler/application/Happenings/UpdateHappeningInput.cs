using Meets.Common.Application;

namespace Meets.Scheduler.Happenings;

public sealed record UpdateHappeningInput
(
    Opt<string> Name,
    Opt<string?> Description
);
