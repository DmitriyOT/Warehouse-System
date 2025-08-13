import {Form} from "react-bootstrap";

type PureTextInputProps = {
    value: number,
    onChange: (value: number) => void,
    id: string,
    disabled?: boolean,
    textSize?: 'large' | 'small',
}

const PureNumberInput = ({value, onChange, id, placeholder, disabled, textSize = 'large'} : PureTextInputProps) => {

    return (
        <div className="w-100">
            <Form.Control
                id={id}
                value={value}
                className={( textSize === 'large' ? " fs-5" : ' fs-6')}
                onChange={e => {onChange(Number(e.target.value) );} }
                placeholder={placeholder}
                disabled={disabled}
                type={'number'}
            />
        </div>
    )
}

export default PureNumberInput