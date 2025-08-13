import type {ItemComponentProps, IncomeEntity, ResourceEntity, UnitEntity} from "../../types/Entities";
import PureTextInput from "../../components/pure/controls/PureTextInput";
import PureDateInput from "../../components/pure/controls/PureDateInput";
import FieldComponent from "../../components/pure/controls/FieldComponent";
import {Button} from "react-bootstrap";
import PureSelectInput from "../../components/pure/controls/PureSelectInput";
import {useEffect, useState} from "react";
import {DataProvider} from "../../api/DataProvider";
import {RESOURCE_API_PATH, UNIT_API_PATH} from "../../utils/consts";
import type {SelectOption} from "../../types/Filters";
import {GridData, ResponseGridDto} from "../../types/Response";


const IncomeItem = ({data, onChange}: ItemComponentProps<IncomeEntity>) => {

    const DpResource = new DataProvider<ResourceEntity>(RESOURCE_API_PATH);
    const DpUnit = new DataProvider<ResourceEntity>(UNIT_API_PATH);
    const [nextId, setNextId] = useState<number>(-1)

    const [optionsResource, setOptionsResource] = useState<Array<SelectOption>>([])
    const [optionsUnit, setOptionsUnit] = useState<Array<SelectOption>>([])

    useEffect(() => {
        DpResource.getData().then(data => {
            let dataT = data as GridData<ResourceEntity>;
            setOptionsResource( dataT.items.map(e => ({value: e.id.toString(), title: e.name}) ) );
        })
        DpUnit.getData().then(data => {
            let dataT = data as GridData<UnitEntity>;
            setOptionsUnit( dataT.items.map(e => ({value: e.id.toString(), title: e.name}) ) )
        })

    }, [])

   return (
       <>
           <PureTextInput value={data?.number ?? ''} onChange={ (e) => onChange({...data!, number: e}) }
                          name={'Номер'} placeholder={'Введите номер'} />
           <FieldComponent name='Дата' >
               <PureDateInput value={data?.date} onChange={ (e) => { onChange({...data!, date: e}) } } />
           </FieldComponent>

       </>
   )
}

export default IncomeItem