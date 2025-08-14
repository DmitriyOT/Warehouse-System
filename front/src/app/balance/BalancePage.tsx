import createGridPage from "../../components/GridPageGenerator";
import {
    BALANCE_API_PATH, BALANCE_PAGE_ROUTE, RESOURCE_API_PATH, UNIT_API_PATH
} from "../../utils/consts";


const BalancePage = createGridPage(BALANCE_API_PATH, BALANCE_PAGE_ROUTE, 'Баланс', 'Filters',
    [
        {field: 'resource', headerName: 'Ресурс', width: 300, renderCell: (params: any) => (params.value.name) },
        {field: 'unit', headerName: 'Единица измерения', width: 150, renderCell: (params: any) => (params.value.name) },
        {field: 'quantity', headerName: 'Количество', width: 150},
    ], [
        {fieldName: 'Resource', name: 'Ресурс', type:'select', apiPath: RESOURCE_API_PATH },
        {fieldName: 'Unit', name: 'Единица измерения', type:'select', apiPath: UNIT_API_PATH },
    ],
    false)

export default BalancePage