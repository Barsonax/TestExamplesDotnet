namespace AngularAuth.Playwright.TestSetup;

public class BrowserStorageState
{
    public BrowserStorageStateOrigin[] Origins { get; init; } = Array.Empty<BrowserStorageStateOrigin>();
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
