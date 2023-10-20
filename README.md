# TestExamplesDotnet
![example workflow](https://github.com/Barsonax/TestExamplesDotnet/actions/workflows/dotnet.yml/badge.svg)

Whenever I start or join a new project first thing I want is good tests. These tests should be easy to setup and fast to run to make sure that inner developer feedback loop is as productive as possible. The tests in this repo have been setup with that in mind. Currently most of the tests are aimed at ASP .NET core.

## Prerequisites
- .NET 
- Docker

## Notable features:
- No manual setup required, `dotnet test` is all you need to run. This not only makes it easy for developers to run the tests but also simplifies the CI as you can see in the [github workflow file](.github/workflows/dotnet.yml). This is made possible with the magic of [TestContainers](https://dotnet.testcontainers.org/) and some custom code.
- Databases are pooled and cleaned between runs with [Respawn](https://github.com/jbogard/Respawn). This way migrations only have to be run once which saves alot of time.
- Tests can run in parallel.

## How to use
I decided not to turn this into a nuget package for now. Depending on if you are using nunit or xunit copy the code that you need to your project. For instance if you are using PostgreSql then copy the code from [TestExamplesDotnet](TestExamplesDotnet) and [TestExamplesDotnet.PostgreSql](TestExamplesDotnet.PostgreSql) to your test project. For examples on how to setup nunit or xunit see the [Examples](Examples).

I do suggest you flatten the code that you need into a single project to keep things simpler.

## Examples
Some examples can be found in [Examples](Examples). For instance for postgresql you can find examples for [NUnit](Examples/Api/PostgreSql/Api.PostgreSql.Nunit) and [xUnit](Examples/Api/PostgreSql/Api.PostgreSql.Xunit). There's also a browser test example using [Playwright](Examples/Razor/Razor.Playwright).
