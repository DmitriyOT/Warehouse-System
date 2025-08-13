import ShipmentItem from "./ShipmentItem";
import {SHIPMENT_API_PATH, SHIPMENT_PAGE_ROUTE} from "../../utils/consts";
import createItemPage from "../../components/ItemPageGenerator";
import type {ShipmentEntity} from "../../types/Entities";

const ShipmentItemPage = createItemPage<ShipmentEntity>(SHIPMENT_API_PATH, SHIPMENT_PAGE_ROUTE,
    'Отгрузка', ShipmentItem, false )

export default ShipmentItemPage