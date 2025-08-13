export type Modal = {
    header: string;
    content: string;
    buttonText: string;
    onClose: () => void;
}

export type ModalContextType = {
    modal: Modal | null,
    setModal: (value: Modal | null) => void
} | null