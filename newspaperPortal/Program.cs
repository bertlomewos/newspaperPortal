using newspaperPortal.Contract;
using newspaperPortal.Model;
using Supabase;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<Supabase.Client>(_ => 

    new Supabase.Client(
        builder.Configuration["Supabase:Url"],
        builder.Configuration["Supabase:AnonKey"],
        new SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = true
        }));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        // Allow your frontend address
        policy.WithOrigins("http://127.0.0.1:5500", "http://localhost:54770", "https://bertlomewos.github.io/newsletterFrontend/")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "api");
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.MapPost(
    "/newsletters",
    async (
       CreateNewsLatterRequest request,
       Supabase.Client client) =>
    {
        var newsletter = new NewsLetter
        {
            Name = request.Name,
            Description = request.Description,
            ReadTime = request.ReadTime

        };
        var response  = await client.From<NewsLetter>().Insert(newsletter);

        var News = response.Models.First();

        return Results.Ok(News.Id);
    });

app.MapGet("newsletters{id}",
    async (long id, Supabase.Client client) =>
    {
        var response = await client.From<NewsLetter>().Where(n=> n.Id == id).Get();

        var newsletter = response.Models.FirstOrDefault();

        if(newsletter == null)
        {
            return Results.NotFound();
        }

        var  newsletterResponse = new NewsLatterResponse
        {
            Id = newsletter.Id,
            Name = newsletter.Name,
            Description = newsletter.Description,
            ReadTime = newsletter.ReadTime,
            CreatedAt = newsletter.CreatedAt
        };

        return Results.Ok(newsletterResponse);


    });
app.MapGet("newsletters", async (Supabase.Client client) =>
{
    var response = await client.From<NewsLetter>().Get();

    var newsletters = response.Models
        .Select(n => new NewsLatterResponse
        {
            Id = n.Id,
            Name = n.Name,
            Description = n.Description,
            ReadTime = n.ReadTime,
            CreatedAt = n.CreatedAt
        })
        .ToList();

    return Results.Ok(newsletters);
});


app.MapDelete("newsletters{id}",
    async (long id, Supabase.Client client) =>
    {
        await client
        .From<NewsLetter>()
        .Where(n => n.Id == id)
        .Delete();

        return Results.NoContent();
    });

app.Run();
