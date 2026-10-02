using System.ComponentModel.DataAnnotations;

namespace ControleTarefas.Models;

public enum StatusTarefa
{
    Novo,
    Andamento,
    Bloqueado,
    [Display(Name = "Concluído")]
    Concluido
}