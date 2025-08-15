import type {PageView} from "../types/PageView";

export const BASE_PAGE_ROUTE: string = '/'


export const UNIT_PAGE_ROUTE: string = '/unit'
export const UNIT_API_PATH: string = '/Unit'

export const RESOURCE_PAGE_ROUTE: string = '/resource'
export const RESOURCE_API_PATH: string = '/Resource'

export const CLIENT_PAGE_ROUTE: string = '/client'
export const CLIENT_API_PATH: string = '/Client'


export const BALANCE_PAGE_ROUTE: string = '/balance'
export const BALANCE_API_PATH: string = '/Balance'

export const INCOME_PAGE_ROUTE: string = '/income'
export const INCOME_API_PATH: string = '/Income'

export const SHIPMENT_PAGE_ROUTE: string = '/shipment'
export const SHIPMENT_API_PATH: string = '/Shipment'


export const DEFAULT_PAGE_VIEW: PageView = {page: 1, size: 10, totalPages: 0}
export const DATE_FORMAT: string = 'yyyy-MM-dd'