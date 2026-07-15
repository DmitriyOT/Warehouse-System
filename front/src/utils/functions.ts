export const LoadStringToDate = (item: Record<string, unknown>) => {
    for (const key in item) {
        const value = item[key];
        if (typeof value === 'string') {
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
