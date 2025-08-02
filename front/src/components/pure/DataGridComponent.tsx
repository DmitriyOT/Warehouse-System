import {Box, ThemeProvider} from "@mui/material";
import GridTheme from "../style/GridTheme";
import {DataGrid} from "@mui/x-data-grid";
import {Button} from "react-bootstrap";
import PurePaginationComponent from "./PurePaginationComponent";
import {PageView} from "../../types/PageView";

type DataGridProps = {
    rows: any[],
    columns: any[],
    pageView: PageView,
    onPageChange: (number) => void,
    onPageSizeChange: (number) => void,
    onItemOpen: (number) => void,
}

const DataGridComponent = ({rows, columns, pageView, onPageSizeChange, onPageChange, onItemOpen }: DataGridProps) => {
  return(
  <>
      <div className='h-100 w-100'>
          <ThemeProvider theme={GridTheme}>
              <Box className='d-flex' sx={{ height: '100%', width: '100%' }} >
                  <DataGrid rows={rows} columns={columns}
                             style={{backgroundColor: "rgba(0,0,0,0)"}}  hideFooter rowHeight={40}
                            //onRowSelectionModelChange={(ids) =>{SetSelectionIds( Array.from(ids.ids) )}}
                            getRowClassName={(params) =>
                                params.indexRelativeToCurrentPage % 2 === 0 ? 'even' : 'odd'
                            }
                            onRowDoubleClick={(params) => onItemOpen(params.id) }
                  />
              </Box>
          </ThemeProvider>
      </div>
      <PurePaginationComponent pageView={pageView} onPageChange={onPageChange} onPageSizeChange={onPageSizeChange} />
  </>
  )
}

export default DataGridComponent