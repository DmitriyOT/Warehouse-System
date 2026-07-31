import {useState, type FormEvent} from "react";
import {useNavigate} from "react-router-dom";
import axios from "axios";
import {Button, Form} from "react-bootstrap";
import {$host} from "../../api/Api";
import type {LoginRequestDto, LoginResponseDto} from "../../types/Auth";
import type {ResponseDto} from "../../types/Response";
import {AUTH_API_PATH, BASE_PAGE_ROUTE, TOKEN_KEY} from "../../utils/consts";

const LoginPage = () => {

    const navigate = useNavigate()
    const [login, setLogin] = useState('')
    const [password, setPassword] = useState('')
    const [error, setError] = useState('')
    const [loading, setLoading] = useState(false)

    const submit = async (e: FormEvent) => {
        e.preventDefault()
        setError('')
        setLoading(true)
        try {
            const request: LoginRequestDto = {login, password}
            const {data} = await $host.post<ResponseDto<LoginResponseDto>>(AUTH_API_PATH + '/login', request)
            if (!data.hasError && data.response?.token) {
                localStorage.setItem(TOKEN_KEY, data.response.token)
                navigate(BASE_PAGE_ROUTE)
            } else {
                setError(data.errorMessage || 'Неверный логин или пароль')
            }
        }
        catch (e) {
            if (axios.isAxiosError(e) && e.response?.status === 401) {
                setError('Неверный логин или пароль')
            } else {
                setError('Ошибка сервера')
            }
        }
        finally {
            setLoading(false)
        }
    }

    return (
        <div className='d-flex justify-content-center align-items-center min-vh-100'>
            <div className='w-100 p-4' style={{maxWidth: 360}}>
                <h4 className='mb-4 text-center'>Вход</h4>
                <Form onSubmit={submit}>
                    <Form.Group className='mb-3'>
                        <Form.Label>Логин</Form.Label>
                        <Form.Control value={login} onChange={e => setLogin(e.target.value)}
                                      placeholder='Введите логин' autoFocus/>
                    </Form.Group>
                    <Form.Group className='mb-3'>
                        <Form.Label>Пароль</Form.Label>
                        <Form.Control type='password' value={password} onChange={e => setPassword(e.target.value)}
                                      placeholder='Введите пароль'/>
                    </Form.Group>
                    {error && <div className='text-danger mb-3'>{error}</div>}
                    <Button variant='dark' type='submit' className='w-100' disabled={loading}>
                        Войти
                    </Button>
                </Form>
            </div>
        </div>
    )
}

export default LoginPage
