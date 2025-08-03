import {Button, Form} from "react-bootstrap";
import type {PageView} from "../../types/PageView";

type PurePaginationProps = {
    pageView: PageView,
    onPageChange: (page: number) => void,
    onPageSizeChange: (size: number) => void,
    pageSizes?: number[]
}

const PurePaginationComponent = ({pageView, pageSizes, onPageChange, onPageSizeChange}: PurePaginationProps) => {

    pageSizes ??= [10,20,50,100];

    const arrayPages = [
        {text: '«', id: -4, onClick: () => {onPageChange(1)}, variant: 'outline-dark'},
        {text: '‹', id: -3, onClick: () => {onPageChange(Math.max(pageView.page - 1, 1))}, variant: 'outline-dark'},
    ]

    for(let i = 1; i <= pageView.totalPages; i++)
    {
        arrayPages.push({text: i.toString(), id: i, onClick: () => {onPageChange(i)},
            variant: pageView.page === i ? 'dark' :'outline-dark'})
    }

    arrayPages.push( {text: '›', id: -2, onClick: () =>
        {onPageChange(Math.min(pageView.page + 1, pageView.totalPages))}, variant: 'outline-dark'} )
    arrayPages.push( {text: '»', id: -1, onClick: () => {onPageChange(pageView.totalPages)}, variant: 'outline-dark'} )

    return(
        <div className='d-flex flex-wrap align-items-center mt-2 justify-content-center'>
            <div>Размер страницы</div>
            <div className='ms-2 me-2 '>
                <Form.Select id='Pagination' value={pageView.size} onChange={(size) => onPageSizeChange(+size.target.value)}>
                    {pageSizes.map(s => <option key={s}>{s}</option>)}
                </Form.Select>
            </div>
             Страница {pageView.page} / {pageView.totalPages}
            <div className='ms-2 mt-2'>
                {arrayPages.map(e => <Button key={e.id} className='ms-1' variant={e.variant} onClick={e.onClick}>{e.text}</Button>)}
            </div>
        </div>
    )
}

export default PurePaginationComponent