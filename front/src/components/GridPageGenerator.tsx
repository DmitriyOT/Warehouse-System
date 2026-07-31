import {useContext, useState} from "react";
import type {PageView} from "../types/PageView";
import {DEFAULT_PAGE_VIEW} from "../utils/consts";
import type {FilterDto} from "../types/Request";
import {createGridApi} from "../api/Api";
import EntityGridComponent from "./pure/EntityGridComponent";
import {useNavigate} from "react-router-dom";
import type {FilterOptions, ReturnFilter} from "../types/Filters";
import type {ModalContextType} from "../types/Modal";
import {ModalContext} from "../context/ModalContext";
import type {GridColumnType} from "../types/Grid";
import {useQuery} from "@tanstack/react-query";
import {gridKey} from "../api/queries";

type GridPageVariant = 'Archive' | 'Filters';

type GridRow = Record<string, unknown>;

const createGridPage = function<T> (apiPath: string, navPath: string, title: string, variant: GridPageVariant,
                                    columns: Array<GridColumnType>,
                                    filters: Array<FilterOptions> = [],
                                    itemOpenCreate: boolean = true,
                                    rowsProcess?: (items: T[]) => GridRow[]) {
    const GridPage = () => {

        const initialFilter: Array<FilterDto> =
            variant === 'Archive' ? [{type: 'equal', propertyName: 'IsArchive', argument: 'false'}] : [];

        // filter — редактируемые значения в контролах, appliedFilter — применённые (идут в запрос)
        const [pageParams, setPageParams] = useState<{page: number, size: number}>(
            {page: DEFAULT_PAGE_VIEW.page, size: DEFAULT_PAGE_VIEW.size});
        const [archive, setArchive] = useState<boolean>(false);
        const [filter, setFilter] = useState<Array<FilterDto>>(initialFilter);
        const [appliedFilter, setAppliedFilter] = useState<Array<FilterDto>>(initialFilter);

        const mContext = useContext<ModalContextType>(ModalContext);

        const {load} = createGridApi<T>(apiPath, mContext);
        const navigate = useNavigate();

        const {data: gridData} = useQuery({
            queryKey: [...gridKey(apiPath), pageParams, appliedFilter],
            // при ошибке load отдаёт undefined — react-query не принимает undefined, отдаём null
            queryFn: async () => (await load({page: pageParams.page, pageSize: pageParams.size, filters: appliedFilter})) ?? null
        })

        const data: Array<GridRow> | undefined = !gridData ? undefined
            : (rowsProcess !== undefined ? rowsProcess(gridData.items) : gridData.items as GridRow[]);
        const pageView: PageView = gridData?.page ?? DEFAULT_PAGE_VIEW;

        const invertArchive = () => {
            const newArchive = !archive;
            const fil = appliedFilter.map((f, index) => index === 0 ? {...f, argument: newArchive.toString()} : f);
            setArchive(newArchive);
            setFilter(fil);
            setAppliedFilter(fil);
            setPageParams(p => ({...p, page: DEFAULT_PAGE_VIEW.page}));
        }


        for (const item of filters)
        {
            item.onChange = (value: ReturnFilter) => {
                let fil = [...filter];
                const f = fil.find(f => f.propertyName === value.fieldName);
                const argument = value.argument;
                if (f === undefined) {
                    fil.push({propertyName: value.fieldName, type: value.type, argument: argument})
                } else {
                    if (argument !== '')
                        f.argument = argument;
                    else
                        fil = fil.filter(x => x.propertyName !== value.fieldName);
                }
                setFilter(fil)
            }
        }


        return (
            <>
                <EntityGridComponent title={title} buttons={
                    variant === 'Archive' ?
                    (
                        !archive ? [{id: "create", onClick: () => {navigate(navPath + '/0');} },
                        {id: "toArchive", onClick: () => {invertArchive()} }]
                            :
                        [{id: "fromArchive", onClick: () => {invertArchive()} }]
                    )
                        :
                    (
                        itemOpenCreate ?
                        [{id: "create", onClick: () => {navigate(navPath + '/0');} },
                        {id: "applyFilter", onClick: () => {setAppliedFilter([...filter]); setPageParams(p => ({...p, page: DEFAULT_PAGE_VIEW.page}));} }]
                            :
                        [{id: "applyFilter", onClick: () => {setAppliedFilter([...filter]); setPageParams(p => ({...p, page: DEFAULT_PAGE_VIEW.page}));} }]
                    )
                }
                                     columns={columns} rows={data ?? []}
                                     pageView={ pageView }
                                     onPageChange={(page: number) => setPageParams(p => ({...p, page: page}))}
                                     onPageSizeChange={(size: number) => setPageParams(p => ({...p, size: size}))}
                                     onItemOpen={(id: number) => {if(itemOpenCreate) navigate(navPath + '/' + id )} }
                                     filters={filters}
                />
            </>
        )
    }

    return GridPage
}

export default createGridPage;
