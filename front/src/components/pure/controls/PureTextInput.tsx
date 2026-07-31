import {Form} from "react-bootstrap";

type PureTextInputProps = {
    value: string,
    onChange: (value: string) => void,
    id: string,
    placeholder?: string,
    disabled?: boolean,
    maxLen?: number,
    textSize?: 'large' | 'small',
}

const PureTextInput = ({value, onChange, id, placeholder, disabled, maxLen} : PureTextInputProps) => {

    return (
        <div className="w-100">
            <Form.Control
                id={id}
                value={value}
                className={value == null? "": value.length > (maxLen ?? 256) ?"text-danger":""}
                onChange={e => {onChange(e.target.value);} }
                placeholder={placeholder}
                disabled={disabled}
            />
        </div>
    )
}

export default PureTextInput