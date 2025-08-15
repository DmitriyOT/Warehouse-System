import type {FilterType} from "./Request";

export type SelectOption = {
    value: string,
    title: string
}

interface BaseFilterOptions {
    name: string,
    fieldName: string,
    onChange?: (value: ReturnFilter) => any,
}

export interface SelectFilterOptions extends BaseFilterOptions{
    type: 'select',
    options?: Array<SelectOption>,
    apiPath: string,
}

export interface DateFilterOptions extends BaseFilterOptions{
    type: 'date',
    startDate?: Date,
    endDate?: Date,
    selectDate?: Date
}

export type FilterOptions = SelectFilterOptions | DateFilterOptions

export type ReturnFilter = {
    argument: string,
    fieldName: string,
    type: FilterType
}