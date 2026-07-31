import {describe, expect, it, vi} from 'vitest';
import {AxiosError} from 'axios';
import {errorHandle} from './Api';
import type {ModalContextType} from '../types/Modal';
import type {ResponseDto} from '../types/Response';

// Мок контекста модалки: перехватываем показанные сообщения
const makeModal = () => {
    const setModal = vi.fn();
    const modalC = {modal: null, setModal} as ModalContextType;
    return {setModal, modalC};
}

describe('errorHandle', () => {
    it('возвращает response при успешном ответе', async () => {
        const {setModal, modalC} = makeModal();
        const result = await errorHandle<number>(
            async () => ({hasError: false, errorMessage: '', response: 42}), modalC);
        expect(result).toBe(42);
        expect(setModal).not.toHaveBeenCalled();
    });

    it('показывает errorMessage из тела ответа при hasError', async () => {
        const {setModal, modalC} = makeModal();
        const result = await errorHandle(async () => ({
            hasError: true, errorMessage: 'Ошибка с сервера', response: undefined
        } as unknown as ResponseDto<unknown>), modalC);
        expect(result).toBeUndefined();
        expect(setModal).toHaveBeenCalledWith(expect.objectContaining({content: 'Ошибка с сервера'}));
    });

    it('показывает errorMessage из response.data при axios-ошибке', async () => {
        const {setModal, modalC} = makeModal();
        const error = new AxiosError('Request failed', undefined, undefined, undefined, {
            status: 400,
            data: {hasError: true, errorMessage: 'Нельзя удалить используемый ресурс', response: null},
        } as never);
        const result = await errorHandle(async () => { throw error; }, modalC);
        expect(result).toBeUndefined();
        expect(setModal).toHaveBeenCalledWith(
            expect.objectContaining({content: 'Нельзя удалить используемый ресурс'}));
    });

    it('показывает «Ошибка сервера» при статусе >= 500 без errorMessage', async () => {
        const {setModal, modalC} = makeModal();
        const error = new AxiosError('Server error', undefined, undefined, undefined, {
            status: 500, data: {},
        } as never);
        await errorHandle(async () => { throw error; }, modalC);
        expect(setModal).toHaveBeenCalledWith(expect.objectContaining({content: 'Ошибка сервера'}));
    });

    it('показывает «Ошибка сети» при ошибке без ответа', async () => {
        const {setModal, modalC} = makeModal();
        const error = new AxiosError('Network Error');
        await errorHandle(async () => { throw error; }, modalC);
        expect(setModal).toHaveBeenCalledWith(expect.objectContaining({content: 'Ошибка сети'}));
    });
});
