import type {components} from "./api-generated";

// Тип фильтра — фронтовое уточнение строкового Type из FilterDto.cs на бэке
export type FilterType = 'equal' | 'dateRange'

// На бэке поля FilterDto — обязательные строки (в swagger — string | null)
export type FilterDto = Required<components['schemas']['FilterDto']> & {
    type: FilterType
}

// Бэк (GridOptionsDto.cs): page/pageSize обязательны, search/filters опциональны
export type GridOptions = Required<Pick<components['schemas']['GridOptionsDto'], 'page' | 'pageSize'>>
    & Pick<components['schemas']['GridOptionsDto'], 'search'> & {
    filters: Array<FilterDto>
}
