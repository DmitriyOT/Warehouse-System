import {BrowserRouter, Route, Routes} from "react-router-dom";
import {routes} from "./utils/routes";
import LeftMenuComponent from "./components/menu/LeftMenuComponent";
import {createContext, useState} from "react";
import type {Modal, ModalContextType} from "./types/Modal";
import ModalComponent from "./components/menu/ModalComponent";

export const ModalContext = createContext<ModalContextType>(null)

function App() {

  const [modal, setModal] = useState<Modal>(null);

  return (
    <>
      <BrowserRouter>
          <ModalContext.Provider value={{modal: modal, setModal: setModal}} >
            <Routes>
              {
                  routes.map(e => <Route id={e.path} path={e.path} element={
                      <div className='d-flex'>
                          <LeftMenuComponent/>
                          <div className='p-3 w-100'>
                              <e.Component/>
                          </div>
                      </div>} />)
              }
            </Routes>
            { modal && <ModalComponent {...modal} />}
          </ModalContext.Provider>
      </BrowserRouter>
    </>
  )
}

export default App
