using ControleTarefas.Models;
using ControleTarefas.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ControleTarefas.Pages;

public class ExcluirModel(ITarefaRepository repositorio) : PageModel
{
    public Tarefa Tarefa { get; private set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var tarefa = await repositorio.ObterAsync(id);
        if (tarefa is null) return NotFound();

        Tarefa = tarefa;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!await repositorio.ExcluirAsync(id)) return NotFound();
        return RedirectToPage("/Quadro");
    }
}