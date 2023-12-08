using System;
using System.Collections.Generic;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CosmosDb.AzureFunction.Sut;

public class CosmosDbTrigger1
{
    private readonly ILogger _logger;

    public CosmosDbTrigger1(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<CosmosDbTrigger1>();
    }
    [Function("CosmosDbTrigger1")]
    public void Run([CosmosDBTrigger(
            databaseName: "ToDoItems",
            containerName:"TriggerItems",
            Connection = "CosmosDBConnection",
            LeaseContainerName = "leases",
            CreateLeaseContainerIfNotExists = true)] IReadOnlyList<ToDoItem> todoItems,
        FunctionContext context)
    {
        if (todoItems is not null && todoItems.Any())
        {
            foreach (var doc in todoItems)
            {
                _logger.LogInformation("ToDoItem: {desc}", doc.Description);
            }
        }
    }
}

public class ToDoItem
{
    public string? Id { get; set; }
    public string? Description { get; set; }
}
