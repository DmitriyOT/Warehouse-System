import type {UnitEntity} from "../../types/Entities";
import UnitItem from "./UnitItem";
import {UNIT_PAGE_ROUTE} from "../../utils/consts";
import createItemPage from "../../components/ItemPageGenerator";

const UnitItemPage = createItemPage<UnitEntity>('/Unit', UNIT_PAGE_ROUTE,
    'Единица измерения', UnitItem, true )

export default UnitItemPage