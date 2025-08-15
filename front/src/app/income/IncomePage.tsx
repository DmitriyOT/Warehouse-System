import createGridPage from "../../components/GridPageGenerator";
import {INCOME_API_PATH, INCOME_PAGE_ROUTE, RESOURCE_API_PATH, UNIT_API_PATH} from "../../utils/consts";

const IncomePage = createGridPage(INCOME_API_PATH, INCOME_PAGE_ROUTE, 'Поступления', 'Filters',
    [
        {field: 'number', headerName: 'Номер', width: 300},
        {field: 'date', headerName: 'Дата', width: 150},
    ], [
        {fieldName: 'Id', name: 'Номер', type:'select', apiPath: INCOME_API_PATH },
        {fieldName: 'IncomeItems.Resource.Id', name: 'Ресурс', type:'select', apiPath: RESOURCE_API_PATH },
        {fieldName: 'IncomeItems.Unit.Id', name: 'Единица измерения', type:'select', apiPath: UNIT_API_PATH },
    ])

export default IncomePage