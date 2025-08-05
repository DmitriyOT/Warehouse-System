import createGridPage from "../../components/GridPageGenerator";
import type {UnitEntity} from "../../types/Entities";
import {UNIT_PAGE_ROUTE} from "../../utils/consts";

const UnitPage = createGridPage<UnitEntity>('/Unit', UNIT_PAGE_ROUTE, 'Единицы измерения','Archive',
    [
        {field:'name', headerName:'Наименование', width: 300}
    ]);

export default UnitPage