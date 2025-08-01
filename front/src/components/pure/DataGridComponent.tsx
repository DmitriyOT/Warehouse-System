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
}

const DataGridComponent = ({rows, columns, pageView, onPageSizeChange, onPageChange }: DataGridProps) => {
  return(
  <>
      <div className='h-100 w-100'>
          <ThemeProvider theme={GridTheme}>
              <Box className='d-flex' sx={{ height: '100%', width: '100%' }} >
                  <DataGrid rows={rows} columns={columns}
                             style={{backgroundColor: "rgba(0,0,0,0)"}}  hideFooter
                            //onRowSelectionModelChange={(ids) =>{SetSelectionIds( Array.from(ids.ids) )}}
                            getRowClassName={(params) =>
                                params.indexRelativeToCurrentPage % 2 === 0 ? 'even' : 'odd'
                            }
                            /*onRowDoubleClick={(params) => {
                                user.setPath(objData.find((x) => x.id == params.id).name, 1);
                                navigate(MENU_ROUTE + '/' + id + '/' + params.id)} }
                            onColumnWidthChange={(params) => {
                                let state = apiRef.current.exportState();
                                let stateJson = JSON.stringify(state);
                                console.log(state);
                                const code = "GridTypeId"+typeData.id;
                                userSettings.setSettings(code, state);
                                saveUserSettings(code, {settings:stateJson}).then(data =>{ });
                            }}
                            apiRef={apiRef}*/
                  />
              </Box>
          </ThemeProvider>
      </div>
      <PurePaginationComponent pageView={pageView} onPageChange={onPageChange} onPageSizeChange={onPageSizeChange} />
  </>
  )
}

export default DataGridComponent