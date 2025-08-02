import EntityGridComponent from "../components/pure/EntityGridComponent";
import {useEffect, useState} from "react";
import {$host} from "../api/Api";
import {PageView} from "../types/PageView";

const ResourcePage = () => {

    const [data, setData] = useState({});
    const [pageView, setPageView] = useState<PageView>({page: 1, totalPages: 1, size: 10})

    const LoadData = async (pageN) => {
        $host.post('/Resource/getAll',
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
                                 onItemOpen={(id) => console.log('item id =', id)}
            />
        </>
    )
}

export default ResourcePage