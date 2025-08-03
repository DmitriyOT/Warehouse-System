import {useEffect, useState} from "react";
import type {GridData} from "../types/Response";
import type {PageView} from "../types/PageView";
import {DEFAULT_PAGE_VIEW} from "../utils/consts";
import type {FilterDto} from "../types/Request";
import {createGridApi} from "../api/Api";
import EntityGridComponent from "./pure/EntityGridComponent";
import {useNavigate} from "react-router-dom";

const createGridPage = function<T> (apiPath: string, navPath: string, title: string,
                                    columns: Array<{field: string, headerName: string, width: number}>) {
    const GridPage = () => {

        const [data, setData] = useState<GridData<T> | undefined>(undefined);
        const [pageView, setPageView] = useState<PageView>(DEFAULT_PAGE_VIEW);
        const [archive, setArchive] = useState<boolean>(false);
        const [filter, setFilter] = useState<Array<FilterDto>>([{type: 'equal', propertyName: 'IsArchive', argument: 'false'}]);

        const {load} = createGridApi<T>(apiPath);
        const navigate = useNavigate();

        const LoadData = async (pageN: {page?:number, size?: number}) => {
            load({"page":pageN.page ?? pageView.page, "pageSize":pageN.size ?? pageView.size, filters: filter})
                .then(data =>
                { setData(data); setPageView(data.page) })
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

        return (
            <>
                <EntityGridComponent<T> title={title} buttons={
                    !archive ? [{id: "create", onClick: () => {navigate(navPath + '/0');} },
                            {id: "toArchive", onClick: () => {invertArchive()} }]
                        :
                        [{id: "fromArchive", onClick: () => {invertArchive()} }]
                }
                                     columns={columns} rows={data?.items ?? []}
                                     pageView={ pageView }
                                     onPageChange={(page) => LoadData({page:page})}
                                     onPageSizeChange={(size) => LoadData({size:size})}
                                     onItemOpen={(id) => navigate(navPath + '/' + id )}
                />
            </>
        )
    }

    return GridPage
}

export default createGridPage;