import {Button} from "react-bootstrap";
import DataGridComponent from "./DataGridComponent";
import type {PageView} from "../../types/PageView";
import type {FilterOptions} from "../../types/Filters";
import FilterComponent from "./FilterComponent";

type GridButtonsId = 'create' | 'toArchive' | 'fromArchive' | 'applyFilter'

type EntityGridProps<T> = {
    title: string,
    buttons: Array<{id: GridButtonsId, onClick: () => void}>
    rows: Array<T>,
    columns: Array<{field: string, headerName: string, width: number}>,
    pageView: PageView,
    onPageChange: (page: number) => void,
    onPageSizeChange: (size: number) => void,
    onItemOpen: (id: number) => void,
    filters: Array<FilterOptions>
}

const EntityGridComponent = function<T> ({title, buttons, rows, columns, pageView, onPageChange, onPageSizeChange,
                                             onItemOpen, filters}: EntityGridProps<T>) {

    const buttonsTemplate: Array<{ id: GridButtonsId, className: string, variant: string, text: string}> = [
        {id: "applyFilter", className:"me-2", variant:"outline-dark", text: 'Применить фильтр' },
        {id: "create", className:"me-2", variant:"outline-success", text:"Добавить" },
        {id: "toArchive", className:"", variant:"outline-secondary", text: "В архив" },
        {id: "fromArchive", className:"", variant:"outline-secondary", text: 'Из архива' },

    ]

  return (
      <div className='h-100 w-100 d-flex flex-column'>
          <div>
              <h3 className='mt-3'>{title}</h3>
              {
              filters.length > 0 ?
                  <div className='d-flex flex-wrap mt-3'>
                      <span className='me-2 mt-1 fs-5'>Фильтры: </span>
                      {
                          filters?.map(f =>
                              <div className='me-3' key={f.name}>
                                  <FilterComponent name={f.name} type={f.type} fieldName={f.fieldName} {...f} />
                              </div>
                          )
                      }
                  </div>
                  :
                  null
              }
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