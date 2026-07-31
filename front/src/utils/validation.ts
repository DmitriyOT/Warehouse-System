// Минимальная клиентская валидация документов (поступление/отгрузка) перед сохранением

export type DocumentItemLike = {
    resourceId?: number | null,
    unitId?: number | null,
    quantity?: number | null,
}

export type DocumentLike = {
    number?: string | null,
    date?: Date | null,
    items?: Array<DocumentItemLike> | null,
}

// Возвращает текст первой ошибки или null, если документ валиден
export const validateDocument = (doc: DocumentLike): string | null => {
    if (!doc.number || doc.number.trim() === '')
        return 'Не заполнен номер документа';
    if (!doc.date || isNaN(doc.date.getTime()))
        return 'Не заполнена дата документа';
    const items = doc.items ?? [];
    for (let i = 0; i < items.length; i++) {
        const item = items[i];
        const row = 'Позиция ' + (i + 1) + ': ';
        if (!item.resourceId || item.resourceId <= 0)
            return row + 'не выбран ресурс';
        if (!item.unitId || item.unitId <= 0)
            return row + 'не выбрана единица измерения';
        if (!item.quantity || item.quantity <= 0)
            return row + 'количество должно быть больше нуля';
    }
    return null;
}
