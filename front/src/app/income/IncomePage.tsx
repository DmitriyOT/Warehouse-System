import createGridPage from "../../components/GridPageGenerator";
import {INCOME_API_PATH, INCOME_PAGE_ROUTE, RESOURCE_API_PATH, UNIT_API_PATH} from "../../utils/consts";

const IncomePage = createGridPage(INCOME_API_PATH, INCOME_PAGE_ROUTE, 'Поступления', 'Filters',
    [
        {field: 'number', headerName: 'Номер', width: 150},
        {field: 'date', headerName: 'Дата', width: 100},
        {field: 'resource', headerName: 'Ресурс', width: 150},
        {field: 'unit', headerName: 'Единица измерения', width: 150},
        {field: 'quantity', headerName: 'Количество', width: 150},
    ], [
        {fieldName: 'Id', name: 'Номер', type:'select', apiPath: INCOME_API_PATH },
        {fieldName: 'IncomeItems.Resource.Id', name: 'Ресурс', type:'select', apiPath: RESOURCE_API_PATH },
        {fieldName: 'IncomeItems.Unit.Id', name: 'Единица измерения', type:'select', apiPath: UNIT_API_PATH },
    ], true,
        items =>
        {
            const rows = [];
            items.forEach(i => {
                if(i.incomeItems.length > 0) {
                    let isFirst = true
                    i.incomeItems.forEach(item => {
                        if(isFirst) {
                            rows.push({
                                id: i.id + 'ID' + item.id,
                                number: i.number,
                                date: i.date,
                                resource: item.resource.name,
                                unit: item.unit.name,
                                quantity: item.quantity,
                                hrefId: i.id
                            })
                            isFirst = false;
                        }
                        else {
                            rows.push({
                                id: i.id + 'ID' + item.id,
                                resource: item.resource.name,
                                unit: item.unit.name,
                                quantity: item.quantity,
                                hrefId: i.id
                            })
                        }
                    })
                }
                else {
                    rows.push({...i, hrefId: i.id});
                }
            })
            return rows;
        })

export default IncomePage