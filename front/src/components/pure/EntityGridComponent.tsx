import {Button} from "react-bootstrap";
import DataGridComponent from "./DataGridComponent";


type EntityGridProps = {
    title: string,
    buttons: Array<{id: string, className: string, variant: string, text:string, onClick: () => void}>
}

const EntityGridComponent = ({title, buttons}: EntityGridProps) => {


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

          <DataGridComponent rows={[]} columns={[]} pageView={{page: 1, totalPages: 1, size: 10}}  />

      </div>
  )
}

export default EntityGridComponent