# Request Value Lookup

Get a value from one of multiple places in a request. Currently supports:

- Query
- Header

## Attribute

| Name     | Type                   | Description                   |
| -------- | ---------------------- | ----------------------------- |
| Location | `RequestValueLocation` | Location to check for value   |
| Name     | `string`               | Priamary key name to look for |
| Aliases  | `string[]`             | Alternate key names           |

## Example

```csharp
public static class VersionedHttpTrigger
{
    [FunctionName(nameof(VersionedHttpTrigger))]
    public static async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "version")]
        HttpRequest req, ILogger log, [
            RequestValue(
            Location = RequestValueLocation.Header | RequestValueLocation.Query,
            Name = "apiVersion",
            Aliases = new[] { "x-api-version" }
        )]
        string version
    )
    {
        if (string.IsNullOrEmpty(version))
        {
            return new BadRequestResult();
        }

        log.LogInformation("Triggered for version {Version}", version);
        return new OkObjectResult(version);
    }
}
```
