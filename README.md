# TestExamplesDotnet
![example workflow](https://github.com/Barsonax/TestExamplesDotnet/actions/workflows/dotnet.yml/badge.svg)

Whenever I start or join a new project first thing I want is good tests. These tests should be easy to setup and fast to run to make sure that inner developer feedback loop is as productive as possible. The tests in this repo have been setup with that in mind. Currently most of the tests are aimed at ASP .NET core.

Some notable features:
- Most tests require no manual setup at all, `dotnet test` is all you need to run. For instance when testing a app that requires a database the test code itself makes sure the database is running using [TestContainers](https://dotnet.testcontainers.org/). 
- Databases are pooled and cleaned between runs with [Respawn](https://github.com/jbogard/Respawn). This way migrations only have to be run once which saves alot of time.
- Tests can run in parallel.

## Prerequisites
- .NET 
- Docker

## How to use
I decided not to turn this into a nuget package for now. Depending on if you are using nunit or xunit copy the code that you need to your project. For instance if you are using PostgreSql then copy the code from [AwesomeApiTest](AwesomeApiTest) and [AwesomeApiTest.PostgreSql](AwesomeApiTest.PostgreSql) to your test project. For examples on how to setup nunit or xunit see the [Examples](Examples).

I do suggest you flatten the code that you need into a single project to keep things simpler.

## Examples
Some examples can be found in [Examples](Examples). For instance for postgresql you can find examples for [nunit](Examples/PostgreSql/AwesomeApiTest.Nunit) and [xunit](Examples/PostgreSql/AwesomeApiTest.Xunit)
