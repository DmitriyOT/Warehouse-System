import {Button} from "react-bootstrap";

type EntityCardProps = {
    title: string,
    buttons: Array<{ id: string, className: string, variant: string, text: string, onClick: () => void }>,
    Component: JSX.Element
}

const EntityCardComponent = ({title, buttons, Component} : EntityCardProps) => {
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
          {Component}
      </div>
  )
}

export default EntityCardComponent