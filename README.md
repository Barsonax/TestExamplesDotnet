# TestExamplesDotnet
![build](https://github.com/Barsonax/TestExamplesDotnet/actions/workflows/dotnet.yml/badge.svg)

Whenever I start or join a new project one of the firsts things I want are good tests. These tests should be easy to setup, fast to run and give good confidence that the application is actually working. The example tests in this repo have been setup with that in mind.

Currently the example tests in this repo are aimed at ASP .NET core with entity framework for persistence.

## Prerequisites
- .NET 
- Docker

## Notable features:
- No manual setup required, `dotnet test` is all you need to run. This not only makes it easy for developers to run the tests but also simplifies the CI as you can see in the [github workflow file](.github/workflows/dotnet.yml). This is made possible with the magic of [TestContainers](https://dotnet.testcontainers.org/) and some custom code.
- Databases are pooled and cleaned between runs with [Respawn](https://github.com/jbogard/Respawn). This way migrations only have to be run once which saves alot of time.
- Tests can run in parallel.

## How to use
I decided not to turn this into a nuget package for now. Depending on if you are using NUnit or xUnit copy the code that you need to your project. For instance if you are using PostgreSql then copy the code from [TestExamplesDotnet](TestExamplesDotnet) and [TestExamplesDotnet.PostgreSql](TestExamplesDotnet.PostgreSql) to your test project. For examples on how to setup [NUnit](Examples/Api/PostgreSql/Api.PostgreSql.Nunit) or [xUnit](Examples/Api/PostgreSql/Api.PostgreSql.Xunit) see the [Examples](Examples). I do suggest you flatten the code that you need into a single project to keep things simpler.

If can choose which testframework to use then I suggest going with NUnit as NUnit achieves a higher level of parallelism because it will even run test cases in the same class in parallel where xUnit will not run tests in the same class in parallel. As such most examples will be for NUnit but the setup is very similar. 

## Examples
Some examples can be found in [Examples](Examples). For instance for postgresql you can find examples for NUnit and xUnit. There's also a browser test example using [Playwright](Examples/Razor/Razor.Playwright).
