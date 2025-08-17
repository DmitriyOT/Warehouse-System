export type GridColumnType = {
    field: string,
    headerName: string,
    width: number,
    renderCell?: (value: any) => any
}