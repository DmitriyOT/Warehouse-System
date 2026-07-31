import {useContext} from "react";
import {useQuery} from "@tanstack/react-query";
import {ModalContext} from "../context/ModalContext";
import {DataProvider} from "./DataProvider";
import type {BaseEntityId} from "../types/Entities";
import type {GridData} from "../types/Response";
import type {SelectOption} from "../types/Filters";

// Ключи react-query для инвалидации после мутаций
export const gridKey = (apiPath: string) => ['grid', apiPath] as const;
export const itemKey = (apiPath: string) => ['item', apiPath] as const;
export const selectKey = (apiPath: string) => ['select', apiPath] as const;

// Загрузка справочника для селектов с кэшированием (staleTime настраивается в QueryClient)
// apiPath === undefined/'' — запрос не выполняется (для фильтров не-select типа)
export const useSelectOptions = <T extends BaseEntityId>(apiPath: string | undefined, ignoreArchive = false) => {
    const mContext = useContext(ModalContext);
    return useQuery<Array<SelectOption>>({
        queryKey: [...selectKey(apiPath ?? ''), ignoreArchive],
        enabled: apiPath !== undefined && apiPath !== '',
        queryFn: async () => {
            const dp = new DataProvider<T>(apiPath!, mContext, ignoreArchive);
            const data = await dp.getData() as GridData<T> | undefined;
            return (data?.items ?? []).map(e => ({
                value: e.id.toString(),
                title: (e as { name?: string, number?: string }).name
                    ?? (e as { name?: string, number?: string }).number ?? ''
            }));
        }
    });
}
