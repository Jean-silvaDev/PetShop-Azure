using FunctionApp.Data;
using FunctionApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionApp;

public class OnSalesUpdate
{
    private readonly ILogger<OnSalesUpdate> _logger;
    private readonly ApplicationDbContext _dbContext;

    public OnSalesUpdate(ILogger<OnSalesUpdate> logger, ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [Function("OnSalesUpdate")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
    {
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        SalesRequest? salesRequest = JsonConvert.DeserializeObject<SalesRequest>(requestBody);
        if (salesRequest != null) {
            salesRequest.Id = Guid.NewGuid().ToString();
            salesRequest.Status = String.Empty;
            _dbContext.SalesRequests.Add(salesRequest);
            _dbContext.SaveChanges();
        }
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}