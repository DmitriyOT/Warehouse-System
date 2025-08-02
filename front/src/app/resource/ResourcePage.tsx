import EntityGridComponent from "../../components/pure/EntityGridComponent";
import {useEffect, useState} from "react";
import {$host} from "../../api/Api";
import type {PageView} from "../../types/PageView";
import type {GridData, ResponseGridDto} from "../../types/Response";
import type {ResourceEntity} from "../../types/ResourceEntity";
import {useNavigate} from "react-router-dom";
import {RESOURCE_PAGE_ROUTE} from "../../utils/consts";
import type {FilterDto} from "../../types/Request";

const ResourcePage = () => {

    const [data, setData] = useState<GridData<ResourceEntity> | undefined>(undefined);
    const [pageView, setPageView] = useState<PageView>({page: 1, totalPages: 1, size: 10});
    const [archive, setArchive] = useState<boolean>(false);
    const [filter, setFilter] = useState<Array<FilterDto>>([{type: 'equal', propertyName: 'IsArchive', argument: 'false'}]);

    const navigate = useNavigate();

    const LoadData = async (pageN: {page?:number, size?: number}) => {
        $host.post<ResponseGridDto<ResourceEntity>>('/Resource/getAll',
            {"page":pageN.page ?? pageView.page, "pageSize":pageN.size ?? pageView.size, filters: filter})
            .then(data =>
                { setData(data.data.response); setPageView(data.data.response.page) })
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
                !archive ? [{id: "add", className:"me-2", variant:"outline-success", text:"Добавить", onClick: () => {navigate(RESOURCE_PAGE_ROUTE + '/0');} },
                {id: "toArchive", className:"", variant:"outline-secondary", text: "В архив", onClick: () => {invertArchive()} }]
                    :
                    [{id: "fromArchive", className:"", variant:"outline-secondary", text: 'Из архива', onClick: () => {invertArchive()} }]
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