using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ControleTarefas.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErroModel : PageModel
{
    public string Identificador { get; private set; } = "";

    public void OnGet() => Identificador = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
    public void OnPost() => OnGet();
}