export type SelectOption = {
    value: string,
    title: string
}

interface BaseFilterOptions {
    name: string,
    fieldName: string,
    onChange?: (value: any) => void,
}

interface SelectFilterOptions extends BaseFilterOptions{
    type: 'select',
    options: Array<SelectOption>,
    selectedOptions: Array<SelectOption>,
}

interface DateFilterOptions extends BaseFilterOptions{
    type: 'date',
    startDate?: Date,
    endDate?: Date,
    selectDate?: Date
}

export type FilterOptions = SelectFilterOptions | DateFilterOptions

export type FilterTypes = 'select' | 'date'