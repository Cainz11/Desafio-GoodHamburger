# Perguntas e respostas técnicas

| Pergunta | Resposta |
|----------|----------|
| Por que quatro projetos? | Separação por responsabilidade: domínio sem dependência de infraestrutura ou HTTP; aplicação orquestra casos de uso e contratos; infraestrutura implementa persistência; API só expõe HTTP e configuração. |
| Onde ficam as regras de desconto? | No projeto `Domain`, em classes que implementam `IDiscountRule`, avaliadas em sequência por `DiscountEvaluator`. A primeira regra que “casa” define o percentual (combo 20% antes de S+D 15% e S+F 10%). |
| Por que não um único `if` para desconto? | Novas combinações viram novas classes sem editar um bloco central (extensível sem alterar o núcleo da avaliação). |
| Como o pedido impede dois sanduíches? | `Order.ReplaceLines` agrupa itens por `MenuItemRole` e rejeita mais de um por papel; IDs repetidos na lista também são rejeitados. |
| `DomainException` vs `NotFoundException` | `DomainException` cobre violação de regra de negócio (400). `NotFoundException` na camada de aplicação para recurso inexistente (404). Ambos viram `ProblemDetails` na API. |
| Por que FluentValidation e validação no domínio? | FluentValidation checa forma do request (ex.: no máximo 3 IDs). O domínio garante regras que dependem do cardápio e de papéis dos itens, onde a API não deveria duplicar lógica. |
| Preço do pedido é congelado na criação? | Não. O pedido guarda só `MenuItemId` nas linhas; subtotal e total usam preço atual do cardápio ao montar o DTO. Trade-off: simples; se precisar nota fiscal fixa, gravaria snapshot na linha. |
| Por que `Encrypt=False` na connection string? | Cliente SQL recente negocia TLS por padrão; o SQL Server em Docker costuma falhar no handshake com certificado não confiável. Em dev desliga criptografia na conexão; em produção use certificado válido e `Encrypt=True`. |
| O que é `EnableRetryOnFailure`? | Retenta operações do EF quando o SQL ainda está subindo ou há falha transitória de rede (comum com Docker no primeiro minuto). |
| Por que `Ignore(Lines)` no `Order` no EF? | A coleção pública `Lines` usa o mesmo campo `_lines` que o mapeamento `HasMany`. Sem `Ignore`, o modelo conflita. As linhas persistem pelo campo `_lines`. |
| `UpdateAsync` do repositório só chama `SaveChanges`? | Sim. O fluxo de atualização carrega o pedido com `GetByIdAsync` (rastreado), altera com `ReplaceLines` e persiste. `Update()` explícito seria redundante nesse fluxo. |
| Swagger só em Development? | Reduz superfície em produção e evita registrar serviços do Swashbuckle onde não são usados. |
| Por que IDs fixos no `MenuCatalog`? | Seed e testes estáveis; o cliente pode referenciar os mesmos GUIDs nos exemplos. |
| Erros da API seguem algum padrão? | JSON `application/problem+json` (RFC 7807), com `title`, `detail`, `status` e extensão `errorCode` para o cliente tratar sem parsear texto. |
| Onde estão os testes automatizados? | Projeto `GoodHamburger.Domain.Tests`: descontos e validações de pedido sem subir banco nem HTTP. |
| Por que não testes de integração na API? | Escopo do desafio; dá para acrescentar WebApplicationFactory + SQL local ou container depois. |
| Paginação em `GET /orders`? | Não implementada; lista completa. Para muitos pedidos, incluir `skip`/`take` ou cursor no repositório. |
| FK de `order_lines` para `menu_items`? | Sim, com `OnDelete(Restrict)` para não apagar item do cardápio se ainda houver linha referenciando. |
