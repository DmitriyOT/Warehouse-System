import {Box, ThemeProvider} from "@mui/material";
import GridTheme from "../style/GridTheme";
import {DataGrid} from "@mui/x-data-grid";
import PurePaginationComponent from "./PurePaginationComponent";
import type {PageView} from "../../types/PageView";

type DataGridProps = {
    rows: any[],
    columns: any[],
    pageView: PageView,
    onPageChange: (page: number) => void,
    onPageSizeChange: (size: number) => void,
    onItemOpen: (id: number) => void,
}

const DataGridComponent = ({rows, columns, pageView, onPageSizeChange, onPageChange, onItemOpen }: DataGridProps) => {
  return(
  <>
      <div className='h-100 w-100'>
          <ThemeProvider theme={GridTheme}>
              <Box className='d-flex' sx={{ height: '100%', width: '100%' }} >
                  <DataGrid rows={rows} columns={columns}
                             style={{backgroundColor: "rgba(0,0,0,0)"}}  hideFooter rowHeight={36}
                            //onRowSelectionModelChange={(ids) =>{SetSelectionIds( Array.from(ids.ids) )}}
                            getRowClassName={(params) =>
                                params.indexRelativeToCurrentPage % 2 === 0 ? 'even' : 'odd'
                            }
                            onRowDoubleClick={(params) => onItemOpen(+(params.row.hrefId ?? params.row.id)) }
                            disableColumnFilter
                            disableColumnSorting
                            sortingMode={'server'}
                  />
              </Box>
          </ThemeProvider>
      </div>
      <PurePaginationComponent pageView={pageView} onPageChange={onPageChange} onPageSizeChange={onPageSizeChange} />
  </>
  )
}

export default DataGridComponent