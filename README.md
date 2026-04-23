# Good Hamburger — API

API REST (.NET 10) para pedidos da lanchonete: cardápio, CRUD de pedidos, descontos e validações de composição do pedido.

## Subir o banco

```powershell
docker compose up -d
```

Credenciais locais: usuário `sa`, senha `GoodHamburger@2026` (configurado no `docker-compose.yml` e `appsettings.json`).

## Rodar a API

```powershell
dotnet restore GoodHamburger.slnx
dotnet run --project src/GoodHamburger.Api
```

Migrações rodam na subida (`MigrateAsync`).

**Swagger:** com `ASPNETCORE_ENVIRONMENT=Development` (padrão do `launchSettings`), abra `http://localhost:5267/swagger` ou `https://localhost:7283/swagger` no perfil https. Arquivo `.http` na pasta da Api para testes rápidos.

## Rotas

| Método | Rota |
|--------|------|
| GET | `/api/menu` |
| GET | `/api/orders` |
| GET | `/api/orders/{id}` |
| POST | `/api/orders` |
| PUT | `/api/orders/{id}` |
| DELETE | `/api/orders/{id}` |

Body de criação/atualização: `{ "menuItemIds": [ "guid", ... ] }` (até 3 itens, sem duplicar categoria).

## Testes

```powershell
dotnet test GoodHamburger.slnx
```

## Migração nova

```powershell
dotnet ef migrations add Nome --project src/GoodHamburger.Infrastructure --startup-project src/GoodHamburger.Api --context AppDbContext --output-dir Persistence/Migrations
```
