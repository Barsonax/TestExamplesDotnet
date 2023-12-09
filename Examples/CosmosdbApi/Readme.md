# ASP.NET Core API with CosmosDb
This example shows how to test an [ASP.NET API](https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-7.0&tabs=visual-studio) that uses a cosmosdb database for persistence.

## How to run
All setup is handled by the code so to run the tests just run:
```
dotnet test
```

## Limitations
Due to the limitations of the [cosmosdb emulator](https://learn.microsoft.com/en-us/azure/cosmos-db/emulator) parallelization has been turned off in this example. Turning this on will result in these kind of errors:
```
Response status code does not indicate success: ServiceUnavailable (503); Substatus: 1007; ActivityId: 0f210fb2-9cb8-4d68-bb5e-34c91be50678; Reason: (Sorry, we are currently experiencing high demand in this region South Central US, and cannot fulfill your request at this time Sat, 09 Dec 2023 16:45:18 GMT.
```
Since this is a pretty huge limitation if you value testing then I strongly suggest choosing something else as a database.
