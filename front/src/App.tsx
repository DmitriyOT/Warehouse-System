import {BrowserRouter, Route, Routes} from "react-router-dom";
import {routes} from "./utils/routes";
import LeftMenuComponent from "./components/menu/LeftMenuComponent";

function App() {

  return (
    <>
      <BrowserRouter>
        <Routes>
          {
              routes.map(e => <Route id={e.path} path={e.path} element={
                  <div className='d-flex'>
                      <LeftMenuComponent/>
                      <div className='p-3'>
                          <e.Component/>
                      </div>
                  </div>} />)
          }
        </Routes>
      </BrowserRouter>
    </>
  )
}

export default App
