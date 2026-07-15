import createGridPage from "../../components/GridPageGenerator";
import {
    CLIENT_API_PATH, RESOURCE_API_PATH, SHIPMENT_API_PATH, SHIPMENT_PAGE_ROUTE, UNIT_API_PATH
} from "../../utils/consts";
import type {ShipmentEntity, ShipmentItemEntity} from "../../types/Entities";

type ShipmentGridRow = {
    id: string;
    number?: string;
    date?: Date;
    clientName?: string;
    isApprove?: boolean;
    resource?: string;
    unit?: string;
    quantity?: number;
    hrefId: number;
}

const ShipmentPage = createGridPage<ShipmentEntity>(SHIPMENT_API_PATH, SHIPMENT_PAGE_ROUTE, 'Отгрузки', 'Filters',
    [
        {field: 'number', headerName: 'Номер', width: 150},
        {field: 'date', headerName: 'Дата', width: 100},
        {field: 'clientName', headerName: 'Клиент', width: 150},
        {field: 'isApprove', headerName: 'Статус', width: 150, renderCell:
                (params) => (params.value !== undefined ? <span className={(params.value ? 'bg-success' : 'bg-secondary')
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
    ], true,
    (items) => {
        const rows: ShipmentGridRow[] = [];
        items.forEach(i => {
            if (i.shipmentItems && i.shipmentItems.length > 0) {
                let isFirst = true;
                i.shipmentItems.forEach((item: ShipmentItemEntity) => {
                    const base: ShipmentGridRow = {
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
                            date: i.date,
                            clientName: i.clientName,
                            isApprove: i.isApprove
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
                    clientName: i.clientName,
                    isApprove: i.isApprove,
                    hrefId: i.id
                });
            }
        });
        return rows;
    });

export default ShipmentPage
