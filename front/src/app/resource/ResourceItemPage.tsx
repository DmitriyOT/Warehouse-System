import EntityCardComponent from "../../components/pure/EntityCardComponent";
import {useParams} from "react-router-dom";
import {$host} from "../../api/Api";
import type {ResponseDto} from "../../types/Response";
import type {ResourceEntity} from "../../types/ResourceEntity";
import {useEffect, useState} from "react";
import ResourceItem from "./ResourceItem";

const ResourceItemPage = () => {

    const {id} = useParams()
    const [data, setData] = useState<ResourceEntity | undefined>(undefined)

    const LoadData = async () => {
        $host.get<ResponseDto<ResourceEntity>>('/Resource/getItem?id=' + id)
            .then(data =>
            { setData(data.data.response); })
    }

    useEffect(() => {
        LoadData()
    }, [])

  return (
      <>
        <EntityCardComponent title='Ресурс' buttons={[]} Component={<ResourceItem id={+id} data={data} />} />
      </>
  )
}

export default ResourceItemPage