import {useContext, useEffect, useState} from "react";
import type {GridData} from "../types/Response";
import type {PageView} from "../types/PageView";
import {DEFAULT_PAGE_VIEW} from "../utils/consts";
import type {FilterDto} from "../types/Request";
import {createGridApi} from "../api/Api";
import EntityGridComponent from "./pure/EntityGridComponent";
import {useNavigate} from "react-router-dom";
import type {FilterOptions, ReturnFilter} from "../types/Filters";
import type {ModalContextType} from "../types/Modal";
import {ModalContext} from "../App";

type GridPageVariant = 'Archive' | 'Filters';

const createGridPage = function<T> (apiPath: string, navPath: string, title: string, variant: GridPageVariant,
                                    columns: Array<{field: string, headerName: string, width: number}>,
                                    filters: Array<FilterOptions> = [], itemOpenCreate: boolean = true) {
    const GridPage = () => {

        const [data, setData] = useState<GridData<T> | undefined>(undefined);
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
                { setData(data); setPageView(data?.page ?? DEFAULT_PAGE_VIEW) })
        }

        useEffect(() => {
            LoadData({});
        }, [])

        const invertArchive = () => {
            let fil = [...filter];
            fil[0].argument = (!archive).toString();
            setArchive(!archive);
            setFilter(fil);
            LoadData({})
        }


        for (const item of filters)
        {
            item.onChange = (value: ReturnFilter) => {
                let fil = [...filter];
                let f = fil.find(f => f.propertyName === value.fieldName);
                let argument = value.options.map(e => e.value).join(',');
                if (f === undefined) {
                    fil.push({propertyName: value.fieldName, type: 'equal', argument: argument})
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
                <EntityGridComponent<T> title={title} buttons={
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
                                     columns={columns} rows={data?.items ?? []}
                                     pageView={ pageView }
                                     onPageChange={(page) => LoadData({page:page})}
                                     onPageSizeChange={(size) => LoadData({size:size})}
                                     onItemOpen={(id) => {if(itemOpenCreate) navigate(navPath + '/' + id )} }
                                     filters={filters}
                />
            </>
        )
    }

    return GridPage
}

export default createGridPage;