import EntityGridComponent from "../../components/pure/EntityGridComponent";
import {useEffect, useState} from "react";
import {$host} from "../../api/Api";
import type {PageView} from "../../types/PageView";
import type {GridData, ResponseGridDto} from "../../types/Response";
import type {ResourceEntity} from "../../types/ResourceEntity";
import {useNavigate} from "react-router-dom";
import {RESOURCE_PAGE_ROUTE} from "../../utils/consts";

const ResourcePage = () => {

    const [data, setData] = useState<GridData<ResourceEntity> | undefined>(undefined);
    const [pageView, setPageView] = useState<PageView>({page: 1, totalPages: 1, size: 10});

    const navigate = useNavigate();

    const LoadData = async (pageN: {page?:number, size?: number}) => {
        $host.post<ResponseGridDto<ResourceEntity>>('/Resource/getAll',
            {"page":pageN.page ?? pageView.page, "pageSize":pageN.size ?? pageView.size})
            .then(data =>
                { setData(data.data.response); setPageView(data.data.response.page) })
    }

    useEffect(() => {
        LoadData({});
    }, [])

    return (
        <>
            <EntityGridComponent title='Ресурсы' buttons={[
                {id: "add", className:"me-2", variant:"outline-success", text:"Добавить", onClick: () => {} },
                {id: "В архив", className:"", variant:"outline-secondary", text:"В архив", onClick: () => {} }
            ]}
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