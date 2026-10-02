using ControleTarefas.Data;
using ControleTarefas.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages().AddMvcOptions(options =>
{
    options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor(
        (valor, campo) => $"O valor informado para {campo} é inválido.");
    options.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor(
        campo => $"Informe o campo {campo}.");
    options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
        valor => "Informe um valor válido.");
});
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<ITarefaRepository, TarefaRepository>();

var app = builder.Build();

using (var escopo = app.Services.CreateScope())
{
    escopo.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Erro");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
