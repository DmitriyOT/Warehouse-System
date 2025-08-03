import type {ResourceEntity} from "../../types/Entities";
import ResourceItem from "./ResourceItem";
import {RESOURCE_PAGE_ROUTE} from "../../utils/consts";
import createItemPage from "../../components/ItemPageGenerator";

const ResourceItemPage = createItemPage<ResourceEntity>('/Resource', RESOURCE_PAGE_ROUTE,
    'Ресурс', ResourceItem )

export default ResourceItemPage