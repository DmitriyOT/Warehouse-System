import createGridPage from "../../components/GridPageGenerator";
import {
    CLIENT_API_PATH, RESOURCE_API_PATH, SHIPMENT_API_PATH, SHIPMENT_PAGE_ROUTE, UNIT_API_PATH
} from "../../utils/consts";

const ShipmentPage = createGridPage(SHIPMENT_API_PATH, SHIPMENT_PAGE_ROUTE, 'Отгрузки', 'Filters',
    [
        {field: 'number', headerName: 'Номер', width: 300},
        {field: 'date', headerName: 'Дата', width: 150},
        {field: 'clientName', headerName: 'Клиент', width: 150},
        {field: 'isApprove', headerName: 'Статус', width: 150, renderCell:
                (params: any) => (<span className={(params.value ? 'bg-success' : 'bg-secondary')
                    + ' p-1 ps-2 pe-2 rounded-2 text-white'}>{params.value ? 'Подписан' : 'Не подписан'}</span>)},
    ], [
        {fieldName: 'Date', name: 'Период', type:'date' },
        {fieldName: 'Number', name: 'Номер', type:'select', apiPath: SHIPMENT_API_PATH },
        {fieldName: 'Client', name: 'Клиент', type:'select', apiPath: CLIENT_API_PATH },
        {fieldName: 'Resource', name: 'Ресурс', type:'select', apiPath: RESOURCE_API_PATH },
        {fieldName: 'Unit', name: 'Единица измерения', type:'select', apiPath: UNIT_API_PATH },
    ])

export default ShipmentPage