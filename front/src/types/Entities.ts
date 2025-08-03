export interface BaseEntityIdArchive {
    id: number,
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

export interface ItemComponentProps<T> {
    id: number,
    data: T | undefined,
    onChange: (item: T) => void
}