using System.Text.Json;

namespace AngularAuth.Playwright.TestSetup;

public class BrowserStorageState
{
    private static JsonSerializerOptions JsonSerializerOptions => new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    public BrowserStorageStateOrigin[] Origins { get; init; } = Array.Empty<BrowserStorageStateOrigin>();

    public string Serialize() => JsonSerializer.Serialize(this, JsonSerializerOptions);
}

public class BrowserStorageStateOrigin
{
    public required string Origin { get; init; }
    public required BrowserStorageStateLocalStorage[] LocalStorage { get; init; }
}
public class BrowserStorageStateLocalStorage
{
    public required string Name { get; init; }
    public required string Value { get; init; }
}
