import {Button} from "react-bootstrap";
import DataGridComponent from "./DataGridComponent";
import type {PageView} from "../../types/PageView";
import type {FilterOptions} from "../../types/Filters";
import FilterComponent from "../FilterComponent";
import type {GridColumnType} from "../../types/Grid";
import {Archive, ArchiveRestore, Funnel, Plus} from "lucide-react";
import type {ReactNode} from "react";

type GridButtonsId = 'create' | 'toArchive' | 'fromArchive' | 'applyFilter'

type EntityGridProps = {
    title: string,
    buttons: Array<{id: GridButtonsId, onClick: () => void}>
    rows: Array<Record<string, unknown>>,
    columns: Array<GridColumnType>,
    pageView: PageView,
    onPageChange: (page: number) => void,
    onPageSizeChange: (size: number) => void,
    onItemOpen: (id: number) => void,
    filters: Array<FilterOptions>
}

const EntityGridComponent = ({title, buttons, rows, columns, pageView, onPageChange, onPageSizeChange,
                                             onItemOpen, filters}: EntityGridProps) => {

    const buttonsTemplate: Array<{ id: GridButtonsId, variant: string, text: string, icon: ReactNode}> = [
        {id: "applyFilter", variant:"outline-dark", text: 'Применить фильтр', icon: <Funnel size={16} /> },
        {id: "create", variant:"outline-success", text:"Добавить", icon: <Plus size={16} /> },
        {id: "toArchive", variant:"outline-secondary", text: "В архив", icon: <Archive size={16} /> },
        {id: "fromArchive", variant:"outline-secondary", text: 'Из архива', icon: <ArchiveRestore size={16} /> },

    ]

  return (
      <div className='h-100 w-100 d-flex flex-column'>
          <div className='page-header'>
              <h3 className='page-title'>{title}</h3>
              <div className='page-toolbar'>
                  {buttonsTemplate.map((bTemplate) =>
                  {
                      const button = buttons.find((x: {id: GridButtonsId}) => x.id === bTemplate.id );
                      if(button)
                        return <Button key={bTemplate.id} variant={bTemplate.variant}
                                     onClick={button.onClick}>{bTemplate.icon}{bTemplate.text}</Button>
                      else
                          return null;
                  }
                  )}
              </div>
          </div>
          {
              filters.length > 0 ?
                  <div className='filter-bar'>
                      {
                          filters.map((f: FilterOptions) =>
                              <FilterComponent key={f.name} {...f} />
                          )
                      }
                  </div>
                  :
                  null
          }

          <div className='page-card page-card--grid flex-grow-1 d-flex flex-column'>
              <DataGridComponent rows={rows} columns={columns} pageView={pageView} onItemOpen={onItemOpen}
                                 onPageChange={onPageChange} onPageSizeChange={onPageSizeChange}  />
          </div>

      </div>
  )
}

export default EntityGridComponent
