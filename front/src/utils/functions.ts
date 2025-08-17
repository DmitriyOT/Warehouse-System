
export const LoadStringToDate = (item: any) => {
    for (const key in item) {
        const date = new Date(item[key])
        if (typeof item[key] === 'string' && date !== undefined) {
            item[key] = date;
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

export const UploadDateToString = (item: any) => {
    for (const key in item) {
        if (item[key] instanceof Date) {
            const d = item[key] as Date;
            item[key] = DateToStringFormat(d);
        }
    }
    return item;
}