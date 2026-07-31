-- Демо-данные для проверки дашборда (локальный запуск, БД warehouse)
BEGIN;

INSERT INTO "Units" ("IsArchive", "Name") VALUES
  (false, 'шт'), (false, 'кг');

INSERT INTO "Resources" ("IsArchive", "Name") VALUES
  (false, 'Сталь 08пс'), (false, 'Профиль 40×40'), (false, 'Лист 2мм'),
  (false, 'Труба 57×3'), (false, 'Круг 12мм'), (true, 'Стружка (архив)');

INSERT INTO "Clients" ("Address", "IsArchive", "Name") VALUES
  ('г. Москва, ул. Промышленная, 12', false, 'ООО «Металлторг»'),
  ('г. Екатеринбург, пр. Ленина, 45', false, 'ЗАО «Строймонтаж»'),
  ('г. Казань, ул. Баумана, 3', false, 'ИП Сидоров А.П.'),
  ('г. Челябинск, ул. Заводская, 8', false, 'ООО «Уралмет»'),
  ('г. Санкт-Петербург, наб. Обводного канала, 90', false, 'АО «Трубпром»'),
  ('—', true, 'ООО «Старт» (архив)');

-- Приходы: предыдущая неделя (для дельт) + текущая неделя 2026-07-25..31
INSERT INTO "Incomes" ("Number", "Date") VALUES
  ('П-098', '2026-07-20'), ('П-099', '2026-07-22'), ('П-100', '2026-07-23'),
  ('П-101', '2026-07-25'), ('П-102', '2026-07-26'), ('П-103', '2026-07-26'),
  ('П-104', '2026-07-28'), ('П-105', '2026-07-29'), ('П-106', '2026-07-30'),
  ('П-107', '2026-07-31');

INSERT INTO "IncomeItems" ("IncomeId", "ResourceId", "UnitId", "Quantity")
SELECT i."Id", r."Id", u."Id", v.qty FROM (VALUES
  ('П-098', 'Круг 12мм',     'кг', 500),
  ('П-099', 'Труба 57×3',    'шт', 200),
  ('П-100', 'Сталь 08пс',    'шт', 400),
  ('П-101', 'Сталь 08пс',    'шт', 500),
  ('П-102', 'Лист 2мм',      'шт', 240),
  ('П-103', 'Круг 12мм',     'кг', 1000),
  ('П-104', 'Труба 57×3',    'шт', 300),
  ('П-105', 'Профиль 40×40', 'шт', 450),
  ('П-106', 'Сталь 08пс',    'шт', 700),
  ('П-107', 'Лист 2мм',      'шт', 180)
) AS v(num, res, unit, qty)
JOIN "Incomes" i ON i."Number" = v.num
JOIN "Resources" r ON r."Name" = v.res
JOIN "Units" u ON u."Name" = v.unit;

-- Отгрузки: часть подтверждена (IsApprove), одна черновая
INSERT INTO "Shipments" ("Number", "Date", "ClientId", "IsApprove")
SELECT v.num, v.dt, c."Id", v.appr FROM (VALUES
  ('О-198', date '2026-07-21', 'ООО «Металлторг»',  true),
  ('О-199', date '2026-07-23', 'АО «Трубпром»',     true),
  ('О-201', date '2026-07-26', 'ЗАО «Строймонтаж»', true),
  ('О-202', date '2026-07-28', 'ООО «Уралмет»',     true),
  ('О-203', date '2026-07-29', 'ООО «Металлторг»',  true),
  ('О-204', date '2026-07-31', 'ИП Сидоров А.П.',   true),
  ('О-205', date '2026-07-31', 'ЗАО «Строймонтаж»', false)
) AS v(num, dt, client, appr)
JOIN "Clients" c ON c."Name" = v.client;

INSERT INTO "ShipmentItems" ("ShipmentId", "ResourceId", "UnitId", "Quantity")
SELECT s."Id", r."Id", u."Id", v.qty FROM (VALUES
  ('О-198', 'Сталь 08пс',    'шт', 200),
  ('О-199', 'Круг 12мм',     'кг', 150),
  ('О-201', 'Профиль 40×40', 'шт', 120),
  ('О-202', 'Труба 57×3',    'шт', 64),
  ('О-203', 'Сталь 08пс',    'шт', 250),
  ('О-204', 'Круг 12мм',     'кг', 300),
  ('О-205', 'Лист 2мм',      'шт', 50)
) AS v(num, res, unit, qty)
JOIN "Shipments" s ON s."Number" = v.num
JOIN "Resources" r ON r."Name" = v.res
JOIN "Units" u ON u."Name" = v.unit;

-- Остатки = приходы − подтверждённые отгрузки (за всё время)
INSERT INTO "Balances" ("ResourceId", "UnitId", "Quantity")
SELECT r."Id", u."Id", v.qty FROM (VALUES
  ('Сталь 08пс',    'шт', 1150),
  ('Профиль 40×40', 'шт', 330),
  ('Лист 2мм',      'шт', 420),
  ('Труба 57×3',    'шт', 436),
  ('Круг 12мм',     'кг', 1050)
) AS v(res, unit, qty)
JOIN "Resources" r ON r."Name" = v.res
JOIN "Units" u ON u."Name" = v.unit;

COMMIT;
