# Under construction
Still under development, you can run the tests but they are flaky.

## How to run
[Playwright](https://playwright.dev/dotnet/) is used to run the browser tests. A bit of setup is required to run the browser tests:
```powershell
pwsh playwright.ps1 install
```
Then the tests can be run with:
```
dotnet test
```
