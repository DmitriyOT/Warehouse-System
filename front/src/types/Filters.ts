export interface FIlterOptions{
    name: string,
    type: FilterTypes,
    fieldName: string,
}

export type FilterTypes = 'select' | 'date'