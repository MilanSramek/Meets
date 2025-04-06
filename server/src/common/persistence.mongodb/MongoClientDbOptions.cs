using System.ComponentModel.DataAnnotations;

namespace Meets.Common.Persistence.MongoDb;

public sealed class MongoClientDbOptions
{
    public const string SectionName = "MongoDb";

    [Required]
    public string Host { get; set; }

    [Required]
    public string Database { get; init; }

    [Required]
    public string Username { get; init; }

    [Required]
    public string Password { get; init; }
}
