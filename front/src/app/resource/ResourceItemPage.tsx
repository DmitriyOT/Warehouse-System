import EntityCardComponent from "../../components/pure/EntityCardComponent";
import {useLocation, useNavigate, useParams} from "react-router-dom";
import {createItemApi} from "../../api/Api";
import type {ResourceEntity} from "../../types/ResourceEntity";
import {useEffect, useState} from "react";
import ResourceItem from "./ResourceItem";
import {RESOURCE_PAGE_ROUTE} from "../../utils/consts";

const ResourceItemPage = () => {

    const {id} = useParams()
    const [data, setData] = useState<ResourceEntity | undefined>(undefined)

    const {load, deleteItems, save, archive} = createItemApi<ResourceEntity>('/Resource');

    const navigate = useNavigate();
    const location = useLocation();

    const LoadData = async () => {
        const response = await load(+id);
        setData(response);
    }

    useEffect(() => {
        LoadData()
    }, [location])

  return (
      <>
        <EntityCardComponent title='Ресурс'  Component={<ResourceItem id={+id} data={data} onChange={setData} />} isArchive={data?.isArchive}
                             buttons={[
                                 {code:'save', onClick: () => { save(data!).then(res => { if(res !== +id) navigate(RESOURCE_PAGE_ROUTE + '/' + res) } )} },
                                 {code:'delete', onClick: () => { deleteItems(data!.id).then(() => navigate(RESOURCE_PAGE_ROUTE))} },
                                 {code:'archiveToggle', onClick: () => {archive(data!.id, !data!.isArchive).then(() => {setData({...data!, isArchive:!data!.isArchive})})} }
                             ]}
        />
      </>
  )
}

export default ResourceItemPage