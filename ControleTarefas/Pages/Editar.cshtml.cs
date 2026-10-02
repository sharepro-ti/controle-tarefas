using ControleTarefas.Models;
using ControleTarefas.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ControleTarefas.Pages;

public class EditarModel(ITarefaRepository repositorio) : PageModel
{
    [BindProperty]
    public Tarefa Tarefa { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var tarefa = await repositorio.ObterAsync(id);
        if (tarefa is null) return NotFound();

        Tarefa = tarefa;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!Enum.IsDefined(Tarefa.Status))
            ModelState.AddModelError("Tarefa.Status", "Selecione um status válido.");

        if (!ModelState.IsValid) return Page();

        Tarefa.Id = id;
        if (!await repositorio.AtualizarAsync(Tarefa)) return NotFound();

        return RedirectToPage("/Quadro");
    }
}