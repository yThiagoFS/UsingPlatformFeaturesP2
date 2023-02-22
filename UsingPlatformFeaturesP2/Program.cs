using Microsoft.AspNetCore.HostFiltering;
using UsingPlatformFeaturesP2;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<HostFilteringOptions>(opts =>
{
    opts.AllowedHosts.Clear();
    opts.AllowedHosts.Add("*.example.com");
});

builder.Services.AddDistributedMemoryCache(); // using session feature

builder.Services.AddSession(opts =>
{
    opts.IdleTimeout = TimeSpan.FromMinutes(30);
    opts.Cookie.IsEssential = true;
});

//builder.Services.AddHsts(opt =>
//{
//    opt.MaxAge = TimeSpan.FromDays(1);
//    opt.IncludeSubDomains = true;
//});

var app = builder.Build();

//if (app.Environment.IsProduction())
//{
//    app.UseHsts();
//}
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error.html");
    app.UseStaticFiles();
}

app.UseStatusCodePages("text/html", ResponseStrings.DefaultResponse);

app.Use(async (context, next) =>
{
    if(context.Request.Path == "/error")
    {
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        await Task.CompletedTask;
    }
    else
    {
        await next();
    }
});

//app.UseHttpsRedirection();

#region Using Session

app.UseSession();

app.MapGet("/session", async context =>
{
    int counter1 = (context.Session.GetInt32("counter1") ?? 0) + 1;
    int counter2 = (context.Session.GetInt32("counter2") ?? 0) + 1;

    context.Session.SetInt32("counter1", counter1);
    context.Session.SetInt32("counter2", counter2);

    await context.Session.CommitAsync();

    await context.Response.WriteAsync($"Counter1: {counter1}\n Counter2: {counter2}");
});

app.MapGet("/clear", async context =>
{
    context.Session.Clear();
    await context.Session.CommitAsync();
    context.Response.Redirect("/");
});

#endregion

#region Using Cookies
//builder.Services.Configure<CookiePolicyOptions>(opts =>
//{
//    opts.CheckConsentNeeded = context => true;
//});


//app.UseCookiePolicy();
//app.UseMiddleware<ConsentMiddleware>();

//app.MapGet("/cookie", async context =>
//{
//    int counter1 = int.Parse(context.Request.Cookies["counter1"] ?? "0") + 1;

//    context.Response.Cookies.Append("counter1", counter1.ToString(), new CookieOptions
//    {
//        MaxAge = TimeSpan.FromMinutes(30),
//        IsEssential = true
//    });

//    int counter2 = int.Parse(context.Request.Cookies["counter2"] ?? "0") + 1;

//    context.Response.Cookies.Append("counter2", counter2.ToString(), new CookieOptions
//    {
//        MaxAge = TimeSpan.FromMinutes(30)
//    });

//    await context.Response.WriteAsync($"Counter1: {counter1}\n Counter2: {counter2}");
//});

//app.MapGet("/clear", context =>
//{
//    context.Response.Cookies.Delete("counter1");
//    context.Response.Cookies.Delete("counter2");
//    context.Response.Redirect("/");
//    return Task.CompletedTask;
//});
#endregion

app.MapFallback(async context =>
{
    await context.Response.WriteAsync($"HTTPS Request: {context.Request.IsHttps} \n");
    await context.Response.WriteAsync("Hello World");
});

app.Run(context =>
{
    throw new Exception($"Something went wront!");
});

app.Run();
