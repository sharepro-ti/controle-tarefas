using ControleTarefas.Models;
using ControleTarefas.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ControleTarefas.Pages;

public class QuadroModel(ITarefaRepository repositorio) : PageModel
{
    public List<Tarefa> Tarefas { get; private set; } = [];

    [BindProperty]
    public Tarefa Tarefa { get; set; } = new();

    public async Task OnGetAsync() => Tarefas = await repositorio.ListarAsync();

    public async Task<IActionResult> OnPostCriarAsync()
    {
        if (!ModelState.IsValid || !Enum.IsDefined(Tarefa.Status))
            return BadRequest("Confira os campos obrigatórios, a data e o status da tarefa.");

        Tarefa.Id = 0;
        await repositorio.AdicionarAsync(Tarefa);
        return Partial("_Cartao", Tarefa);
    }

    public async Task<IActionResult> OnPostMoverAsync(int? id, StatusTarefa? status)
    {
        if (id is null || status is null || !Enum.IsDefined(status.Value))
            return BadRequest("Informe um status válido.");

        if (!await repositorio.MoverAsync(id.Value, status.Value))
            return NotFound("Tarefa não encontrada.");

        return new OkResult();
    }
}