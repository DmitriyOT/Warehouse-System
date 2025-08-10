import {createGridApi} from "./Api";
import type {GridOptions} from "../types/Request";

export class DataProvider<T> {
    private entityPath: string;
    private readonly loadData: (options: GridOptions) => Promise<any>;
    constructor(entityPath: string) {
        this.entityPath = entityPath
        this.loadData = createGridApi<T>(entityPath).load;
    }
    public getData()
    {
        return this.loadData({page: 1, pageSize: 1000, filters: []})
    }
}