import type {ResourceEntity} from "../../types/ResourceEntity";
import {Input} from "@mui/material";
import {Form} from "react-bootstrap";
import {useEffect, useState} from "react";
import PureTextInput from "../../components/pure/controls/PureTextInput";

type ResourceItemProps = {
    id: number,
    data: ResourceEntity | undefined,
    onChange: (ResourceEntity) => void
}

const ResourceItem = ({id, data, onChange}: ResourceItemProps) => {

   return (
       <>
           <PureTextInput value={data?.name ?? ''} onChange={ (e) => onChange({...data, name: e}) }
                          name={'Наименование'} placeholder={'Введите наименование'} />
       </>
   )
}

export default ResourceItem