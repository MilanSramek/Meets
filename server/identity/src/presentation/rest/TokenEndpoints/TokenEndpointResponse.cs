using System.Text.Json.Serialization;

namespace Meets.Identity.TokenEndpoints;

internal sealed record TokenEndpointResponse
(
    [property: JsonPropertyName("access_token")]
    string AccessToken,
    [property: JsonPropertyName("token_type")]
    string TokenType,
    [property: JsonPropertyName("expires_in")]
    uint ExpiresIn,
    [property: JsonPropertyName("refresh_token")]
    string RefreshToken
);