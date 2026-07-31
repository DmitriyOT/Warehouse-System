import {beforeEach, describe, expect, it, vi} from 'vitest';
import {render, screen, waitFor} from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import {MemoryRouter} from 'react-router-dom';
import LoginPage from './LoginPage';
import {TOKEN_KEY} from '../../utils/consts';

// Мокаем API-слой, чтобы не ходить в сеть
const {postMock} = vi.hoisted(() => ({postMock: vi.fn()}));
vi.mock('../../api/Api', () => ({$host: {post: postMock}}));

describe('LoginPage', () => {
    beforeEach(() => {
        postMock.mockReset();
        localStorage.clear();
    });

    it('рендерит форму входа', () => {
        render(<MemoryRouter><LoginPage/></MemoryRouter>);
        expect(screen.getByPlaceholderText('Введите логин')).toBeInTheDocument();
        expect(screen.getByPlaceholderText('Введите пароль')).toBeInTheDocument();
        expect(screen.getByRole('button', {name: 'Войти'})).toBeInTheDocument();
    });

    it('при успешном входе сохраняет токен в localStorage', async () => {
        postMock.mockResolvedValue({data: {hasError: false, errorMessage: '', response: {token: 'test-token'}}});
        render(<MemoryRouter><LoginPage/></MemoryRouter>);

        await userEvent.type(screen.getByPlaceholderText('Введите логин'), 'admin');
        await userEvent.type(screen.getByPlaceholderText('Введите пароль'), '12345');
        await userEvent.click(screen.getByRole('button', {name: 'Войти'}));

        await waitFor(() => expect(localStorage.getItem(TOKEN_KEY)).toBe('test-token'));
        expect(postMock).toHaveBeenCalledWith('/Auth/login', {login: 'admin', password: '12345'});
    });

    it('при ошибке входа показывает сообщение и не сохраняет токен', async () => {
        postMock.mockResolvedValue({data: {hasError: true, errorMessage: 'Неверный логин или пароль', response: null}});
        render(<MemoryRouter><LoginPage/></MemoryRouter>);

        await userEvent.type(screen.getByPlaceholderText('Введите логин'), 'admin');
        await userEvent.type(screen.getByPlaceholderText('Введите пароль'), 'wrong');
        await userEvent.click(screen.getByRole('button', {name: 'Войти'}));

        expect(await screen.findByText('Неверный логин или пароль')).toBeInTheDocument();
        expect(localStorage.getItem(TOKEN_KEY)).toBeNull();
    });
});
