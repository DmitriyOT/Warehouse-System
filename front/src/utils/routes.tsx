import type {ComponentType, LazyExoticComponent} from "react";
import {lazy} from "react";
import {
    BALANCE_PAGE_ROUTE,
    BASE_PAGE_ROUTE,
    CLIENT_PAGE_ROUTE,
    INCOME_PAGE_ROUTE, RESOURCE_PAGE_ROUTE,
    SHIPMENT_PAGE_ROUTE,
    UNIT_PAGE_ROUTE
} from "./consts";

// Страницы грузятся лениво — код списков/карточек попадает в отдельные чанки
const BasePage = lazy(() => import("../app/BasePage"));
const BalancePage = lazy(() => import("../app/balance/BalancePage"));
const IncomePage = lazy(() => import("../app/income/IncomePage"));
const ShipmentPage = lazy(() => import("../app/shipment/ShipmentPage"));
const ClientPage = lazy(() => import("../app/client/ClientPage"));
const UnitPage = lazy(() => import("../app/unit/UnitPage"));
const ResourcePage = lazy(() => import("../app/resource/ResourcePage"));
const ResourceItemPage = lazy(() => import("../app/resource/ResourceItemPage"));
const ClientItemPage = lazy(() => import("../app/client/ClientItemPage"));
const UnitItemPage = lazy(() => import("../app/unit/UnitItemPage"));
const IncomeItemPage = lazy(() => import("../app/income/IncomeItemPage"));
const ShipmentItemPage = lazy(() => import("../app/shipment/ShipmentItemPage"));

export const routes: Array<{path: string, Component: ComponentType | LazyExoticComponent<ComponentType>}> = [
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
