# WarehouseSystem

Пример системы управления складом (упрощённый вариант). Backend реализован на ASP.NET Core, frontend — на React + TypeScript + Vite.

## Технологии

- **Backend:** ASP.NET Core 9, Entity Framework Core, PostgreSQL, Swashbuckle (OpenAPI/Swagger)
- **Frontend:** React 19, TypeScript, Vite, MUI, Axios, React Router
- **Архитектура:** слоистая архитектура (Contracts → Domain → Infrastructure → Application → Api)

## Требования

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org/)
- [PostgreSQL](https://www.postgresql.org/) (локальный сервер)

## Быстрый старт

### 1. Клонирование и настройка окружения

```bash
git clone https://github.com/DmitriyOT/Warehouse-System.git
cd WarehouseSystem
```

Создайте локальные конфиги на основе примеров:

```bash
# Backend
cp back/Warehouse.Api/appsettings.Development.json.example back/Warehouse.Api/appsettings.Development.json

# Frontend
cp front/.env.example front/.env
```

Отредактируйте созданные файлы и укажите свои значения (строку подключения к БД, пароль и т.д.).

### 2. Запуск backend

```bash
cd back
dotnet restore
dotnet build
cd Warehouse.Api
dotnet run
```

По умолчанию API доступен по адресам:
- HTTP: `http://localhost:5189`
- HTTPS: `https://localhost:7291`
- Swagger: `https://localhost:7291/swagger`

### 3. Запуск frontend

```bash
cd front
npm install
npm run dev
```

Приложение откроется по адресу `http://localhost:5173` (или другому, который покажет Vite).

## Структура проекта

```
WarehouseSystem
├── back/                    # Серверная часть (ASP.NET Core)
│   ├── Warehouse.Contracts  # Общие сущности, интерфейсы, DTO
│   ├── Warehouse.Domain     # Доменные модели
│   ├── Warehouse.Infrastructure # Работа с БД, репозитории, внешние сервисы
│   ├── Warehouse.Application    # Сервисы и бизнес-логика
│   ├── Warehouse.Tests      # Тесты
│   └── Warehouse.Api        # API-контроллеры и точка входа
│
└── front/                   # Клиентская часть (React + TS + Vite)
    ├── api/                 # Подключение к backend
    ├── app/                 # Страницы приложения
    ├── assets/              # Статические ассеты
    ├── components/          # Компоненты интерфейса
    │   ├── menu/            # Компоненты меню
    │   ├── pure/            # Чистые компоненты
    │   └── style/           # Стилистические компоненты
    ├── types/               # TypeScript-типы
    └── utils/               # Вспомогательные утилиты
```

## Архитектурные решения

- **Унифицированный ответ API** — все ответы сервера возвращаются с кодом 200 внутри `ResponseDTO` с полями `response`, `hasError`, `errorMessage`.
- **Чистые компоненты на frontend** — упрощение компонентов и логики работы с ними.
- **Слоистая архитектура backend** — повышение тестируемости, читаемости и снижение связности.
- **Централизованное отображение ошибок** — одно модальное окно для ошибок, доступное через контекст из любой части frontend.
- **Типизированные CROC-абстракции** — `CrudRepository`, `CrudService`, `BaseCrudController` с использованием `ExpressionTree` для фильтрации. Это уменьшает дублирование кода и позволяет создавать унифицированные компоненты.
- **Генераторы страниц с параметрами** — снижение дублирования кода на frontend.
- **TypeScript** — повышение надёжности frontend благодаря статической типизации и линтингу.
- **Транзакции БД для остатков** — логика расчёта остатков вынесена в отдельный `BalanceService` и выполняется в транзакциях.

## База данных

Проект использует PostgreSQL. Строка подключения задаётся в `back/Warehouse.Api/appsettings.Development.json` (см. пример `appsettings.Development.json.example`).

> **Важно:** файлы `appsettings.Development.json` и `front/.env` содержат локальные настройки и пароли, поэтому они исключены из репозитория через `.gitignore`. Не коммитьте их в публичный репозиторий.

## Лицензия

Распространяется под лицензией MIT. См. файл [LICENSE](LICENSE).
