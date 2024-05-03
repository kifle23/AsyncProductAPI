using AsyncProductAPI.Data;
using AsyncProductAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite("Data Source=RequestDB.db") );

var app = builder.Build();


app.UseHttpsRedirection();

//Start Endpoint
app.MapPost("api/v1/products",async(AppDbContext context, ListingRequest listingRequest) => {
    if(listingRequest == null)
        return Results.BadRequest();
    
    listingRequest.RequestStatus = "ACCEPT";
    listingRequest.EstimatedComplitionTime = "2024-05-03:18:00:00";

    await context.ListingRequests.AddAsync(listingRequest);
    await context.SaveChangesAsync();

    return Results.Accepted("api/v1/productstatus/{listingRequestId}",listingRequest);
});

app.Run();

