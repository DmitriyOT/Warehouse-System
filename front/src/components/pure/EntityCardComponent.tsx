import {Button} from "react-bootstrap";

type ItemButtonCode = 'save' | 'delete' | 'archiveToggle';

type EntityCardProps = {
    title: string,
    buttons: Array<{ code: ItemButtonCode, onClick: () => void }>,
    Component: JSX.Element,
    isArchive: boolean | undefined
}

const EntityCardComponent = ({title, buttons, Component, isArchive} : EntityCardProps) => {

    const buttonsTemplate: Array<{ code: ItemButtonCode, className: string, variant: string, text: string}> = [
        {code: 'save', className: 'me-2', variant: 'outline-success', text:'Сохранить' },
        {code: 'archiveToggle', className: 'me-2', variant: 'outline-dark', text: isArchive ? 'Из архива' : 'В архив' },
        {code: 'delete', className: '', variant: 'outline-danger', text:'Удалить'},
    ]

  return (
      <div className='h-100 w-100 d-flex flex-column'>
          <div>
              <h3 className='mt-3'>{title}</h3>
              <div className='d-flex flex-wrap mt-3 mb-3'>
                  <span className='me-2 mt-1 fs-5'>Действия: </span>
                  {buttonsTemplate?.map(button => {
                      let b = buttons.find(x => x.code === button.code);
                      if(b)
                          return <Button key={b.code} className={button.className} variant={button.variant}
                                  onClick={() => b.onClick()}>{button.text}</Button>
                      else
                          return null;
                      }
                  )}
              </div>
          </div>
          {Component}
      </div>
  )
}

export default EntityCardComponent