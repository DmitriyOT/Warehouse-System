export interface FilterOptions {
    name: string,
    type: FilterTypes,
    fieldName: string,
}

export type FilterTypes = 'select' | 'date'