# План разработки дашборда склада

Референс: `docs/Screenshot 2026-07-30 203414.png` (исходник макета — SVG-компонент
`WarehousePreview` в `portfolio-site/app/src/components/previews.tsx:115-186`).

Цель: заменить пустую стартовую страницу (`/`) полноценным дашбордом в стиле референса:
тёмная тема, 4 KPI-карточки, столбчатый график движения товаров за неделю, список
последних операций, статус-бар здоровья системы, бейджи технологий.

---

## 1. Что есть сейчас

**Backend** (`back/`, ASP.NET Core 9 + EF Core + PostgreSQL):

- Только CRUD/grid-эндпоинты: `GET /{Controller}/getItem`, `POST /{Controller}/getAll`
  (`BaseCrudController.cs`), агрегаций для дашборда нет — нужен новый эндпоинт.
- `GET /health` уже настроен (`Program.cs:160-164`, проверка NpgSql).
- Миграции применяются при старте (`Program.cs:115-119`, `context.Database.Migrate()`).
- Сущности: `BalanceEntity{ResourceId, UnitId, Quantity}`, `IncomeEntity{Number, Date, IncomeItems}`,
  `ShipmentEntity{Number, Date, ClientId, IsApprove, ShipmentItems}`, справочники
  `Client/Resource/Unit` с `IsArchive`.

**Frontend** (`front/`, React 19 + Vite, axios, Bootstrap 5, MUI DataGrid, plain CSS):

- Маршрут `/` → `BasePage.tsx` — пустой placeholder, туда и встаёт дашборд.
- API-слой: `src/api/Api.ts` (`$host`, `baseURL = VITE_APP_API_URL`).
- Меню `LeftMenuComponent.tsx`: группы «Склад» (Баланс/Поступления/Отгрузки),
  «Справочники» (Клиенты/Ед. измерения/Ресурсы) — совпадает с сайдбаром макета.
- Тема сейчас светлая (`index.css`, `body` `rgb(241,241,232)`); тёмная тема дашборда
  делается локально, без переделки остальных страниц.
- Графических библиотек нет — график рисуем простым SVG (как в макете), без новых зависимостей.

**Тесты**: backend — xUnit (`Warehouse.Tests`, паттерн fake-репозиториев из `BalanceServiceTests`);
frontend — тестов нет. CI (`.github/workflows/ci.yml`): build + test + lint + проверка миграций.

---

## 2. Дизайн-токены (из макета, дословно)

```css
--panel:      #0E1016;  /* фон окна */
--panel-2:    #12151D;  /* сайдбар, карточки, панели */
--line:       rgba(255,255,255,0.08);  /* hairline-бордеры */
--line-strong:rgba(255,255,255,0.16);  /* ось графика */
--text:       #9AA1B2;
--text-dim:   #5B6172;  /* подписи */
--heading:    #E8EAF0;  /* заголовки групп */
--value:      #EEF0F5;  /* значения KPI */
--gold:       #E2B15C;  /* акцент: активный пункт меню, пиковый столбец */
--gold-dim:   rgba(226,177,92,0.16);
--indigo:     #8B93F8;  /* столбцы графика */
--green:      #5FC98A;  /* OK, положительные дельты, приход */
--green-dim:  rgba(95,201,138,0.16);   /* фон health-бара */
--red:        #E4685C;  /* отрицательные дельты, расход */
```

Типографика макета: Manrope (заголовки, KPI) + JetBrains Mono (дельты, операции, health,
бейджи). Радиусы: карточки/панели 9–10px, пункты меню 6px, health-бар 8px, бейджи — pills.
В проекте этих шрифтов нет — подключить через `@fontsource` (как в portfolio-site) или
ограничиться системными fallback (решить на этапе реализации; `@fontsource` — две
зависимости, работает офлайн).

---

## 3. Архитектура решения

### Backend: один агрегирующий эндпоинт

Новый `DashboardController` (`back/Warehouse.Api/Controllers/DashboardController.cs`),
**не** наследуется от CRUD-базовых:

```
GET /Dashboard/summary → ResponseDto<DashboardSummaryDto>
```

`DashboardSummaryDto` (в `back/Warehouse.Contracts/Api/Response/Dtos/`):

```csharp
public class DashboardSummaryDto
{
    public required DashboardKpisDto Kpis { get; set; }
    public required List<DashboardDayDto> WeekMovement { get; set; }   // 7 дней
    public required List<DashboardOperationDto> LastOperations { get; set; } // 5 шт.
}

public class DashboardKpisDto
{
    public decimal TotalBalance { get; set; }        // сумма Balance.Quantity по неархивным ресурсам
    public decimal BalanceDeltaPercent { get; set; } // vs неделю назад (приход-расход за неделю / текущий остаток)
    public int IncomeCount { get; set; }             // документов поступления за неделю
    public int IncomeDelta { get; set; }             // vs предыдущая неделя
    public int ShipmentCount { get; set; }           // отгрузок за неделю (все статусы)
    public int ShipmentDelta { get; set; }
    public int ActiveClientCount { get; set; }       // IsArchive == false
    public int ClientDelta { get; set; }             // новые клиенты за неделю (если даты создания нет — 0, см. замечание ниже)
}

public class DashboardDayDto { public DateOnly Date; public decimal Income; public decimal Shipment; }
public class DashboardOperationDto { public string ResourceName; public decimal Quantity; } // +приход / −отгрузка
```

Логика — в `Warehouse.Application` (`IDashboardService` / `DashboardService`), запросы через
существующие репозитории/`DbContext` с агрегацией на стороне БД (`GroupBy`/`Sum`), без
выгрузки таблиц в память.

**Замечания / известные ограничения:**
- У справочников нет даты создания → `ClientDelta` считать невозможно. Вариант А: вернуть
  0 и не показывать дельту; вариант Б (опционально): добавить `CreatedAt` в
  `BaseEntityWithIdArchiveName` + миграция. В плане — вариант А, Б отдельной задачей.
- Дельта остатка: баланс — это текущее состояние, истории нет. Считаем приближённо:
  `остаток_неделю_назад = текущий − приходы_недели + отгрузки_недели` (только подтверждённые
  отгрузки, `IsApprove = true` — они реально списывают баланс, см. `BalanceService`).
- `GET /health` уже отдаёт статус postgres — frontend опрашивает его напрямую, на бэке
  ничего не меняем. «Миграции применены» и «CI passed» в health-баре — статичные подписи
  (применение миграций гарантировано стартом приложения), либо health-бар упрощаем до
  реальных двух статусов.

### Frontend: страница дашборда на месте BasePage

Новые файлы в `front/src/`:

```
src/app/dashboard/
  DashboardPage.tsx        — заменяет содержимое BasePage.tsx (роут "/")
  DashboardPage.css        — тёмная тема, скоуплена классом .dashboard (не трогаем index.css глобально)
  components/
    StatCard.tsx           — label + value + дельта (зелёная/красная)
    WeekChart.tsx          — SVG-столбцы за 7 дней, пиковый день — gold, без библиотек
    OperationsList.tsx     — последние операции, точка-статус + моноширинный текст
    HealthBar.tsx          — опрос GET /health, пульсирующая точка (CSS-анимация, prefers-reduced-motion)
    TechChips.tsx          — .NET 9 / EF Core / PostgreSQL / React 19 / Docker (статично)
src/api/dashboardApi.ts    — getSummary(): axios GET /Dashboard/summary через $host
src/types/Dashboard.ts     — типы DTO (зеркало контрактов)
```

- Данные: `useEffect` + один запрос summary при монтировании + `GET /health`; без новых
  state-менеджеров (в проекте только React Context).
- Меню: в `LeftMenuComponent.tsx` добавить пункт «Дашборд» в группу «Склад» (первым),
  активное состояние — в золотом акценте макета. Сайдбар целиком можно перекрасить
  в `--panel-2` как на макете — но это уже решение по объёму (см. этап 4).
- Компоновка: CSS grid внутри `.dashboard`: 4 колонки KPI → строка «график (≈57%) +
  операции (≈42%)» → health-бар на всю ширину → ряд бейджей. Отступы 24px между карточками,
  16px внутри панелей — ритм макета.
- MUI/Bootstrap на этой странице не использовать — чистый CSS по токенам, чтобы точно
  попасть в макет.

---

## 4. Этапы реализации

### Этап 1. Backend: эндпоинт агрегации (≈ основной объём)

1. DTO в `back/Warehouse.Contracts/Api/Response/Dtos/DashboardSummaryDto.cs` (+ `DashboardKpisDto`,
   `DashboardDayDto`, `DashboardOperationDto`).
2. `IDashboardService` + `DashboardService` в `Warehouse.Application`:
   - `TotalBalance`: `Sum(Balance.Quantity)` с join на неархивные ресурсы/единицы;
   - счётчики документов за текущую и предыдущую неделю (`Income.Date`, `Shipment.Date`);
   - `WeekMovement`: `GroupBy(Date)` приходов и подтверждённых отгрузок за 7 дней,
     с заполнением нулями пустых дней;
   - `LastOperations`: `Union` последних позиций приходов (+) и отгрузок (−), сортировка
     по дате документа, топ-5.
3. Регистрация сервиса в DI (по образцу существующих `*ServiceCollectionExtensions`).
4. `DashboardController` с `GET /Dashboard/summary`, обёртка в `ResponseDto<>`, ошибки —
   через существующий middleware (`UserException` → 400).
5. Тесты `DashboardServiceTests` в `Warehouse.Tests` по паттерну `BalanceServiceTests`
   (fake-репозитории): суммы, дельты, неделя с пустыми днями, знаки операций.
6. Проверка: `dotnet test`, `dotnet ef migrations has-pending-model-changes` (модель не
   меняется — миграция не нужна).

### Этап 2. Frontend: каркас и данные

1. `src/types/Dashboard.ts`, `src/api/dashboardApi.ts` (через `$host`).
2. `DashboardPage.tsx`: загрузка summary + health, состояния loading/error (error — через
   существующий `ModalContext`).
3. `DashboardPage.css`: дизайн-токены (CSS-переменные), grid-компоновка.
4. Подключение страницы к роуту `/` вместо пустого `BasePage` (содержимое BasePage.tsx
   заменяется на `<DashboardPage/>`).

### Этап 3. Frontend: визуальные компоненты

1. `StatCard` ×4 — «Остаток, шт», «Поступления», «Отгрузки», «Клиенты» с дельтами
   (формат чисел с разделителем тысяч, как `12 480`).
2. `WeekChart` — SVG 7 столбцов (rx=4, indigo 0.55, пик — gold), базовая линия,
   заголовок «Движение товаров · неделя». Значение столбца = приход+отгрузка за день.
3. `OperationsList` — «Последние операции», 5 строк: зелёная/красная точка +
   `+500 · Сталь 08пс` моноширинным.
4. `HealthBar` — `GET /health`: всё Healthy → зелёный бар; иначе красный с текстом ошибки.
   Пульсация точки — `@keyframes`, отключение при `prefers-reduced-motion`.
5. `TechChips` — статичный ряд бейджей.
6. Шрифты: `@fontsource/manrope` + `@fontsource/jetbrains-mono` (cyrillic subset) —
   подтвердить, что добавление двух пакетов допустимо; иначе системные fallback.
7. Пункт «Дашборд» в `LeftMenuComponent.tsx`.

### Этап 4. (Опционально) Полное соответствие макету

- Перекраска сайдбара и фона приложения в тёмную тему на всех страницах (в макете тёмный
  весь интерфейс, а не только дашборд). Отдельным PR: глобальные CSS-переменные в
  `index.css`, темизация MUI `GridTheme.ts`.
- `CreatedAt` для справочников → настоящая дельта «Клиенты +2» (миграция БД).

### Этап 5. Проверка и CI

1. `npm run lint`, `npm run build` (front); `dotnet build`, `dotnet test` (back) — локально.
2. Ручная проверка: docker-compose up → открыть `/` → сверка с референсом (скриншот в docs/).
3. CI уже покрывает обе части, новых job'ов не нужно.

---

## 5. Порядок коммитов (предложение)

1. `back`: DTO + DashboardService + тесты + контроллер.
2. `front`: типы, api, DashboardPage с компонентами, пункт меню.
3. (опционально) тёмная тема приложения.
4. Обновить `README.md` / `README.ru.md` скриншотом нового дашборда.

## 6. Риски

- **Дельты без истории**: приближённый расчёт (см. §3) может расходиться с ожиданием —
  зафиксировать формулу в тестах.
- **Формат `getAll` не подходит** для агрегатов — именно поэтому новый эндпоинт, а не
  клиентская агрегация (на больших объёмах выгрузка недопустима).
- **`DateOnly` в `Income.Date`/`Shipment.Date`**: неделя считается по `DateOnly.FromDateTime(DateTime.UtcNow)` — учесть таймзону, зафиксировать в тестах.
- Сборка фронта запекает `VITE_APP_API_URL` на build-time — при проверке в docker-compose
  использовать корректный URL API.
