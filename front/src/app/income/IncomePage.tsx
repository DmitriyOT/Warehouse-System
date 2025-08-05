import createGridPage from "../../components/GridPageGenerator";
import {INCOME_API_PATH, INCOME_PAGE_ROUTE} from "../../utils/consts";

const IncomePage = createGridPage(INCOME_API_PATH, INCOME_PAGE_ROUTE, 'Поступления', 'Filters',
    [
        {field: 'number', headerName: 'Номер', width: 150},
        {field: 'date', headerName: 'Дата', width: 150},
    ], [
        {fieldName: 'number', name: 'Номер', type:'select'},
        {fieldName: 'resource', name: 'Ресурс', type:'select'},
        {fieldName: 'unit', name: 'Единица измерения', type:'select'},
    ])

export default IncomePage