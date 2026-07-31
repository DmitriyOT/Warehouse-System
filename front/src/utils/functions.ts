export const LoadStringToDate = (item: Record<string, unknown>) => {
    // Конвертируем только строки формата ISO-даты (2026-07-25 или 2026-07-25T10:00:00),
    // иначе new Date() распарсит даже номера документов вида "П-101" (год 101) и испортит данные
    const isoDatePattern = /^\d{4}-\d{2}-\d{2}(T.*)?$/;
    for (const key in item) {
        const value = item[key];
        if (typeof value === 'string' && isoDatePattern.test(value)) {
            const date = new Date(value);
            if (!isNaN(date.getTime())) {
                item[key] = date;
            }
        }
    }
}

export const DateToStringFormat = (d: Date) => {
    let month = '' + (d.getMonth() + 1);
    let day = '' + d.getDate();
    const year = d.getFullYear();

    if (month.length < 2)
        month = '0' + month;
    if (day.length < 2)
        day = '0' + day;
    return [year, month, day].join('-');
}

export const UploadDateToString = <T extends Record<string, unknown>>(item: T): T => {
    for (const key in item) {
        const value = item[key];
        if (value instanceof Date) {
            item[key] = DateToStringFormat(value) as unknown as T[Extract<keyof T, string>];
        }
    }
    return item;
}
