import type {PageView} from "./PageView";

export interface ResponseDto<T> {
    hasError: boolean,
    errorMessage: string,
    response: T
}

export interface GridData<T> {
    items: Array<T>, 
    page: PageView
}

export interface ResponseGridDto<T> extends ResponseDto<GridData<T>>
{}