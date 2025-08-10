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
           <table className='table-bordered2 border-dark border mt-2'>
               <thead className='bg-secondary bg-opacity-25' >
               <tr>
                <th className='p-1 '>
                    <Button className='' variant='outline-success' onClick={
                        () => {let items = data?.incomeItems;
                            let newItem = {quantity: 0, id: nextId, resource: undefined, unit: undefined };
                            setNextId(nextId-1);
                            if(items) items.push(newItem);
                            else items = [newItem];
                            onChange({...data, incomeItems: items});
                        }}>+</Button></th>
                   <th>Ресурс</th>
                   <th>Единица измерения</th>
                   <th>Количество</th>
               </tr>
               </thead>
               <tbody>
               {
                   data?.incomeItems?.map(item =>
                   <tr key={item.id} className='border-dark border'>
                       <td className='p-1'><Button variant='outline-danger' onClick={
                           () => {onChange({...data, incomeItems: data?.incomeItems.filter((x) => x !== item)})} }>X</Button></td>
                       <td><PureSelectInput selected={ item.resource?.id ? {value: item.resource?.id.toString(), title: item.resource?.name ?? ''} : undefined}
                                            options={optionsResource} onChange={() => {} } /> </td>
                       <td><PureSelectInput options={optionsUnit} onChange={() => {}}
                                            selected={item.unit?.id ? {value: item.unit?.id.toString(), title: item.unit?.name ?? ''} : undefined} /></td>
                       <td><PureTextInput value={item.quantity.toString()} onChange={() => {}} name={''} /></td>
                   </tr>
                   )
               }
               </tbody>
           </table>
       </>
   )
}

export default IncomeItem