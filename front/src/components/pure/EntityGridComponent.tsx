import {Button} from "react-bootstrap";
import DataGridComponent from "./DataGridComponent";
import {PageView} from "../../types/PageView";


type EntityGridProps = {
    title: string,
    buttons: Array<{id: string, className: string, variant: string, text:string, onClick: () => void}>
    rows: any[],
    columns: Array<{field: string, headerName: string, width: number}>,
    pageView: PageView,
    onPageChange: (number) => void,
    onPageSizeChange: (number) => void,
    onItemOpen: (number) => void,
}

const EntityGridComponent = ({title, buttons, rows, columns, pageView, onPageChange, onPageSizeChange, onItemOpen}: EntityGridProps) => {


  return (
      <div className='h-100 w-100 d-flex flex-column'>
          <div>
              <h3 className='mt-3'>{title}</h3>
              <div className='d-flex flex-wrap mt-3 mb-3'>
                  <span className='me-2 mt-1 fs-5'>Действия: </span>
                  {buttons?.map(b =>
                      <Button key={b.id} className={b.className} variant={b.variant} onClick={b.onClick}>{b.text}</Button>
                  )}
              </div>
          </div>

          <DataGridComponent rows={rows} columns={columns} pageView={pageView} onItemOpen={onItemOpen}
                             onPageChange={onPageChange} onPageSizeChange={onPageSizeChange}  />

      </div>
  )
}

export default EntityGridComponent