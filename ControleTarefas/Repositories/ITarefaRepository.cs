using ControleTarefas.Models;

namespace ControleTarefas.Repositories;

public interface ITarefaRepository
{
    Task<List<Tarefa>> ListarAsync();
    Task<Tarefa?> ObterAsync(int id);
    Task AdicionarAsync(Tarefa tarefa);
    Task<bool> AtualizarAsync(Tarefa tarefa);
    Task<bool> ExcluirAsync(int id);
    Task<bool> MoverAsync(int id, StatusTarefa status);
}