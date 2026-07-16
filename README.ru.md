# WarehouseSystem

![CI](https://github.com/DmitriyOT/Warehouse-System/actions/workflows/ci.yml/badge.svg)
![.NET 9](https://img.shields.io/badge/.NET-9-512BD4?logo=dotnet)
![React](https://img.shields.io/badge/React-19-61DAFB?logo=react)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-4169E1?logo=postgresql)
![Docker](https://img.shields.io/badge/Docker-Compose-2496ED?logo=docker)

Пример системы управления складом (упрощённый вариант). Backend реализован на **ASP.NET Core 9**, frontend — на **React 19 + TypeScript + Vite**.

[English version](README.md)

## Технологии

- **Backend:** ASP.NET Core 9, Entity Framework Core, PostgreSQL, Swashbuckle (OpenAPI/Swagger), Serilog, health checks
- **Frontend:** React 19, TypeScript, Vite, MUI, Axios, React Router
- **Архитектура:** слоистая архитектура (Contracts → Domain → Infrastructure → Application → Api)
- **DevOps:** Docker Compose, GitHub Actions CI, `.editorconfig`, `global.json`

## Быстрый старт (Docker Compose)

Самый быстрый способ поднять весь стек:

```bash
git clone https://github.com/DmitriyOT/Warehouse-System.git
cd WarehouseSystem
docker compose up --build
```

Открой http://localhost:3000.

- Frontend: http://localhost:3000
- Backend API: http://localhost:5001
- Health backend: http://localhost:5001/health

## Локальная разработка

### Требования

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org/)
- [PostgreSQL](https://www.postgresql.org/)

### Backend

```bash
cp back/Warehouse.Api/appsettings.Development.json.example back/Warehouse.Api/appsettings.Development.json
# отредактируй строку подключения
cd back
dotnet restore
dotnet build
cd Warehouse.Api
dotnet run
```

API доступен по адресам:
- HTTP: http://localhost:5189
- HTTPS: https://localhost:7291
- Swagger: https://localhost:7291/swagger

### Frontend

```bash
cp front/.env.example front/.env
# VITE_APP_API_URL=https://localhost:7291/
cd front
npm install
npm run dev
```

Приложение откроется по адресу http://localhost:5173.

## Структура проекта

```
WarehouseSystem
├── back/
│   ├── Warehouse.Contracts      # Общие интерфейсы, DTO, исключения
│   ├── Warehouse.Domain         # Доменные модели
│   ├── Warehouse.Infrastructure # EF Core, репозитории, UoW, health checks
│   ├── Warehouse.Application    # Сервисы и бизнес-логика
│   ├── Warehouse.Api            # API-контроллеры и точка входа
│   └── Warehouse.Tests          # Тесты
├── front/                       # Клиент React + TypeScript + Vite
├── docker-compose.yml           # Стек из одной команды (Postgres + backend + frontend)
└── .github/workflows/ci.yml     # CI: сборка + тест backend, lint + build frontend
```

## Архитектурные решения

- **Унифицированный ответ API** — все ответы сервера возвращаются внутри `ResponseDTO` с полями `response`, `hasError`, `errorMessage`.
- **Слоистая архитектура backend** — повышение тестируемости, читаемости и снижение связности.
- **Типизированные CRUD-абстракции** — `CrudRepository`, `CrudService`, `BaseCrudController` с использованием `ExpressionTree` для фильтрации. Это уменьшает дублирование кода и позволяет создавать унифицированные компоненты.
- **Транзакции БД для остатков** — логика расчёта остатков вынесена в отдельный `BalanceService` и выполняется в транзакциях EF Core.
- **Централизованное отображение ошибок** — одно модальное окно для ошибок, доступное через контекст из любой части frontend.
- **Автоматические миграции БД** — EF Core миграции применяются при старте приложения.
- **Health checks** — эндпоинты `/health` и `/health/ready` для оркестрации контейнеров.

## Скриншоты

*Главная страница и справочники в Docker Compose.*

![Главная страница](docs/screenshot-dashboard.png)
![Справочник](docs/screenshot-directory.png)

## CI

При каждом push или pull request в `main`:

1. **Backend:** restore → build → test → проверка актуальности EF Core миграций.
2. **Frontend:** install → lint → build.

## Лицензия

Распространяется под лицензией MIT. См. [LICENSE](LICENSE).
