import {useContext, useEffect, useState} from "react";
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

type GridPageVariant = 'Archive' | 'Filters';

type GridRow = Record<string, unknown>;

const createGridPage = function<T> (apiPath: string, navPath: string, title: string, variant: GridPageVariant,
                                    columns: Array<GridColumnType>,
                                    filters: Array<FilterOptions> = [],
                                    itemOpenCreate: boolean = true,
                                    rowsProcess?: (items: T[]) => GridRow[]) {
    const GridPage = () => {

        const [data, setData] = useState<Array<GridRow> | undefined>(undefined);
        const [pageView, setPageView] = useState<PageView>(DEFAULT_PAGE_VIEW);
        const [archive, setArchive] = useState<boolean>(false);
        const [filter, setFilter] = useState<Array<FilterDto>>(
            variant === 'Archive' ? [{type: 'equal', propertyName: 'IsArchive', argument: 'false'}] : []
        );

        const mContext = useContext<ModalContextType>(ModalContext);

        const {load} = createGridApi<T>(apiPath, mContext);
        const navigate = useNavigate();

        const LoadData = async (pageN: {page?:number, size?: number}) => {
            load({"page":pageN.page ?? pageView.page, "pageSize":pageN.size ?? pageView.size, filters: filter})
                .then(data =>
                {
                    if (data === undefined) {
                        return;
                    }
                    if(rowsProcess !== undefined)
                    {
                        setData(rowsProcess(data.items))
                    }
                    else {
                        setData(data.items as GridRow[]);
                    }
                    setPageView(data.page ?? DEFAULT_PAGE_VIEW)
                })
        }

        useEffect(() => {
            LoadData({});
            // eslint-disable-next-line react-hooks/exhaustive-deps
        }, [])

        const invertArchive = () => {
            const fil = [...filter];
            fil[0].argument = (!archive).toString();
            setArchive(!archive);
            setFilter(fil);
            LoadData({})
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
                        {id: "applyFilter", onClick: () => {LoadData({});} }]
                            :
                        [{id: "applyFilter", onClick: () => {LoadData({});} }]
                    )
                }
                                     columns={columns} rows={data ?? []}
                                     pageView={ pageView }
                                     onPageChange={(page: number) => LoadData({page:page})}
                                     onPageSizeChange={(size: number) => LoadData({size:size})}
                                     onItemOpen={(id: number) => {if(itemOpenCreate) navigate(navPath + '/' + id )} }
                                     filters={filters}
                />
            </>
        )
    }

    return GridPage
}

export default createGridPage;
