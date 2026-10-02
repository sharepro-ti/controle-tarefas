---
applyTo: "**/*.cs"
---

# Backend (C# / .NET 10)

## Princípios

- Código **mínimo e direto**, sem validações extras ou `try/catch` genérico. Verificar tarefa inexistente é necessário para responder HTTP 404.
- Usar file-scoped namespace, primary constructors e nullable habilitado. Não adicionar `record`, DTOs ou abstrações sem necessidade.
- Métodos assíncronos com sufixo `Async` e `await` do EF Core.

## Modelo

- `Tarefa`: `Id`, `Titulo`, `Descricao`, `Data` (`DateTime`), `Status`.
- `StatusTarefa` (enum): `Novo`, `Andamento`, `Bloqueado`, `Concluido`.
- Sem Data Annotations ou Fluent API, exceto o estritamente necessário.

## Repository

- Interface `ITarefaRepository` e implementação `TarefaRepository` em `Repositories/`.
- Métodos: `ListarAsync`, `ObterAsync(int id)`, `AdicionarAsync`, `AtualizarAsync`, `ExcluirAsync`, `MoverAsync(int id, StatusTarefa status)`.
- Retornos: `Task<List<Tarefa>>`, `Task<Tarefa?>`, `Task` e `Task<bool>` nos três últimos métodos, respectivamente.
- Métodos de escrita persistem com `SaveChangesAsync`; leituras não salvam alterações.
- Atualização, exclusão e movimentação retornam `bool` para o handler responder com HTTP 404 quando a tarefa não existe.
- Somente o repositório acessa os dados via `AppDbContext`; `Program.cs` o utiliza apenas para aplicar migrations.

## Entity Framework Core

- Provider `Microsoft.EntityFrameworkCore.Sqlite`; `AppDbContext` com `DbSet<Tarefa> Tarefas`.
- Configuração em `Program.cs` com `AddDbContext` e `UseSqlite` usando a string de conexão `Default`.
- Usar migrations (`dotnet ef`) e aplicar com `Database.Migrate()` na inicialização.
- Leituras sem alteração com `AsNoTracking()`.
- Ordenar a listagem por `Data` e `Id`; atualizar apenas os campos editáveis da tarefa existente, sem substituir a entidade com dados recebidos.
- Alinhar versões 10.0.x de `Microsoft.EntityFrameworkCore.Sqlite`, `Microsoft.EntityFrameworkCore.Design` e ferramenta local `dotnet-ef`; Design com `PrivateAssets=all`.
- Versões verificadas neste projeto: EF Core e dotnet-ef `10.0.9`, `SQLitePCLRaw.bundle_e_sqlite3` `2.1.13`. A dependência transitiva `2.1.11` gerou NU1903; usar a referência corrigida e verificar advisories ao atualizar.
- Gerar e versionar migration inicial e snapshot em `Data/Migrations` antes de executar; não substituir migrations por `EnsureCreated`.
- Comando: `dotnet ef migrations add CriacaoInicial --project ControleTarefas/ControleTarefas.csproj --output-dir Data/Migrations`.

## Razor Pages (PageModel)

- Injetar `ITarefaRepository` pelo construtor.
- `PageModel` fino: chama o repositório e expõe dados à view.
- `[BindProperty]` apenas nas propriedades de formulário.
- Em POSTs de formulário, verificar `ModelState` e `Enum.IsDefined` para status; retornar mensagens em português. Não adicionar bibliotecas de validação.
- No cadastro, zerar `Id` antes de adicionar e retornar `Partial("_Cartao", Tarefa)` após salvar. Em entrada inválida, HTTP 400 com mensagem; não redirecionar o `fetch`.
- Na edição, usar o `id` da rota, não confiar em `Id` enviado pelo formulário.
- Manter antiforgery padrão dos formulários e incluir seu token em `FormData` no `fetch`. Não desabilitar a proteção dos handlers de escrita.
- `OnPostMoverAsync`: HTTP 200 após persistir, 400 para entrada inválida e 404 para tarefa ausente.
- No JavaScript, verificar `response.ok`, exibir falhas e preservar dados/cartões; `catch` apenas para notificar erros de rede/HTTP, nunca para simular sucesso.
