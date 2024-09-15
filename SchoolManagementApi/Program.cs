using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using SchoolManagementApi.Contexts;
using SchoolManagementApi.Models;
using SchoolManagementApi.Services;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.

builder.Services.AddControllers()
                .AddOData(
                    options => options.AddRouteComponents(getEdmModel() 
                                      ).OrderBy().Select().SkipToken().Expand().Count()
                                       .Filter().EnableQueryFeatures(50)
                );

IEdmModel getEdmModel() {
    var odataBuilder = new ODataConventionModelBuilder();
    odataBuilder.EntitySet<School>("School").EntityType.HasKey(x => x.Id);
    odataBuilder.EntitySet<Student>("Student").EntityType.HasKey(x => x.Id);

    return odataBuilder.GetEdmModel();
}

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddDbContext<DataBaseContext>(options => {
    var connectionString = Environment.GetEnvironmentVariable("SchoolDBConnection");

    if (string.IsNullOrEmpty(connectionString)) {
        throw new Exception("Connection string 'SchoolDBConnection' not found in environment variables.");
    }

    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddScoped<SchoolService>();
builder.Services.AddScoped<StudentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AlloAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
