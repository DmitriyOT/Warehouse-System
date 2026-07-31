import {Form} from "react-bootstrap";

type PureNumberInputProps = {
    value: number,
    onChange: (value: number) => void,
    id: string,
    placeholder?: string,
    disabled?: boolean,
    textSize?: 'large' | 'small',
}

const PureNumberInput = ({value, onChange, id, placeholder, disabled} : PureNumberInputProps) => {

    return (
        <div className="w-100">
            <Form.Control
                id={id}
                value={value}
                onChange={e => {
                    // Пустой/некорректный ввод не прокидываем как NaN — считаем нулём
                    const n = (e.target as HTMLInputElement).valueAsNumber;
                    onChange(Number.isNaN(n) ? 0 : n);
                } }
                placeholder={placeholder}
                disabled={disabled}
                type={'number'}
            />
        </div>
    )
}

export default PureNumberInput