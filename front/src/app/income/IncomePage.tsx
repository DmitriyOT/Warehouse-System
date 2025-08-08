import createGridPage from "../../components/GridPageGenerator";
import {INCOME_API_PATH, INCOME_PAGE_ROUTE} from "../../utils/consts";

const IncomePage = createGridPage(INCOME_API_PATH, INCOME_PAGE_ROUTE, 'Поступления', 'Filters',
    [
        {field: 'number', headerName: 'Номер', width: 150},
        {field: 'date', headerName: 'Дата', width: 150},
    ], [
        {fieldName: 'Number', name: 'Номер', type:'select', options: [{value: '1', title: '1'},{value: '2', title: '2'},{value: '3', title: '3'}] },
        {fieldName: 'Resource', name: 'Ресурс', type:'select', options: [{value: '1', title: '1'},{value: '2', title: '2'},{value: '3', title: '3'}] },
        {fieldName: 'Unit', name: 'Единица измерения', type:'select', options: [{value: '1', title: '1'},{value: '2', title: '2'},{value: '3', title: '3'}] },
    ])

export default IncomePage