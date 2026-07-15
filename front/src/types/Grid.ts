import type {GridRenderCellParams} from "@mui/x-data-grid";

export type GridColumnType = {
    field: string,
    headerName: string,
    width: number,
    renderCell?: (params: GridRenderCellParams) => React.ReactNode
}
