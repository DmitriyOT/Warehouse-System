export interface BaseEntityId {
    id: number
}

export interface BaseEntityIdArchive extends BaseEntityId {
    isArchive: boolean
}

export interface ResourceEntity extends BaseEntityIdArchive{
    name: string
}

export interface UnitEntity extends BaseEntityIdArchive{
    name: string
}

export interface ClientEntity extends BaseEntityIdArchive{
    name: string,
    address: string
}

export interface IncomeEntity extends BaseEntityId {
    number: string,
    date: string,
    incomeItems: Array<string>
}

export interface ItemComponentProps<T> {
    id: number,
    data: T | undefined,
    onChange: (item: T) => void
}