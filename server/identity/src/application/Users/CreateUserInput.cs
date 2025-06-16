using System.ComponentModel.DataAnnotations;

namespace Meets.Identity.Users;

public sealed record CreateUserInput
(
    [Required]
    string Username,
    [Required]
    string Password
);