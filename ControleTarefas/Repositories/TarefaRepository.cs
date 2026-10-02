using ControleTarefas.Data;
using ControleTarefas.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleTarefas.Repositories;

public class TarefaRepository(AppDbContext contexto) : ITarefaRepository
{
    public Task<List<Tarefa>> ListarAsync() => contexto.Tarefas.AsNoTracking()
        .OrderBy(tarefa => tarefa.Data).ThenBy(tarefa => tarefa.Id).ToListAsync();

    public Task<Tarefa?> ObterAsync(int id) => contexto.Tarefas.AsNoTracking()
        .FirstOrDefaultAsync(tarefa => tarefa.Id == id);

    public async Task AdicionarAsync(Tarefa tarefa)
    {
        contexto.Tarefas.Add(tarefa);
        await contexto.SaveChangesAsync();
    }

    public async Task<bool> AtualizarAsync(Tarefa tarefa)
    {
        var existente = await contexto.Tarefas.FindAsync(tarefa.Id);
        if (existente is null) return false;

        existente.Titulo = tarefa.Titulo;
        existente.Descricao = tarefa.Descricao;
        existente.Data = tarefa.Data;
        existente.Status = tarefa.Status;
        await contexto.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExcluirAsync(int id)
    {
        var tarefa = await contexto.Tarefas.FindAsync(id);
        if (tarefa is null) return false;

        contexto.Tarefas.Remove(tarefa);
        await contexto.SaveChangesAsync();
        return true;
    }

    public async Task<bool> MoverAsync(int id, StatusTarefa status)
    {
        var tarefa = await contexto.Tarefas.FindAsync(id);
        if (tarefa is null) return false;

        tarefa.Status = status;
        await contexto.SaveChangesAsync();
        return true;
    }
}