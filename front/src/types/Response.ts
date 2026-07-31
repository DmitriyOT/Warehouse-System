import type {PageView} from "./PageView";

// Обёртки ответов бэка (ResponseDto.cs / ResponseDtoGrid.cs). Это generic-классы,
// в api-generated.ts их схем нет — типы ручные по контрактам Warehouse.Contracts.
export interface ResponseDto<T> {
    hasError: boolean,
    // На бэке ErrorMessage — string? (null при успехе)
    errorMessage: string | null,
    response: T
}

// Бэк: GridResponsePair<T> (ResponseDtoGrid.cs)
export interface GridData<T> {
    items: Array<T>,
    page: PageView
}

export type ResponseGridDto<T> = ResponseDto<GridData<T>>;
