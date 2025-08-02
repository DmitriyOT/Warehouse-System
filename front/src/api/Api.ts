import axios from 'axios'
import type {ResponseDto} from "../types/Response";

const baseUrlApi = import.meta.env.VITE_APP_API_URL

const $host = axios.create({
    baseURL: baseUrlApi
})

const createItemApi = function<T> (itemPath) {
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
            const data = await $host.post<ResponseDto<T>>(itemPath + '/EditItem', item);
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

export { $host, createItemApi }