import {createGridApi} from "./Api";
import type {GridOptions} from "../types/Request";
import type {ModalContextType} from "../types/Modal";

// Класс для доступа к данным в select выборах
export class DataProvider<T> {

    private readonly loadData: (options: GridOptions) => Promise<any>;

    constructor(entityPath: string, mContext: ModalContextType) {
        this.loadData = createGridApi<T>(entityPath, mContext).load;
    }

    public getData()
    {
        return this.loadData({page: 1, pageSize: 1000, filters: [{propertyName:'IsArchive', type:'equal', argument:'false'}]})
    }
}