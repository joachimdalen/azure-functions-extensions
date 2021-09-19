# Query Parameter Bindings

## Simple binding

Supports binding to:

- string
- Guid
- int
- object
- bool

```csharp
public static class StringQueryParamHttpTrigger
    {
        [FunctionName(nameof(StringQueryParamHttpTrigger))]
        public static async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "test/query/name")]           HttpRequest req, ILogger log, [QueryParam("name")] string name)
        {
            var data = new NameModel
            {
                Name = name
            };
            return new OkObjectResult(data);
        }
    }

```

## Complex binding

Extends query parameter bindings to POCO.

```csharp
public class QueryParamValues
{
    [QueryParamValue]
    public Guid? Id { get; set; }

    [QueryParamValue]
    public string Name { get; set; }

    [QueryParamValue]
    public int Age { get; set; }
}

```

Usage in functions

```csharp
 [FunctionName(nameof(ClassQueryParamHttpTrigger))]
public static async Task<IActionResult> RunAsync([
    HttpTrigger(AuthorizationLevel.Function, "get", Route = "test/query/class")] HttpRequest req,
    ILogger log,
    [QueryParam] QueryParamValues queryParamContainer)
{
    var data = new
    {
        queryParamContainer.Name,
        queryParamContainer.Age,
        queryParamContainer.Id
    };
    return new OkObjectResult(data);
}
```
