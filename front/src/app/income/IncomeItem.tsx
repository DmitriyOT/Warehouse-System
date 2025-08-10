import type {ItemComponentProps,IncomeEntity } from "../../types/Entities";
import PureTextInput from "../../components/pure/controls/PureTextInput";
import PureDateInput from "../../components/pure/controls/PureDateInput";
import FieldComponent from "../../components/pure/controls/FieldComponent";
import {Button} from "react-bootstrap";
import PureSelectInput from "../../components/pure/controls/PureSelectInput";
import {useState} from "react";


const IncomeItem = ({data, onChange}: ItemComponentProps<IncomeEntity>) => {

    const [nextId, setNextId] = useState<number>(-1)

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
                       <td><PureSelectInput selected={ item.unit?.id ? {value: item.unit?.id.toString(), title: item.unit?.name ?? ''} : undefined}
                                            options={[{value:'1', title:'1'},{value:'2', title:'2'}]} onChange={() => {} } /> </td>
                       <td>{item.unit?.name}</td>
                       <td>{item.quantity}</td>
                   </tr>
                   )
               }
               </tbody>
           </table>
       </>
   )
}

export default IncomeItem