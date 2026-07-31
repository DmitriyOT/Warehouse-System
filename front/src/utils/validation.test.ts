import {describe, expect, it} from 'vitest';
import {validateDocument} from './validation';

const validDoc = {
    number: 'ПР-001',
    date: new Date('2026-07-31'),
    items: [{resourceId: 1, unitId: 2, quantity: 5}],
}

describe('validateDocument', () => {
    it('валидный документ проходит проверку', () => {
        expect(validateDocument(validDoc)).toBeNull();
    });

    it('пустой номер не проходит', () => {
        expect(validateDocument({...validDoc, number: ''})).toBe('Не заполнен номер документа');
        expect(validateDocument({...validDoc, number: '   '})).toBe('Не заполнен номер документа');
        expect(validateDocument({...validDoc, number: undefined})).toBe('Не заполнен номер документа');
    });

    it('незаполненная дата не проходит', () => {
        expect(validateDocument({...validDoc, date: undefined})).toBe('Не заполнена дата документа');
        expect(validateDocument({...validDoc, date: new Date('invalid')})).toBe('Не заполнена дата документа');
    });

    it('невыбранный ресурс у позиции не проходит', () => {
        expect(validateDocument({...validDoc, items: [{resourceId: 0, unitId: 2, quantity: 5}]}))
            .toBe('Позиция 1: не выбран ресурс');
    });

    it('невыбранная единица измерения у позиции не проходит', () => {
        expect(validateDocument({...validDoc, items: [{resourceId: 1, unitId: 0, quantity: 5}]}))
            .toBe('Позиция 1: не выбрана единица измерения');
    });

    it('отрицательное и нулевое количество не проходит', () => {
        expect(validateDocument({...validDoc, items: [{resourceId: 1, unitId: 2, quantity: -3}]}))
            .toBe('Позиция 1: количество должно быть больше нуля');
        expect(validateDocument({...validDoc, items: [{resourceId: 1, unitId: 2, quantity: 0}]}))
            .toBe('Позиция 1: количество должно быть больше нуля');
    });

    it('документ без позиций проходит проверку', () => {
        expect(validateDocument({...validDoc, items: []})).toBeNull();
    });
});
