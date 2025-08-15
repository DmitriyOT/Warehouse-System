export type FilterType = 'equal' | 'dateRange'

export interface FilterDto{
    propertyName: string,
    type: FilterType,
    argument: number | string
}

export interface GridOptions{
    page: number,
    pageSize: number,
    filters: Array<FilterDto>
}