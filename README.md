# TestExamplesDotnet
![build](https://github.com/Barsonax/TestExamplesDotnet/actions/workflows/dotnet.yml/badge.svg)

## What is this?
Whenever I start or join a new project one of the first things I want are good tests. These tests should be easy to setup, fast to run and give good confidence that the application is actually working. This is important to have a productive developer feedback loop. The example tests in this repo have been setup with that in mind.

Currently the example tests in this repo are aimed at ASP .NET core with entityframework for persistence.

## What this is not
This is not an example repository showing you how to run end to end tests against a real environment. Every example you see here will run completely locally. This is both an advantage (higher reliability, much faster) as a disadvantage (less real than the real thing). Things like network latency will play out differently in a real environment so keep this in mind.

## Prerequisites
- .NET 
- Docker

## Notable features:
- No manual setup required, `dotnet test` is all you need to run. This not only makes it easy for developers to run the tests but also simplifies the CI as you can see in the [github workflow file](.github/workflows/dotnet.yml). This is made possible with the magic of [TestContainers](https://dotnet.testcontainers.org/) and some custom code.
- Uses a real database so you can be quite confident that your app is actually working. You won't run into differences between an in memory database and a real database here.
- Databases are pooled and cleaned between runs with [Respawn](https://github.com/jbogard/Respawn). This way migrations only have to be run once which saves alot of time. Furthermore tests run in parallel. After migrations are done and depending on how fast your app is a test might finish in less than 100ms.

Just how fast is this setup? Just look at this test run from [Api.PostgreSql.Nunit](Examples/Api/PostgreSql/Api.PostgreSql.Nunit):
<img src="/Media/2000testsin10sec.gif" height="300" />


## How to use
I decided not to turn this into a nuget package for now. Depending on if you are using NUnit or xUnit copy the code that you need to your project. For instance if you are using PostgreSql then copy the code from [TestExamplesDotnet](TestExamplesDotnet) and [TestExamplesDotnet.PostgreSql](TestExamplesDotnet.PostgreSql) to your test project then use [Examples](Examples) to see how to setup your test project. I do suggest you flatten the code that you need into a single project to keep things simpler.

If you can choose which testframework to use then I suggest going with NUnit as NUnit achieves a higher level of parallelism because it will even run test cases in the same class in parallel where xUnit will not run tests in the same class in parallel. As such most examples will be for NUnit but the setup is very similar. 

## Examples
[Examples](Examples) contain example projects which will show you how to setup the tests for various types of applications. For instance for postgresql you can find examples for [NUnit](Examples/Api/PostgreSql/Api.PostgreSql.Nunit) and [xUnit](Examples/Api/PostgreSql/Api.PostgreSql.Xunit). There's also a browser test example using [Playwright](Examples/Razor/Razor.Playwright).
