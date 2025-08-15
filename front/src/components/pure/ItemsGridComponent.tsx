import PureSelectInput from "./controls/PureSelectInput";
import PureTextInput from "./controls/PureTextInput";
import { Button } from "react-bootstrap";
import type { SelectOption } from "../../types/Filters";
import PureNumberInput from "./controls/PureNumberInput";
import type {BaseEntityId} from "../../types/Entities";

// Определение типа для колонки
type GridColumn = {
    id: string;
    title: string;
    type: 'select' | 'text' | 'number';
    field: string;
    source?: string;
    options?: Array<SelectOption>; // Обязательно для типа 'select'
};

type ItemsGridOptions<T> = {
    items: Array<T>;
    onChange: (items: Array<T>) => void;
    nextId: number;
    setNextId: (value: number) => void;
    columns: Array<GridColumn>;
};

const ItemsGridComponent = function<T extends BaseEntityId> ({ items, onChange, nextId, setNextId, columns }: ItemsGridOptions<T>) {

    // Обработчик добавления нового элемента
    const handleAddItem = () => {
        const newItem: any = { id: nextId };

        // Инициализация полей по колонкам
        /*columns.forEach(column => {
            newItem[column.field] = column.type === 'select'
                ? column.options?.at(0)?.value
                : '';
        });*/

        setNextId(nextId - 1);
        const newItems = [...items, newItem];
        onChange(newItems);
    };

    // Обработчик удаления элемента
    const handleDeleteItem = (item: any) => {
        onChange(items.filter(x => x.id !== item.id));
    };

    // Функция получения компонента ввода
    const getInput = (column: GridColumn, item: any) => {
        const handleFieldChange = (newValue: any) => {
            let updatedItem;
            if(item[column.source] !== undefined)
                updatedItem = { ...item, [column.field]: newValue, [column.source]: {...item[column.source], id: newValue} };
            else
                updatedItem = { ...item, [column.field]: newValue };
            const newItems = items.map(i =>
                i.id === item.id ? updatedItem : i
            );
            onChange(newItems);
        };

        switch (column.type) {
            case 'select':
                if (!column.options) return null;

                const currentValue = item[column.source];

                return (
                    <PureSelectInput
                        selected={{value:currentValue?.id.toString() ?? item[column.field], title: currentValue?.name ?? currentValue?.number}}
                        options={column.options}
                        onChange={(selected) =>
                            handleFieldChange(selected)
                        }
                        size={'medium'}
                    />
                );

            case 'text':
                return (
                    <PureTextInput
                        value={String(item[column.field])}
                        onChange={(value) =>
                            handleFieldChange(value)
                        }
                        id={column.field}
                        textSize={'small'}
                    />
                );
            case 'number':
                return (
                    <PureNumberInput
                        value={item[column.field]}
                        onChange={(value) =>
                            handleFieldChange(value)
                        }
                        id={column.field}
                        textSize={'small'}
                    />
                );

            default:
                return null;
        }
    };

    return (
        <table className='table-bordered2 border-dark border mt-2'>
            <thead className='bg-secondary bg-opacity-25'>
            <tr>
                <th className='p-1'>
                    <Button
                        variant='outline-success'
                        onClick={handleAddItem}
                    >
                        +
                    </Button>
                </th>
                {columns.map(column => (
                    <th key={column.id}>{column.title}</th>
                ))}
            </tr>
            </thead>
            <tbody>
            {items?.map(item => (
                <tr key={item.id} className='border-dark border'>
                    <td className='p-1'>
                        <Button
                            variant='outline-danger'
                            onClick={() => handleDeleteItem(item)}
                        >
                            X
                        </Button>
                    </td>
                    {columns.map(column => (
                        <td key={column.id}>
                            {getInput(column, item)}
                        </td>
                    ))}
                </tr>
            ))}
            </tbody>
        </table>
    );
};

export default ItemsGridComponent;