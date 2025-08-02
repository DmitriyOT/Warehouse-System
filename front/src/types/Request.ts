export interface FilterDto{
    propertyName: string,
    type: 'equal' | 'above' | 'low',
    argument: number | string
}