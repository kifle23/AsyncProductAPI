using AsyncProductAPI.Data;
using AsyncProductAPI.Dtos;
using AsyncProductAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite("Data Source=RequestDB.db"));

var app = builder.Build();


app.UseHttpsRedirection();

//Start Endpoint
app.MapPost("api/v1/products", async (AppDbContext context, ListingRequest listingRequest) =>
{
    if (listingRequest == null)
        return Results.BadRequest();

    listingRequest.RequestStatus = "ACCEPT";
    listingRequest.EstimatedComplitionTime = "2024-05-03:18:00:00";

    await context.ListingRequests.AddAsync(listingRequest);
    await context.SaveChangesAsync();

    return Results.Accepted("api/v1/productstatus/{listingRequestId}", listingRequest);
});

//Status endpoint
app.MapGet("api/v1/productstatus/{requestId}", (AppDbContext context, string requestId) =>
{
    var listingRequest = context.ListingRequests.FirstOrDefault(lr => lr.RequestId == requestId);

    if (listingRequest == null)
        return Results.NotFound();

    ListingStatus listingStatus = new()
    {
        RequestStatus = listingRequest.RequestStatus,
        ResourceURL = string.Empty
    };

    if (listingRequest.RequestStatus.ToUpper() == "COMPLETE")
    {
        listingStatus.ResourceURL = $"api/v1/products/{Guid.NewGuid().ToString()}";
        return Results.Redirect("http://localhost:5248/" + listingStatus.ResourceURL);
    }

    listingStatus.EstimatedCompletionTime = "2024-05-03:18:00:00";
    return Results.Ok(listingStatus);
});

//Final endpoint

app.MapGet("api/v1/products/{requestId}", (string requestId)=>{
    return Results.Ok("This is where you would pass back the final result");
});

app.Run();

