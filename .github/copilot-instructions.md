# Controle de Tarefas

Aplicação web para controle de tarefas com visão Kanban. Projeto de demonstração do uso do GitHub Copilot (instruções, contexto e tokens), então o código deve ser **simples, objetivo e fácil de explicar**.

## Stack

- .NET SDK 10 (`net10.0`), ASP.NET Core **Razor Pages**
- Entity Framework Core + **SQLite**
- Bootstrap local e JavaScript puro (front-end, sem jQuery)

## Domínio

Entidade única `Tarefa`:

| Campo       | Descrição                                        |
|-------------|--------------------------------------------------|
| `Id`        | Identificador                                    |
| `Titulo`    | Título da tarefa                                 |
| `Descricao` | Descrição                                        |
| `Data`      | Data da tarefa                                   |
| `Status`    | `Novo`, `Andamento`, `Bloqueado`, `Concluido`    |

## Funcionalidades

- CRUD de tarefas.
- Criar tarefa em modal Bootstrap no próprio Kanban, via `fetch`, sem navegação ou recarga. Após salvar, inserir o cartão na coluna escolhida, atualizar contadores e fechar/limpar o modal.
- Editar e excluir em Razor Pages; a exclusão exige confirmação.
- Quadro Kanban com 4 colunas (Novo, Andamento, Bloqueado, Concluído); mover o card entre colunas altera o `Status`.

## Interface

- Layout com Bootstrap; **topo (navbar) predominantemente azul**.
- Colunas: Novo = azul, Andamento = amarelo, Bloqueado = vermelho, Concluído = verde.
- Layout responsivo; permitir alterar status também pelo formulário de edição.
- Em falhas de cadastro, manter o modal aberto com os dados e mostrar a mensagem. Bloquear envio duplicado enquanto salva.

## Princípios gerais

- Prefira a solução mais simples; não adicione recursos, camadas ou abstrações além do pedido.
- Validar apenas o necessário: campos obrigatórios, conversão de data, status válido e tarefa existente. Não criar regras extras ou `try/catch` genérico no backend.
- Manter antiforgery nos POSTs, inclusive via `fetch`; erros não podem parecer sucesso.
- Nomes de classes, propriedades e arquivos em **português**, sem acentos nos identificadores; textos da interface em português do Brasil.
- Respostas e comentários em português; comente o código apenas quando necessário.
- Preservar os nomes convencionais do framework (`Program.cs`, `AppDbContext`, `Models`, `Data`, `Repositories`, `Pages`, `wwwroot`). Usar português nos nomes do domínio e das páginas, com `Quadro` na rota `/`.

## Criação e verificação

- Ler estas instruções e as detalhadas antes de gerar código; preservar alterações existentes.
- Criar apenas um projeto `ControleTarefas`, com `dotnet new webapp -n ControleTarefas -o ControleTarefas --framework net10.0 --no-restore`.
- Remover as páginas de exemplo, CSS isolado do layout e referências a jQuery do template. Reutilizar o Bootstrap local gerado; não adicionar frameworks.
- Usar sempre o caminho completo do projeto nos comandos: `--project ControleTarefas/ControleTarefas.csproj`. Metadados `._*.csproj` em volumes macOS podem tornar o diretório ambíguo.
- Antes do primeiro build, excluir `**/._*` dos itens do SDK no `.csproj` com `DefaultItemExcludes`; somente `.gitignore` não impede erros de compilação de arquivos binários.
- Versionar `global.json` para SDK 10 com `rollForward: latestFeature` e manifesto local de `dotnet-ef`. Não depender de ferramentas globais.
- Ignorar `bin`, `obj`, `tarefas.db*`, `.DS_Store` e `._*` no Git; não apagar metadados ou arquivos do usuário como solução.
- Compilar o `.csproj` e verificar dependências transitivas com `dotnet list ControleTarefas/ControleTarefas.csproj package --vulnerable --include-transitive`.
- Validar no navegador: modal sem navegação/recarga, cancelamento, falha com dados preservados, CRUD, quatro status com persistência após recarregar, contadores e layout móvel.
- Usar banco de validação separado via `ConnectionStrings:Default`, sem alterar o banco do usuário. Encerrar apenas os processos iniciados para teste e remover apenas os artefatos de teste criados.
- Documentar execução e migrations no README; não criar projetos ou ferramentas extras de teste para esta demonstração.

## Instruções detalhadas

- Arquitetura: [arquitetura.instructions.md](instructions/arquitetura.instructions.md)
- Backend: [backend.instructions.md](instructions/backend.instructions.md)
