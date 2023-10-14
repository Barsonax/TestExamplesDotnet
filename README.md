# AwesomeApiTest
This repository gives a example on how one can setup very fast and easy to run api tests that start to feel more like unit tests even though the whole app and database is being run during the tests. 

This setup currently has the following features:
- Because [TestContainers](https://dotnet.testcontainers.org/) is used for the database no manual setup is required, just run `dotnet test` and you are good to go.
- Databases are pooled and cleaned between runs with [Respawn](https://github.com/jbogard/Respawn). This way migrations only have to be run once which saves alot of time.
- Tests can run in parallel.

## How to use
I decided not to turn this into a nuget package for now. Depending on if you are using nunit or xunit copy the code that you need to your project, its not that much. For instance if you are using PostgreSql then copy the code from [AwesomeApiTest](AwesomeApiTest) and [AwesomeApiTest.PostgreSql](AwesomeApiTest.PostgreSql) to your test project. For examples on how to setup nunit or xunit see the [Examples](Examples).

I do suggest you flatten the code that you need into a single project to keep things simpler.

## Examples
Some examples can be found in [Examples](Examples). For instance for postgresql you can find examples for [nunit](Examples/PostgreSql/AwesomeApiTest.Nunit) and [xunit](Examples/PostgreSql/AwesomeApiTest.Xunit)
