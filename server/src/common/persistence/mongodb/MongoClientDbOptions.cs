using System.ComponentModel.DataAnnotations;

namespace Meets.Common.Persistence.MongoDb;

public sealed class MongoClientDbOptions
{
    [Required]
    public string Host { get; set; }

    [Required]
    public string Database { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}
