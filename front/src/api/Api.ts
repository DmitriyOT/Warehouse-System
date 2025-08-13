import axios from 'axios'
import type {ResponseDto} from "../types/Response";
import type {ResponseGridDto} from "../types/Response";
import type {GridOptions} from "../types/Request";
import type {ModalContextType} from "../types/Modal";

const baseUrlApi = import.meta.env.VITE_APP_API_URL

const $host = axios.create({
    baseURL: baseUrlApi
})

const createItemApi = function<T> (itemPath: string) {
    return {
        load: async (itemId: number) => {
            if(itemId !== 0) {
                const data = await $host.get<ResponseDto<T>>(itemPath + '/getItem?id=' + itemId);
                return data.data.response;
            }
            else
            {
                return undefined;
            }
        },
        save: async (item: T) => {
            const data = await $host.post<ResponseDto<number>>(itemPath + '/EditItem', item);
            return data.data.response;
        },
        deleteItems: async (itemId: number ) => {
            const data = await $host.post<ResponseDto<T>>(itemPath + '/DeleteItems?id='+ itemId);
            return data.data.response;
        },
        archive: async (itemId: number, newState: boolean) =>{
            await $host.put<ResponseDto<void>>(itemPath + '/EditArchiveItem?id='+itemId+'&newState='+newState);
            return undefined;
        }
    }
}

const createGridApi = function<T> (itemPath: string, modalC: ModalContextType) {
    return {
        load: async (gridOptions: GridOptions) => {
            try {
                let data = await $host.post<ResponseGridDto<T>>(itemPath + '/getAll', gridOptions);
                return data.data.response;
            }
            catch (e) {
                modalC?.setModal({header:'Ошибка', content: e.message, buttonText: 'Ок', onClose: () => {modalC?.setModal(null)} })
            }
        }
    }
}

export { $host, createItemApi, createGridApi }