import { useEffect, useCallback } from 'react';
import {createPortal} from "react-dom";
import {Button} from "react-bootstrap";
import type {Modal} from "../../types/Modal";

const ModalComponent = ({ header, content, buttonText, onClose }: Modal) => {
    // Обработчик нажатия клавиши Escape
    const handleKeyDown = useCallback((e: KeyboardEvent) => {
        if (e.key === 'Escape') onClose();
    }, [onClose]);

    useEffect(() => {
        // Блокировка скролла страницы при открытии модалки
        document.body.style.overflow = 'hidden';
        document.addEventListener('keydown', handleKeyDown);

        return () => {
            // Восстановление скролла при закрытии
            document.body.style.overflow = '';
            document.removeEventListener('keydown', handleKeyDown);
        };
    }, [handleKeyDown]);

    // Рендеринг в отдельный DOM-элемент (портал)
    return createPortal(
        <div
            className="modal show d-block"
            style={{ backgroundColor: 'rgba(0,0,0,0.5)' }}
            onClick={onClose}
        >
            <div
                className="modal-dialog modal-dialog-centered"
                onClick={e => e.stopPropagation()}
            >
                <div className="modal-content">
                    <div className="modal-header">
                        <h5 className="modal-title">{header}</h5>
                    </div>

                    <div className="modal-body">
                        {content}
                    </div>

                    <div className="modal-footer">
                        <Button variant={'dark'} onClick={onClose} > {buttonText} </Button>
                    </div>
                </div>
            </div>
        </div>,
        document.body
    );
};

export default ModalComponent;