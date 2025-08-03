export interface FilterDto{
    propertyName: string,
    type: 'equal' | 'above' | 'low',
    argument: number | string
}

export interface GridOptions{
    page: number,
    pageSize: number,
    filters: Array<FilterDto>
}