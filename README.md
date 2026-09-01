# TagPerson (Tagmar) - Angular 20 + API C# (.NET 9) + MySQL

Este pacote é o *starter* do backend + importador para migrar o seu SQLite (`tagperson.db`) para MySQL e expor uma API REST
para o frontend Angular.

## O que vem aqui
- `docker-compose.yml` + `db/init.sql` (schema MySQL)
- `backend/TagPerson.Api` (API .NET 9 com EF Core + Swagger)
- `backend/TagPerson.Importer` (Console app: SQLite -> MySQL)
- Endpoints principais:
  - `GET /api/characters` (lista)
  - `GET /api/characters/{id}/sheet` (ficha completa + derivados calculados)
  - `PUT /api/characters/{id}` (atualiza base do personagem)
  - `PUT /api/characters/{id}/equipment` (equipar/desequipar slots)

## 1) Subir MySQL
```bash
docker compose up -d
```

## 2) Configurar connection string
No `backend/TagPerson.Api/appsettings.json` e `backend/TagPerson.Importer/appsettings.json`:
```json
"ConnectionStrings": {
  "MySql": "Server=localhost;Port=3306;Database=tagperson;User=tagperson;Password=tagperson;"
}
```

## 3) Rodar o Importador
O importador lê o SQLite e joga no MySQL.
- Coloque seu arquivo SQLite em `backend/TagPerson.Importer/input/tagperson.db`
- Rode:
```bash
dotnet run --project backend/TagPerson.Importer/TagPerson.Importer.csproj
```

## 4) Rodar a API
```bash
dotnet run --project backend/TagPerson.Api/TagPerson.Api.csproj
```

Swagger: `/swagger`

## Observação importante
Este pacote foi gerado sem build aqui no sandbox (não tem SDK dotnet instalado).
O código está pronto pra você abrir no VS/VSCode e rodar local com .NET 9.

