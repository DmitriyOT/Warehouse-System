export type Modal = {
    header: string;
    content: string;
    buttonText: string;
    onClose: () => void;
    cancelText?: string;
    onCancel?: () => void;
}

export type ModalContextType = {
    modal: Modal | null,
    setModal: (value: Modal | null) => void
} | null