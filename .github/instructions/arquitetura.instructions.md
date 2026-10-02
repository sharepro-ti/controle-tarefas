---
applyTo: "**"
---

# Arquitetura

## Estrutura

Projeto único Razor Pages, organizado por responsabilidade:

```
ControleTarefas/
├── Models/          # Entidades e enums (Tarefa, StatusTarefa)
├── Data/            # AppDbContext e migrations
├── Repositories/    # ITarefaRepository e TarefaRepository
├── Pages/           # Razor Pages (PageModel + .cshtml)
├── wwwroot/         # CSS, JS e Bootstrap
└── Program.cs       # Configuração e injeção de dependência
```

## Fluxo

`Razor Page (PageModel)` → `Repository` → `AppDbContext (EF Core)` → `SQLite`

## Regras

- Não há camada de serviço: o `PageModel` usa o repositório diretamente.
- O `PageModel` **nunca** acessa o `AppDbContext`; o repositório acessa os dados. `Program.cs` pode obter o contexto em um escopo apenas para aplicar migrations.
- Dependências registradas em `Program.cs` via injeção de dependência (`AddScoped` para o repositório).
- Não criar projetos adicionais, Clean Architecture, CQRS, MediatR, AutoMapper ou DTOs.
- Banco SQLite em arquivo local (`tarefas.db`), string de conexão em `appsettings.json`.
- Alteração de status no Kanban via handler da Razor Page (ex.: `OnPostMoverAsync`) chamado por JavaScript (`fetch`) no drag and drop.
- Cadastro via modal em `Quadro.cshtml` e handler `OnPostCriarAsync`; não criar uma página separada de cadastro.
- Modal com `modal-dialog-scrollable` e formulário como `modal-content`, mantendo cabeçalho/rodapé fora da área rolável, inclusive em telas baixas.
- O handler de cadastro salva pelo repositório e retorna o HTML da partial `_Cartao`; o JavaScript insere o cartão sem recarregar ou navegar.
- Reutilizar `_Cartao` para renderização inicial e resposta do cadastro; `_Formulario` para modal e edição. Ambas ficam em `Pages/Shared`.
- Centralizar atualização dos contadores e avisos de coluna vazia no JavaScript, reutilizando em cadastro e movimentação.
- Usar delegação de eventos para arrastar cartões recém-criados; mover o cartão visualmente somente após sucesso do servidor.
- Front-end: Bootstrap local em `wwwroot`, JavaScript puro (sem frameworks SPA ou jQuery).
- Página `Erro` em português para falhas de produção, com identificador da solicitação e sem detalhes internos. Aceitar GET e POST no reprocessamento do exception handler.
