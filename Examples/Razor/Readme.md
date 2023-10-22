# ASP.NET Core backend and Vue frontend with Playwright
This example shows how to use a real browser to test an [ASP.NET API](https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-7.0&tabs=visual-studio) backend with a [VueJs](https://vuejs.org/) frontend. [Playwright](https://playwright.dev/dotnet/) is used to setup and control the browser.

## How to run
All setup is handled by the code so to run the tests just run:
```
dotnet test
```

## Lifecycle
The flow chart below shows what happens when you run `dotnet test`:

<img src="/Media/PlaywrightTestsFlowChart.drawio.svg" height="900" />
