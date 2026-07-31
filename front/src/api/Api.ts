import axios from 'axios'
import type {ResponseDto, ResponseGridDto} from "../types/Response";
import type {GridOptions} from "../types/Request";
import type {ModalContextType} from "../types/Modal";
import {LoadStringToDate, UploadDateToString} from "../utils/functions";
import {LOGIN_PAGE_ROUTE, TOKEN_KEY} from "../utils/consts";

const baseUrlApi = import.meta.env.VITE_APP_API_URL

const $host = axios.create({
    baseURL: baseUrlApi
})

// Подстановка JWT-токена во все запросы
$host.interceptors.request.use(config => {
    const token = localStorage.getItem(TOKEN_KEY)
    if (token) {
        config.headers.Authorization = `Bearer ${token}`
    }
    return config
})

// При 401 (просрочен/невалиден токен) — удаляем токен и отправляем на страницу входа
$host.interceptors.response.use(response => response, error => {
    if (axios.isAxiosError(error) && error.response?.status === 401
        && window.location.pathname !== LOGIN_PAGE_ROUTE) {
        localStorage.removeItem(TOKEN_KEY)
        window.location.href = LOGIN_PAGE_ROUTE
    }
    return Promise.reject(error)
})

const showError = (text: string, modalC: ModalContextType) => {
    modalC?.setModal({header:'Ошибка', content: text, buttonText: 'Ок', onClose:
            () => {modalC?.setModal(null)} })
}

const errorHandle = async <T>(script: () => Promise<ResponseDto<T>>, modalC: ModalContextType): Promise<T | undefined> => {
    try {
        const result = await script();
        if(result?.hasError)
        {
            showError( result.errorMessage ?? 'Ошибка', modalC )
        }
        else
        {
            return result?.response;
        }
    }
    catch (e) {
        if (axios.isAxiosError(e)) {
            const data = e.response?.data as ResponseDto<unknown> | undefined;
            const message = data?.errorMessage
                ?? (e.response !== undefined && e.response.status >= 500 ? 'Ошибка сервера' : 'Ошибка сети');
            showError(message, modalC);
        }
        else {
            showError(e instanceof Error ? e.message : 'Ошибка сети', modalC);
        }
    }
}

const createItemApi = function<T> (itemPath: string, modalC: ModalContextType, editPath: string = 'EditItem') {

    return {
        load: async (itemId: number) => {
            return await errorHandle<T>( async () => {
                if (itemId !== 0) {
                    const {data} = await $host.get<ResponseDto<T>>(itemPath + '/getItem?id=' + itemId);
                    if (!data.hasError && data.response) {
                        LoadStringToDate(data.response as Record<string, unknown>);
                    }
                    return data;
                } else {
                    return {hasError: false, errorMessage: '', response: undefined as unknown as T};
                }
            }, modalC)
        },
        save: async (item: T) => {
            return await errorHandle<number>( async () => {
                const fixItem = UploadDateToString({...item} as Record<string, unknown>);
                const {data} = await $host.post<ResponseDto<number>>(itemPath + '/' + editPath, fixItem);
                return data;
            }, modalC )
        },
        deleteItems: async (itemId: number ) => {
            return await errorHandle<T>( async () => {
                const {data} = await $host.post<ResponseDto<T>>(itemPath + '/DeleteItems?id=' + itemId);
                return data;
            }, modalC)
        },
        archive: async (itemId: number, newState: boolean) => {
            return await errorHandle<void>( async () => {
                const {data} = await $host.put<ResponseDto<void>>(itemPath + '/EditArchiveItem?id='+itemId+'&newState='+newState);
                return data;
            }, modalC)
        },
        changeState: async (itemId: number, newStateCode: string) => {
            return await errorHandle<void>(async () => {
                const {data} = await $host.put<ResponseDto<void>>(itemPath + '/ChangeState?id=' + itemId+'&newStateCode='+newStateCode);
                return data;
            }, modalC)
        }
    }
}

const createGridApi = function<T> (itemPath: string, modalC: ModalContextType) {
    return {
        load: async (gridOptions: GridOptions) => {
            return await errorHandle( async () => {
                const {data} = await $host.post<ResponseGridDto<T>>(itemPath + '/getAll', gridOptions);
                return data;
            }, modalC)
        }
    }
}

export { $host, errorHandle, createItemApi, createGridApi }
