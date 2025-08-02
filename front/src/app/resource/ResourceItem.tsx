import type {ResourceEntity} from "../../types/ResourceEntity";
import {Input} from "@mui/material";
import {Form} from "react-bootstrap";
import {useEffect, useState} from "react";

type ResourceItemProps = {
    id: number,
    data: ResourceEntity | undefined
}

const ResourceItem = ({id, data}: ResourceItemProps) => {

    const [name, setName] = useState<string>('')

    useEffect(() => {
        setName(data?.name ?? '')
    }, [data])

   return (
       <>
        Resource item id {id}
           <Form.Control value={name} onChange={(e) => setName(e.target.value) } />
       </>
   )
}

export default ResourceItem