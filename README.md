# Controle de Tarefas

Aplicação Kanban em português, com ASP.NET Core Razor Pages (.NET 10), EF Core, SQLite, Bootstrap local e JavaScript puro.

## Executar

Instale o SDK .NET 10. Execute os comandos na raiz do repositório:

```powershell
dotnet restore ControleTarefas/ControleTarefas.csproj
dotnet tool restore
dotnet run --project ControleTarefas/ControleTarefas.csproj --launch-profile http
```

Acesse http://localhost:5071. O quadro fica na rota `/`. As migrations são aplicadas na inicialização e o banco `tarefas.db` é criado no diretório do projeto. A conexão `Default` está em `ControleTarefas/appsettings.json`.

Se a porta estiver ocupada:

```powershell
dotnet run --project ControleTarefas/ControleTarefas.csproj --launch-profile http --urls http://localhost:5072
```

## Funcionalidades

- Cadastro no modal do quadro, sem navegação ou recarga.
- Edição de título, descrição, data e status.
- Exclusão com confirmação.
- Quatro colunas: Novo, Andamento, Bloqueado e Concluído.
- Movimentação por arrastar e soltar, persistida antes da mudança visual.
- Contadores atualizados após cadastro e movimentação; layout responsivo.
- Em dispositivos sem drag and drop, o status pode ser alterado na edição.

## Organização

`Pages` → `Repositories` → `Data/AppDbContext` → SQLite.

`Models` contém `Tarefa` e `StatusTarefa`. As partials `_Formulario` e `_Cartao` ficam em `Pages/Shared`. Não há camada de serviço, SPA ou jQuery. Os POSTs usam antiforgery, inclusive os enviados por `fetch`.

## Migrations

O manifesto local `dotnet-tools.json` fixa o `dotnet-ef` na mesma versão do EF Core. A migration inicial e o snapshot estão em `ControleTarefas/Data/Migrations`.

```powershell
dotnet tool restore
dotnet ef migrations add NomeDaAlteracao --project ControleTarefas/ControleTarefas.csproj --output-dir Data/Migrations
dotnet ef database update --project ControleTarefas/ControleTarefas.csproj
```

## Verificação

```powershell
dotnet build ControleTarefas/ControleTarefas.csproj
dotnet list ControleTarefas/ControleTarefas.csproj package --vulnerable --include-transitive
```

Para validar sem usar o banco padrão, escolha um caminho de teste novo:

```powershell
dotnet run --project ControleTarefas/ControleTarefas.csproj --launch-profile http --urls http://localhost:5072 --ConnectionStrings:Default="Data Source=validacao.db"
```

Confira no navegador: cadastro sem recarga, cancelamento, falha com dados preservados, edição, confirmação de exclusão, movimentação nos quatro status, persistência após recarregar, contadores e modal em telas móveis e baixas. Ao terminar, encerre esse processo e remova apenas o banco de validação criado e seus arquivos auxiliares.