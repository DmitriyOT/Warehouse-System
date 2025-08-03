import EntityGridComponent from "../../components/pure/EntityGridComponent";
import {useEffect, useState} from "react";
import {createGridApi} from "../../api/Api";
import type {PageView} from "../../types/PageView";
import type {GridData} from "../../types/Response";
import type {ResourceEntity} from "../../types/ResourceEntity";
import {useNavigate} from "react-router-dom";
import {DEFAULT_PAGE_VIEW, RESOURCE_PAGE_ROUTE} from "../../utils/consts";
import type {FilterDto} from "../../types/Request";

const ResourcePage = () => {

    const [data, setData] = useState<GridData<ResourceEntity> | undefined>(undefined);
    const [pageView, setPageView] = useState<PageView>(DEFAULT_PAGE_VIEW);
    const [archive, setArchive] = useState<boolean>(false);
    const [filter, setFilter] = useState<Array<FilterDto>>([{type: 'equal', propertyName: 'IsArchive', argument: 'false'}]);

    const {load} = createGridApi('/Resource');
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
            <EntityGridComponent title='Ресурсы' buttons={
                !archive ? [{id: "create", onClick: () => {navigate(RESOURCE_PAGE_ROUTE + '/0');} },
                {id: "toArchive", onClick: () => {invertArchive()} }]
                    :
                    [{id: "fromArchive", onClick: () => {invertArchive()} }]
            }
                                 columns={[{field:'name', headerName:'Наименование', width: 300}]} rows={data?.items ?? []}
                                 pageView={ pageView }
                                 onPageChange={(page) => LoadData({page:page})}
                                 onPageSizeChange={(size) => LoadData({size:size})}
                                 onItemOpen={(id) => navigate(RESOURCE_PAGE_ROUTE + '/' + id )}
            />
        </>
    )
}

export default ResourcePage