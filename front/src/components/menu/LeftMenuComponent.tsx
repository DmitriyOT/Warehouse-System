import {Button} from "react-bootstrap";
import {useLocation, useNavigate} from "react-router-dom";
import {
    BALANCE_PAGE_ROUTE,
    CLIENT_PAGE_ROUTE,
    INCOME_PAGE_ROUTE,
    RESOURCE_PAGE_ROUTE,
    SHIPMENT_PAGE_ROUTE,
    UNIT_PAGE_ROUTE
} from "../../utils/consts.js";

const menu: Array<{label: string, elems: Array<{name: string, href: string}>}> = [
    {
        label: "Склад",
        elems: [
            {name: 'Баланс', href: BALANCE_PAGE_ROUTE},
            {name: 'Поступления', href: INCOME_PAGE_ROUTE},
            {name: 'Отгрузки', href: SHIPMENT_PAGE_ROUTE},
        ]
    },
    {
        label: "Справочники",
        elems: [
            {name: 'Клиенты', href: CLIENT_PAGE_ROUTE},
            {name: 'Единицы измерения', href: UNIT_PAGE_ROUTE},
            {name: 'Ресурсы', href: RESOURCE_PAGE_ROUTE},
        ]
    }
]

const LeftMenuComponent = () => {

    const navigate = useNavigate()

    const location = useLocation()

  return (
      <div className='min-vh-100 p-3' style={{width: '300px'}}>
          <div className='Block h-100'>
              <h5>Управление складом</h5>
              {
                  menu.map(e => <>
                      <h3 className='mt-4 mb-3'>{e.label}</h3>
                      {
                          e.elems.map(elem =>
                          <div className='w-100 ps-2'>
                            <Button className='w-100 mt-1 text-start' onClick={() => navigate(elem.href)}
                                    variant={location.pathname === elem.href ? 'dark' : 'outline-dark'} >
                                {elem.name}
                            </Button>
                          </div>
                          )
                      }
                  </>)
              }
          </div>
      </div>
  )
}

export default LeftMenuComponent