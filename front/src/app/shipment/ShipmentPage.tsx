import createGridPage from "../../components/GridPageGenerator";
import {
    CLIENT_API_PATH, RESOURCE_API_PATH, SHIPMENT_API_PATH, SHIPMENT_PAGE_ROUTE, UNIT_API_PATH
} from "../../utils/consts";

const ShipmentPage = createGridPage(SHIPMENT_API_PATH, SHIPMENT_PAGE_ROUTE, 'Отгрузки', 'Filters',
    [
        {field: 'number', headerName: 'Номер', width: 150},
        {field: 'date', headerName: 'Дата', width: 100},
        {field: 'clientName', headerName: 'Клиент', width: 150},
        {field: 'isApprove', headerName: 'Статус', width: 150, renderCell:
                (params: any) => (params.value !== undefined ? <span className={(params.value ? 'bg-success' : 'bg-secondary')
                    + ' p-1 ps-2 pe-2 rounded-2 text-white'}>{params.value ? 'Подписан' : 'Не подписан'}</span> : null)},
        {field: 'resource', headerName: 'Ресурс', width: 150},
        {field: 'unit', headerName: 'Единица измерения', width: 150},
        {field: 'quantity', headerName: 'Количество', width: 150},
    ], [
        {fieldName: 'Date', name: 'Период', type:'date' },
        {fieldName: 'Id', name: 'Номер', type:'select', apiPath: SHIPMENT_API_PATH },
        {fieldName: 'Client.Id', name: 'Клиент', type:'select', apiPath: CLIENT_API_PATH },
        {fieldName: 'ShipmentItems.Resource.Id', name: 'Ресурс', type:'select', apiPath: RESOURCE_API_PATH },
        {fieldName: 'ShipmentItems.Unit.Id', name: 'Единица измерения', type:'select', apiPath: UNIT_API_PATH },
    ], true, items => {
        const rows: any[] = [];
        items.forEach(i => {
            if(i.shipmentItems !== null) {
                let isFirst = true
                i.shipmentItems.forEach((item: any) => {
                    if(isFirst) {
                        rows.push({
                            id: i.id + 'ID' + item.id,
                            number: i.number,
                            date: i.date,
                            clientName: i.clientName,
                            isApprove: i.isApprove,
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
            else
                rows.push({...i, hrefId: i.id});
        })
        return rows;
    })

export default ShipmentPage