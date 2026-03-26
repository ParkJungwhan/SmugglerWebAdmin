using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using MudBlazor.Services;
using SmugglerWebAdmin.Client.Pages;
using SmugglerWebAdmin.Components;
using SmugglerWebAdmin.Components.Account;
using SmugglerWebAdmin.Data;
using SmugglerWebAdmin.Services;
using SmugglerWebCommon.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddAuthenticationStateSerialization();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

builder.Services.AddIdentityCore<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddUserStore<DapperUserStore>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddMudServices();
builder.Services.AddScoped<ThemeService>();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddSingleton<SchemaInitializer>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IRegionService, RegionService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<AdminInitializer>();
builder.Services.AddScoped<TestDataSeeder>();

var app = builder.Build();

// DB 스키마 초기화 및 초기 어드민 계정 생성
using (var scope = app.Services.CreateScope())
{
    var schemaInit = scope.ServiceProvider.GetRequiredService<SchemaInitializer>();
    await schemaInit.InitializeAsync();

    var adminInit = scope.ServiceProvider.GetRequiredService<AdminInitializer>();
    await adminInit.InitializeAsync();

    var testSeeder = scope.ServiceProvider.GetRequiredService<TestDataSeeder>();
    await testSeeder.SeedAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(SmugglerWebAdmin.Client._Imports).Assembly);

app.MapAdditionalIdentityEndpoints();

app.Run();
