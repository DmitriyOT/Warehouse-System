import type {PageView} from "../types/PageView";

export const BASE_PAGE_ROUTE: string = '/'

export const UNIT_PAGE_ROUTE: string = '/unit'
export const RESOURCE_PAGE_ROUTE: string = '/resource'
export const CLIENT_PAGE_ROUTE: string = '/client'

export const BALANCE_PAGE_ROUTE: string = '/balance'
export const INCOME_PAGE_ROUTE: string = '/income'
export const SHIPMENT_PAGE_ROUTE: string = '/shipment'

export const DEFAULT_PAGE_VIEW: PageView = {page: 1, size: 10, totalPages: 0}