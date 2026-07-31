import type {ReactElement} from "react";
import {Navigate} from "react-router-dom";
import {LOGIN_PAGE_ROUTE, TOKEN_KEY} from "../utils/consts";

// Обёртка защищённых роутов: без токена — редирект на страницу входа
const RequireAuth = ({children}: {children: ReactElement}) => {
    if (!localStorage.getItem(TOKEN_KEY)) {
        return <Navigate to={LOGIN_PAGE_ROUTE} replace/>
    }
    return children
}

export default RequireAuth
