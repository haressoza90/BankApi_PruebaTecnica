# Prueba Tecnica - para una API Bancaria (.NET 8)


Hola, le entrego mi API bancaria, cumpliendo mayoria sino es que todo en **.NET 8** con **ASP.NET Web API**, **Entity Framework Core (SQLite)**, **Swagger**, e **inyección de dependencias**. Incluye **pruebas unitarias** con xUnit y FluentAssertions, agregue un documentado por que queria estar segura de que si funciona o no, 

## Requisitos
- .NET 8 SDK
- Visual Studio Code (recomendado)

## Ejecutar
```bash
dotnet restore
dotnet build
dotnet test
dotnet run --project src/BankApi/BankApi.csproj
```
Se abrira Swagger en el navegador (perfil de lanzamiento apunta a `/swagger`).

## Endpoints
- `POST /api/customers` para crear cliente `{ name, birthDate, sex, income }`
- `GET /api/customers/{id}` para obtener cliente
- `POST /api/accounts` este para crear cuenta `{ customerId, accountNumber, initialBalance }`
- `GET /api/accounts/{accountNumber}/balance` aqui consulto saldo
- `GET /api/accounts/{accountNumber}/transactions` para historial
- `POST /api/accounts/{accountNumber}/apply-interest` para el rate `{ rate }`
- `POST /api/transactions/deposit` → `{ accountNumber, amount }`
- `POST /api/transactions/withdraw` → `{ accountNumber, amount }`

## Persistencia
sqlite`bank.db` se crea automaticamente


gracias 