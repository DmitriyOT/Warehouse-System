import type {ResourceEntity} from "../../types/Entities";
import ClientItem from "./ClientItem";
import {CLIENT_PAGE_ROUTE} from "../../utils/consts";
import createItemPage from "../../components/ItemPageGenerator";

const ClientItemPage = createItemPage<ResourceEntity>('/Client', CLIENT_PAGE_ROUTE,
    'Клиент', ClientItem )

export default ClientItemPage