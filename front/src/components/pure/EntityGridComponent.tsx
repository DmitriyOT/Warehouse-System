import {Button} from "react-bootstrap";
import DataGridComponent from "./DataGridComponent";
import type {PageView} from "../../types/PageView";
import {RESOURCE_PAGE_ROUTE} from "../../utils/consts";

type GridButtonsId = 'create' | 'toArchive' | 'fromArchive'

type EntityGridProps = {
    title: string,
    buttons: Array<{id: GridButtonsId, onClick: () => void}>
    rows: Array<{id: number}>,
    columns: Array<{field: string, headerName: string, width: number}>,
    pageView: PageView,
    onPageChange: (page: number) => void,
    onPageSizeChange: (size: number) => void,
    onItemOpen: (id: number) => void,
}

const EntityGridComponent = ({title, buttons, rows, columns, pageView, onPageChange, onPageSizeChange, onItemOpen}: EntityGridProps) => {

    const buttonsTemplate: Array<{ id: GridButtonsId, className: string, variant: string, text: string}> = [
        {id: "create", className:"me-2", variant:"outline-success", text:"Добавить" },
        {id: "toArchive", className:"", variant:"outline-secondary", text: "В архив" },
        {id: "fromArchive", className:"", variant:"outline-secondary", text: 'Из архива' }
    ]

  return (
      <div className='h-100 w-100 d-flex flex-column'>
          <div>
              <h3 className='mt-3'>{title}</h3>
              <div className='d-flex flex-wrap mt-3 mb-3'>
                  <span className='me-2 mt-1 fs-5'>Действия: </span>
                  {buttonsTemplate?.map(bTemplate =>
                  {
                      let button = buttons.find(x => x.id === bTemplate.id );
                      if(button)
                        return <Button key={bTemplate.id} className={bTemplate.className} variant={bTemplate.variant}
                                     onClick={button.onClick}>{bTemplate.text}</Button>
                      else
                          return null;
                  }
                  )}
              </div>
          </div>

          <DataGridComponent rows={rows} columns={columns} pageView={pageView} onItemOpen={onItemOpen}
                             onPageChange={onPageChange} onPageSizeChange={onPageSizeChange}  />

      </div>
  )
}

export default EntityGridComponent