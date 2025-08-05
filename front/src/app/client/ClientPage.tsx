import createGridPage from "../../components/GridPageGenerator";
import type {ClientEntity} from "../../types/Entities";
import {CLIENT_PAGE_ROUTE} from "../../utils/consts";

const ClientPage = createGridPage<ClientEntity>('/Client', CLIENT_PAGE_ROUTE, 'Клиенты', 'Archive',
    [
        {field:'name', headerName:'Наименование', width: 300},
        {field:'address', headerName:'Адрес', width: 300}
    ]);

export default ClientPage