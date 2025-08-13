import PureSelectInput from "./controls/PureSelectInput";
import PureTextInput from "./controls/PureTextInput";
import {Button} from "react-bootstrap";
import type {SelectOption} from "../../types/Filters";

type ItemsGridOptions = {
    incomeItems: Array<any>,
    onChange: (items: Array<any>) => any,
    nextId: number,
    setNextId: (value: number) => void,
    columns: Array<any>
}

const ItemsGridComponent = ({incomeItems, onChange, nextId, setNextId, columns} : ItemsGridOptions) => {

    const getInput = (column: {type: 'select' | 'text', options: Array<SelectOption>}, item: {value: string, title: string}) => {
        switch (column.type) {
            case 'select':
            {
                return <PureSelectInput selected={ item}
                                        options={column.options} onChange={() => {} } />
            }
        }
    }

    return(
        <table className='table-bordered2 border-dark border mt-2'>
            <thead className='bg-secondary bg-opacity-25' >
            <tr>
                <th className='p-1 '>
                    <Button className='' variant='outline-success' onClick={
                        () => {let items = incomeItems;
                            let newItem = {quantity: 0, id: nextId, resource: undefined, unit: undefined };
                            setNextId(nextId-1);
                            if(items) items.push(newItem);
                            else items = [newItem];
                            onChange(items);
                        }}>+</Button></th>
                <th>Ресурс</th>
                <th>Единица измерения</th>
                <th>Количество</th>
            </tr>
            </thead>
            <tbody>
            {
                incomeItems?.map(item =>
                    <tr key={item.id} className='border-dark border'>
                        <td className='p-1'>
                            <Button variant='outline-danger' onClick={
                            () => {onChange(incomeItems.filter((x) => x !== item) ) } }>X</Button>
                        </td>
                        {columns.map(e =>
                            <td key={e.id}>
                                {
                                    getInput(e, item)
                                }
                            </td>
                        )}
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
    )
}

export default ItemsGridComponent