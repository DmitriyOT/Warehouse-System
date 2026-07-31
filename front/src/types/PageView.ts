// Бэк: PageView.cs. Generic-обёрток нет в api-generated.ts — тип ручной.
export interface PageView {
    page: number,
    size: number,
    // Есть на бэке; фронт пока использует только totalPages
    totalCount?: number,
    totalPages: number
}
