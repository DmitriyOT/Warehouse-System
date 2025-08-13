import {
    BALANCE_PAGE_ROUTE,
    BASE_PAGE_ROUTE,
    CLIENT_PAGE_ROUTE,
    INCOME_PAGE_ROUTE, RESOURCE_PAGE_ROUTE,
    SHIPMENT_PAGE_ROUTE,
    UNIT_PAGE_ROUTE
} from "./consts";
import BasePage from "../app/BasePage";
import BalancePage from "../app/balance/BalancePage";
import IncomePage from "../app/income/IncomePage";
import ShipmentPage from "../app/shipment/ShipmentPage";
import ClientPage from "../app/client/ClientPage";
import UnitPage from "../app/unit/UnitPage";
import ResourcePage from "../app/resource/ResourcePage";
import ResourceItemPage from "../app/resource/ResourceItemPage";
import ClientItemPage from "../app/client/ClientItemPage";
import UnitItemPage from "../app/unit/UnitItemPage";
import IncomeItemPage from "../app/income/IncomeItemPage";
import ShipmentItemPage from "../app/shipment/ShipmentItemPage";

export const routes: Array<{path: string, Component: any}> = [
    {
        path: BASE_PAGE_ROUTE,
        Component: BasePage
    },
    {
        path: '*',
        Component: BasePage
    },
    {
        path: BALANCE_PAGE_ROUTE,
        Component: BalancePage,
    },
    {
        path: INCOME_PAGE_ROUTE,
        Component: IncomePage,
    },
    {
        path: INCOME_PAGE_ROUTE + '/:id',
        Component: IncomeItemPage,
    },
    {
        path: SHIPMENT_PAGE_ROUTE,
        Component: ShipmentPage,
    },
    {
        path: SHIPMENT_PAGE_ROUTE + '/:id',
        Component: ShipmentItemPage,
    },
    {
        path: CLIENT_PAGE_ROUTE,
        Component: ClientPage,
    },
    {
        path: CLIENT_PAGE_ROUTE + '/:id',
        Component: ClientItemPage,
    },
    {
        path: UNIT_PAGE_ROUTE,
        Component: UnitPage,
    },
    {
        path: UNIT_PAGE_ROUTE + '/:id',
        Component: UnitItemPage,
    },
    {
        path: RESOURCE_PAGE_ROUTE,
        Component: ResourcePage,
    },
    {
        path: RESOURCE_PAGE_ROUTE + '/:id',
        Component: ResourceItemPage,
    },
]