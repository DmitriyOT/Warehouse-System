import createGridPage from "../../components/GridPageGenerator";
import {RESOURCE_API_PATH, RESOURCE_PAGE_ROUTE} from "../../utils/consts";
import type {ResourceEntity} from "../../types/Entities";

const ResourcePage = createGridPage<ResourceEntity>(RESOURCE_API_PATH, RESOURCE_PAGE_ROUTE, 'Ресурсы','Archive',
    [
        {field:'name', headerName:'Наименование', width: 300}
    ]);

export default ResourcePage