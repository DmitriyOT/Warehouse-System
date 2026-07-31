export interface DashboardKpisDto {
    totalBalance: number,
    balanceDeltaPercent: number,
    incomeCount: number,
    incomeDelta: number,
    shipmentCount: number,
    shipmentDelta: number,
    activeClientCount: number,
    clientDelta: number
}

export interface DashboardDayDto {
    date: string,
    income: number,
    shipment: number
}

export interface DashboardOperationDto {
    resourceName: string,
    quantity: number
}

export interface DashboardSummaryDto {
    kpis: DashboardKpisDto,
    weekMovement: Array<DashboardDayDto>,
    lastOperations: Array<DashboardOperationDto>
}
