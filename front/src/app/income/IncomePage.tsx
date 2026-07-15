import createGridPage from "../../components/GridPageGenerator";
import {INCOME_API_PATH, INCOME_PAGE_ROUTE, RESOURCE_API_PATH, UNIT_API_PATH} from "../../utils/consts";
import type {IncomeEntity, IncomeItemEntity} from "../../types/Entities";

type IncomeGridRow = {
    id: string;
    number?: string;
    date?: Date;
    resource?: string;
    unit?: string;
    quantity?: number;
    hrefId: number;
}

const IncomePage = createGridPage<IncomeEntity>(INCOME_API_PATH, INCOME_PAGE_ROUTE, 'Поступления', 'Filters',
    [
        {field: 'number', headerName: 'Номер', width: 150},
        {field: 'date', headerName: 'Дата', width: 100},
        {field: 'resource', headerName: 'Ресурс', width: 150},
        {field: 'unit', headerName: 'Единица измерения', width: 150},
        {field: 'quantity', headerName: 'Количество', width: 150},
    ], [
        {fieldName: 'Date', name: 'Период', type:'date' },
        {fieldName: 'Id', name: 'Номер', type:'select', apiPath: INCOME_API_PATH },
        {fieldName: 'IncomeItems.Resource.Id', name: 'Ресурс', type:'select', apiPath: RESOURCE_API_PATH },
        {fieldName: 'IncomeItems.Unit.Id', name: 'Единица измерения', type:'select', apiPath: UNIT_API_PATH },
    ], true,
    (items) => {
        const rows: IncomeGridRow[] = [];
        items.forEach(i => {
            if (i.incomeItems && i.incomeItems.length > 0) {
                let isFirst = true;
                i.incomeItems.forEach((item: IncomeItemEntity) => {
                    const base: IncomeGridRow = {
                        id: i.id + 'ID' + item.id,
                        resource: item.resource?.name,
                        unit: item.unit?.name,
                        quantity: item.quantity,
                        hrefId: i.id
                    };
                    if (isFirst) {
                        rows.push({
                            ...base,
                            number: i.number,
                            date: i.date
                        });
                        isFirst = false;
                    } else {
                        rows.push(base);
                    }
                });
            } else {
                rows.push({
                    id: i.id.toString(),
                    number: i.number,
                    date: i.date,
                    hrefId: i.id
                });
            }
        });
        return rows;
    });

export default IncomePage
