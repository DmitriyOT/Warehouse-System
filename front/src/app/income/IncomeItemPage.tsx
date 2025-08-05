import type {IncomeEntity} from "../../types/Entities";
import IncomeItem from "./IncomeItem";
import {INCOME_PAGE_ROUTE} from "../../utils/consts";
import createItemPage from "../../components/ItemPageGenerator";

const IncomeItemPage = createItemPage<IncomeEntity>('/Income', INCOME_PAGE_ROUTE,
    'Поступление', IncomeItem, false )

export default IncomeItemPage