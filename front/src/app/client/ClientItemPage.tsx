import type {ResourceEntity} from "../../types/Entities";
import ClientItem from "./ClientItem";
import {CLIENT_API_PATH, CLIENT_PAGE_ROUTE} from "../../utils/consts";
import createItemPage from "../../components/ItemPageGenerator";

const ClientItemPage = createItemPage<ResourceEntity>(CLIENT_API_PATH, CLIENT_PAGE_ROUTE,
    'Клиент', ClientItem, true )

export default ClientItemPage