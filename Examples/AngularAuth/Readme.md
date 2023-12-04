# ASP.NET Core backend with auth and Angular frontend with Playwright
This example shows how to use a real browser to test an [ASP.NET API](https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-7.0&tabs=visual-studio) backend with a [Angular](https://angular.io/) frontend. [Playwright](https://playwright.dev/dotnet/) is used to setup and control the browser.

The Angular frontend was created with `ng new` then slightly modified so that it shows a link to my blog for the test. For authentication the [Microsoft Authentication Library for Angular](https://github.com/AzureAD/microsoft-authentication-library-for-js/tree/dev/lib/msal-angular) is used. The Angular app is then served from ASP.NET which also makes it easy to test using [WebApplicationFactory](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-7.0). This might not match your specific application model, for instance you might serve the frontend from a different machine. This doesn't have to be a problem but do be aware of this difference if this is the case as that will affect your testing strategy.

## How to run
All setup is handled by the code so to run the tests just run:
```
dotnet test
```

## Lifecycle
The flow chart below shows what happens when you run `dotnet test`:

<img src="/Media/PlaywrightTestsFlowChart.drawio.svg" height="900" />
