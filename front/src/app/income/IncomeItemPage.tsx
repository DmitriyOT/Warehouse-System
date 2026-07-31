import type {IncomeEntity} from "../../types/Entities";
import IncomeItem from "./IncomeItem";
import {INCOME_API_PATH, INCOME_PAGE_ROUTE} from "../../utils/consts";
import createItemPage from "../../components/ItemPageGenerator";
import {validateDocument} from "../../utils/validation";

const IncomeItemPage = createItemPage<IncomeEntity>(INCOME_API_PATH, INCOME_PAGE_ROUTE,
    'Поступление', IncomeItem, false, false, 'Edit',
    (d) => validateDocument({number: d.number, date: d.date, items: d.incomeItems}))

export default IncomeItemPage
