namespace CITADEL_NEXUS_PUBLIC.Services;

public sealed class SanityFilterService
{
    private static readonly string[] BlockedTerms =
    [
        "private key",
        "internal hub",
        "bypass",
        "exploit",
        "credential"
    ];

    public (bool IsAllowed, string Reason) Validate(string prompt)
    {
        if (string.IsNullOrWhiteSpace(prompt))
        {
            return (false, "Message is required.");
        }

        if (prompt.Length < 4)
        {
            return (false, "Message must be at least 4 characters.");
        }

        if (prompt.Length > 300)
        {
            return (false, "Message exceeds the 300 character public limit.");
        }

        var normalized = Normalize(prompt);
        if (BlockedTerms.Any(term => normalized.Contains(Normalize(term), StringComparison.OrdinalIgnoreCase)))
        {
            return (false, "Message blocked by sanity filter policy.");
        }

        return (true, "Accepted");
    }

    private static string Normalize(string input)
    {
        return new string(
            input
                .ToLowerInvariant()
                .Replace('1', 'i')
                .Replace('3', 'e')
                .Replace('4', 'a')
                .Replace('5', 's')
                .Replace('0', 'o')
                .Replace('!', 'i')
                .Replace('@', 'a')
                .Where(char.IsLetterOrDigit)
                .ToArray());
    }
}
