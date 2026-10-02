using ControleTarefas.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleTarefas.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Tarefa> Tarefas => Set<Tarefa>();
}