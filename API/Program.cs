using API.LoginCtrl;
using BLL.DependencyResolvers;
using Newtonsoft.Json;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);




builder.Services.AddControllers();
builder.Services.AddControllers()
    .AddNewtonsoftJson(static options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Bizim olu�turdugumuz servisler
builder.Services.AddSingleton<LoginStatus>();
builder.Services.AddIdentityService();
builder.Services.AddDbContextService();
builder.Services.AddRepManServices();   


//builder.Services.AddDistributedMemoryCache();
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(15);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});


////JWT Token
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = false,
//            ValidateAudience = false,
//            ValidateIssuerSigningKey = true, //Sadece imza 
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("asdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasd"))
//        };
//    });






WebApplication app = builder.Build();










if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseSession();

//app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
