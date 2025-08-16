import axios from 'axios'
import type {ResponseDto} from "../types/Response";
import type {ResponseGridDto} from "../types/Response";
import type {GridOptions} from "../types/Request";
import type {ModalContextType} from "../types/Modal";

const baseUrlApi = import.meta.env.VITE_APP_API_URL

const $host = axios.create({
    baseURL: baseUrlApi
})

const showError = (text: string, modalC: ModalContextType) => {
    modalC?.setModal({header:'Ошибка', content: text, buttonText: 'Ок', onClose:
            () => {modalC?.setModal(null)} })
}

const errorHandle = async (script: any, modalC: ModalContextType) => {
    try {
        const result = await script();
        if(result?.hasError)
        {
            showError( result.errorMessage, modalC )
        }
        else
        {
            return result?.response;
        }
    }
    catch (e) {
        showError( (e as unknown as Error).message, modalC );
    }
}

const createItemApi = function<T> (itemPath: string, modalC: ModalContextType) {

    return {
        load: async (itemId: number) => {
            return await errorHandle( async () => {
                if (itemId !== 0) {
                    const data = await $host.get<ResponseDto<T>>(itemPath + '/getItem?id=' + itemId);
                    return data.data;
                } else {
                    return undefined;
                }
            }, modalC)
        },
        save: async (item: T) => {
            return await errorHandle( async () => {
                const {data} = await $host.post<ResponseDto<number>>(itemPath + '/EditItem', item);
                return data;
            }, modalC )
        },
        deleteItems: async (itemId: number ) => {
            return await errorHandle( async () => {
                const {data} = await $host.post<ResponseDto<T>>(itemPath + '/DeleteItems?id=' + itemId);
                return data;
            }, modalC)
        },
        archive: async (itemId: number, newState: boolean) => {
            return await errorHandle( async () => {
                const {data} = await $host.put<ResponseDto<void>>(itemPath + '/EditArchiveItem?id='+itemId+'&newState='+newState);
                return data;
            }, modalC)
        },
        changeState: async (itemId: number, newStateCode: string) => {
            return await errorHandle(async () => {
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
                let {data} = await $host.post<ResponseGridDto<T>>(itemPath + '/getAll', gridOptions);
                return data;
            }, modalC)
        }
    }
}

export { $host, createItemApi, createGridApi }