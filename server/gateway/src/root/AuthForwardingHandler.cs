namespace Meets.Gateway;

public class AuthForwardingHandler : DelegatingHandler
{
    private const string AuthHeaderName = "Authorization";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthForwardingHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var incomingHeaders = _httpContextAccessor.HttpContext?.Request.Headers;
        if (incomingHeaders?.TryGetValue(AuthHeaderName, out var authHeader) == true)
        {
            request.Headers.Add(AuthHeaderName, authHeader.ToString());
        }

        return base.SendAsync(request, cancellationToken);
    }
}
