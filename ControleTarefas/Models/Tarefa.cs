using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ControleTarefas.Models;

public class Tarefa
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Informe o título.")]
    public string Titulo { get; set; } = "";

    [Required(ErrorMessage = "Informe a descrição.")]
    public string Descricao { get; set; } = "";

    [BindRequired]
    public DateTime Data { get; set; } = DateTime.Today;

    [BindRequired]
    public StatusTarefa Status { get; set; }
}