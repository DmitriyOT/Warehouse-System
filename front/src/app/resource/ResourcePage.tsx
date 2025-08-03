import createGridPage from "../../components/GridPageGenerator";
import {RESOURCE_PAGE_ROUTE} from "../../utils/consts";
import type {ResourceEntity} from "../../types/Entities";

const ResourcePage = createGridPage<ResourceEntity>('/Resource', RESOURCE_PAGE_ROUTE, 'Ресурсы',
    [
        {field:'name', headerName:'Наименование', width: 300}
    ]);

export default ResourcePage