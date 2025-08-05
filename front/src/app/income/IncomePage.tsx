import createGridPage from "../../components/GridPageGenerator";
import {INCOME_PAGE_ROUTE} from "../../utils/consts";

const IncomePage = createGridPage('/Income', INCOME_PAGE_ROUTE, 'Поступления', 'Filters',
    [
        {field: 'number', headerName: 'Номер', width: 150},
        {field: 'date', headerName: 'Дата', width: 150},
    ])

export default IncomePage