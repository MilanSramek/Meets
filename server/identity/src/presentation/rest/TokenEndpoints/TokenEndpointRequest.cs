using System.Text.Json.Serialization;

namespace Meets.Identity.TokenEndpoints;

internal sealed record TokenEndpointRequest
(
    string Username,
    string Password,
    string Scope,
    [property: JsonPropertyName("grant_type")]
    string GrantType = "password"
);