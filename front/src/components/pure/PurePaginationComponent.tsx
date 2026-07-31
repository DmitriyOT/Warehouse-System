import {Button, Form} from "react-bootstrap";
import {ChevronLeft, ChevronRight, ChevronsLeft, ChevronsRight} from "lucide-react";
import type {PageView} from "../../types/PageView";

type PurePaginationProps = {
    pageView: PageView,
    onPageChange: (page: number) => void,
    onPageSizeChange: (size: number) => void,
    pageSizes?: number[]
}

// Окно номеров страниц вокруг текущей (не более 5)
const pageWindow = (page: number, totalPages: number): number[] => {
    let start = Math.max(1, page - 2);
    const end = Math.min(totalPages, start + 4);
    start = Math.max(1, end - 4);
    const result: number[] = [];
    for (let i = start; i <= end; i++)
        result.push(i);
    return result;
}

const PurePaginationComponent = ({pageView, pageSizes, onPageChange, onPageSizeChange}: PurePaginationProps) => {

    pageSizes ??= [10,20,50,100];

    return(
        <div className='pagination-bar'>
            <span className='pagination-bar__label'>Размер страницы</span>
            <Form.Select id='Pagination' value={pageView.size} onChange={(size) => onPageSizeChange(+size.target.value)}>
                {pageSizes.map(s => <option key={s}>{s}</option>)}
            </Form.Select>
            <span className='pagination-bar__info'>Стр. {pageView.page} из {pageView.totalPages}</span>
            <Button variant='outline-dark' disabled={pageView.page <= 1}
                    onClick={() => onPageChange(1)}><ChevronsLeft size={16} /></Button>
            <Button variant='outline-dark' disabled={pageView.page <= 1}
                    onClick={() => onPageChange(Math.max(pageView.page - 1, 1))}><ChevronLeft size={16} /></Button>
            {pageWindow(pageView.page, pageView.totalPages).map(p =>
                <Button key={p} variant={pageView.page === p ? 'dark' : 'outline-dark'}
                        onClick={() => onPageChange(p)}>{p}</Button>
            )}
            <Button variant='outline-dark' disabled={pageView.page >= pageView.totalPages}
                    onClick={() => onPageChange(Math.min(pageView.page + 1, pageView.totalPages))}><ChevronRight size={16} /></Button>
            <Button variant='outline-dark' disabled={pageView.page >= pageView.totalPages}
                    onClick={() => onPageChange(pageView.totalPages)}><ChevronsRight size={16} /></Button>
        </div>
    )
}

export default PurePaginationComponent
