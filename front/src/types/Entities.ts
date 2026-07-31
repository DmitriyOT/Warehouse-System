import type {components} from "./api-generated";

// Схемы из OpenAPI (генерация: npm run generate:api).
// В сгенерированных схемах поля id и пр. опциональны — на фронте после загрузки
// они всегда заполнены, поэтому уточняем обязательность через Required.
type Schemas = components['schemas'];

// Базовые интерфейсы фронта (бэк: BaseEntityWithId / BaseEntityWithIdArchiveName)
export interface BaseEntityId {
    id: number
}

export interface BaseEntityIdArchive extends BaseEntityId {
    isArchive: boolean
}

export type ResourceEntity = Required<Schemas['ResourceEntity']>

export type UnitEntity = Required<Schemas['UnitEntity']>

export type ClientEntity = Required<Schemas['ClientEntity']>

export type BalanceEntity = Required<Omit<Schemas['BalanceEntity'], 'resource' | 'unit'>> & {
    resource?: ResourceEntity,
    unit?: UnitEntity
}

// getItem возвращает доменную сущность бэка, по полям совпадающую с EditDto;
// дату (DateOnly на бэке) фронт конвертирует в Date (LoadStringToDate/UploadDateToString)
export interface IncomeEntity extends BaseEntityId {
    number: Schemas['IncomeEditDto']['number'],
    date: Date,
    incomeItems: Array<IncomeItemEntity>
}

export type IncomeItemEntity = Required<Schemas['IncomeItemEditDto']> & {
    resource?: ResourceEntity,
    unit?: UnitEntity
}

// clientName и isApprove есть в доменной сущности ShipmentEntity бэка,
// но не в ShipmentEditDto (в api-generated.ts поле isApprove устарело — его уже нет в DTO бэка)
export interface ShipmentEntity extends BaseEntityId {
    number: Schemas['ShipmentEditDto']['number'],
    date: Date,
    clientId: Schemas['ShipmentEditDto']['clientId'],
    clientName?: string,
    isApprove: boolean,
    shipmentItems: Array<ShipmentItemEntity>
}

export type ShipmentItemEntity = Required<Schemas['ShipmentItemEditDto']> & {
    resource?: ResourceEntity,
    unit?: UnitEntity
}

export interface ItemComponentProps<T> {
    id: number,
    data: T | undefined,
    onChange: (item: T) => void
}
