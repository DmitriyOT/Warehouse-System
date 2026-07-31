import {BrowserRouter, Route, Routes} from "react-router-dom";
import {routes} from "./utils/routes";
import {LOGIN_PAGE_ROUTE} from "./utils/consts";
import LeftMenuComponent from "./components/menu/LeftMenuComponent";
import RequireAuth from "./components/RequireAuth";
import LoginPage from "./app/login/LoginPage";
import {useMemo, useState, Suspense} from "react";
import type {Modal} from "./types/Modal";
import ModalComponent from "./components/menu/ModalComponent";
import {ModalContext} from "./context/ModalContext";

function App() {

  const [modal, setModal] = useState<Modal | null>(null);

  const modalContextValue = useMemo(() => ({modal: modal, setModal: setModal}), [modal]);

  return (
    <>
      <BrowserRouter>
          <ModalContext.Provider value={modalContextValue} >
            <Suspense fallback={<div className='p-4'>Загрузка...</div>}>
            <Routes>
              <Route path={LOGIN_PAGE_ROUTE} element={<LoginPage/>} />
              {
                  routes.map(e => <Route key={e.path} path={e.path} element={
                      <RequireAuth>
                          <div className='d-flex'>
                              <LeftMenuComponent/>
                              <div className='p-3 w-100'>
                                  <e.Component/>
                              </div>
                          </div>
                      </RequireAuth>} />)
              }
            </Routes>
            </Suspense>
            { modal && <ModalComponent {...modal} />}
          </ModalContext.Provider>
      </BrowserRouter>
    </>
  )
}

export default App
