using System.ComponentModel.DataAnnotations;

namespace Meets.Identity;

public sealed record CreateUserRequest
(
    [Required] string UserName,
    [Required] string Password
);