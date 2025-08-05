import type {IncomeEntity} from "../../types/Entities";
import IncomeItem from "./IncomeItem";
import {INCOME_API_PATH, INCOME_PAGE_ROUTE} from "../../utils/consts";
import createItemPage from "../../components/ItemPageGenerator";

const IncomeItemPage = createItemPage<IncomeEntity>(INCOME_API_PATH, INCOME_PAGE_ROUTE,
    'Поступление', IncomeItem, false )

export default IncomeItemPage