import {useLocation, useNavigate} from "react-router-dom";
import {
    BALANCE_PAGE_ROUTE,
    BASE_PAGE_ROUTE,
    CLIENT_PAGE_ROUTE,
    INCOME_PAGE_ROUTE,
    LOGIN_PAGE_ROUTE,
    RESOURCE_PAGE_ROUTE,
    SHIPMENT_PAGE_ROUTE,
    TOKEN_KEY,
    UNIT_PAGE_ROUTE
} from "../../utils/consts.js";
import {
    ArrowDownToLine,
    ArrowUpFromLine,
    LayoutDashboard,
    LogOut,
    Moon,
    Package,
    Ruler,
    Scale,
    Sun,
    Users,
    type LucideIcon
} from "lucide-react";
import {useThemeMode} from "../../theme/themeMode";

const menu: Array<{label: string, elems: Array<{name: string, href: string, icon: LucideIcon}>}> = [
    {
        label: "Склад",
        elems: [
            {name: 'Дашборд', href: BASE_PAGE_ROUTE, icon: LayoutDashboard},
            {name: 'Баланс', href: BALANCE_PAGE_ROUTE, icon: Scale},
            {name: 'Поступления', href: INCOME_PAGE_ROUTE, icon: ArrowDownToLine},
            {name: 'Отгрузки', href: SHIPMENT_PAGE_ROUTE, icon: ArrowUpFromLine},
        ]
    },
    {
        label: "Справочники",
        elems: [
            {name: 'Клиенты', href: CLIENT_PAGE_ROUTE, icon: Users},
            {name: 'Единицы измерения', href: UNIT_PAGE_ROUTE, icon: Ruler},
            {name: 'Ресурсы', href: RESOURCE_PAGE_ROUTE, icon: Package},
        ]
    }
]

const LeftMenuComponent = () => {

    const navigate = useNavigate()
    const location = useLocation()
    const [mode, toggleTheme] = useThemeMode()

    const logout = () => {
        localStorage.removeItem(TOKEN_KEY)
        navigate(LOGIN_PAGE_ROUTE)
    }

  return (
      <div className='LeftMenu min-vh-100'>
          <div className='LeftMenu__inner'>
              <h5 className='LeftMenu__title'>Управление складом</h5>
              {
                  menu.map(e => <div key={e.label}>
                      <div className='LeftMenu__group'>{e.label}</div>
                      {
                          e.elems.map(elem =>
                          <div key={elem.href}
                               className={'LeftMenu__item' + (location.pathname === elem.href ? ' LeftMenu__item--active' : '')}
                               onClick={() => navigate(elem.href)}>
                              <elem.icon size={16} />
                              {elem.name}
                          </div>
                          )
                      }
                  </div>)
              }
          </div>
          <div className='LeftMenu__theme-toggle' onClick={logout}>
              <LogOut size={16} />
              Выйти
          </div>
          <div className='LeftMenu__theme-toggle' onClick={toggleTheme}>
              {mode === 'dark' ? <Sun size={16} /> : <Moon size={16} />}
              {mode === 'dark' ? 'Светлая тема' : 'Тёмная тема'}
          </div>
      </div>
  )
}

export default LeftMenuComponent
