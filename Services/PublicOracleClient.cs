using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Sockets;
using CITADEL_NEXUS_PUBLIC.Models;
using Microsoft.Extensions.Options;

namespace CITADEL_NEXUS_PUBLIC.Services;

public sealed class PublicOracleClient(HttpClient httpClient, IOptions<PublicOracleOptions> options)
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly PublicOracleOptions _options = options.Value;

    public bool IsConfigured()
    {
        return TryValidatePublicEndpoint(_options.Endpoint, out _) && !string.IsNullOrWhiteSpace(_options.PublicApiKey);
    }

    public async Task<(bool Success, string Message)> SendAsync(string prompt)
    {
        if (!TryValidatePublicEndpoint(_options.Endpoint, out var endpoint))
        {
            return (false, "Public Oracle endpoint must be a valid public HTTPS URL.");
        }

        if (string.IsNullOrWhiteSpace(_options.PublicApiKey))
        {
            return (false, "Missing Public API key.");
        }

        using var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
        {
            Content = JsonContent.Create(new { prompt })
        };

        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.Add("X-Public-Api-Key", _options.PublicApiKey);

        try
        {
            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return (false, $"Public Oracle returned {(int)response.StatusCode}. {Trim(content)}");
            }

            var parsed = TryReadOracleMessage(content);
            return (true, string.IsNullOrWhiteSpace(parsed) ? "Oracle accepted request." : parsed!);
        }
        catch (HttpRequestException)
        {
            return (false, "Unable to reach the Public Oracle endpoint.");
        }
    }

    private static string? TryReadOracleMessage(string content)
    {
        try
        {
            var payload = System.Text.Json.JsonSerializer.Deserialize<OracleResponse>(content);
            return payload?.Message ?? payload?.Reply;
        }
        catch
        {
            return Trim(content);
        }
    }

    private static string Trim(string content)
    {
        const int maxLength = 240;
        return content.Length <= maxLength ? content : content[..maxLength];
    }

    private static bool TryValidatePublicEndpoint(string endpoint, out Uri? uri)
    {
        uri = null;
        if (!Uri.TryCreate(endpoint, UriKind.Absolute, out var parsed) || parsed.Scheme != Uri.UriSchemeHttps)
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(parsed.UserInfo))
        {
            return false;
        }

        if (parsed.IsLoopback || parsed.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase) || parsed.Host.EndsWith(".internal", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        var blockedTlds = new[] { ".local", ".intranet", ".corp" };
        if (blockedTlds.Any(tld => parsed.Host.EndsWith(tld, StringComparison.OrdinalIgnoreCase)))
        {
            return false;
        }

        if (IPAddress.TryParse(parsed.Host, out _))
        {
            return false;
        }

        if (IPAddress.TryParse(parsed.DnsSafeHost, out var ip) && (IPAddress.IsLoopback(ip) || IsPrivateIp(ip)))
        {
            return false;
        }

        uri = parsed;
        return true;
    }

    private static bool IsPrivateIp(IPAddress ip)
    {
        if (ip.AddressFamily == AddressFamily.InterNetworkV6)
        {
            return ip.IsIPv6LinkLocal ||
                   ip.IsIPv6SiteLocal ||
                   ip.IsIPv6Multicast ||
                   ip.Equals(IPAddress.IPv6Loopback) ||
                   ip.Equals(IPAddress.IPv6None) ||
                   (ip.GetAddressBytes()[0] & 0xFE) == 0xFC;
        }

        var bytes = ip.GetAddressBytes();
        return bytes.Length switch
        {
            4 => bytes[0] == 0 ||
                 bytes[0] == 10 ||
                 bytes[0] == 127 ||
                 (bytes[0] == 172 && bytes[1] >= 16 && bytes[1] <= 31) ||
                 (bytes[0] == 192 && bytes[1] == 168) ||
                 (bytes[0] == 169 && bytes[1] == 254) ||
                 (bytes[0] == 100 && bytes[1] >= 64 && bytes[1] <= 127) ||
                 (bytes[0] == 198 && (bytes[1] == 18 || bytes[1] == 19)),
            _ => false
        };
    }

    private sealed class OracleResponse
    {
        public string? Message { get; set; }

        public string? Reply { get; set; }
    }
}
