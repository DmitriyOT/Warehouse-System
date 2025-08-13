import {Form} from "react-bootstrap";

type PureTextInputProps = {
    value: string,
    onChange: (value: string) => void,
    name: string,
    placeholder?: string,
    disabled?: boolean,
    maxLen?: number,
    textSize?: 'large' | 'small',
    type?: 'text' | 'number'
}

const PureTextInput = ({value, onChange, name, placeholder, disabled, maxLen, textSize = 'large', type = 'text'} : PureTextInputProps) => {

    return (
        <div>
            <Form.Group className={"mb-1 mt-1 w-100 d-flex fs-5"}>
                <div className={"FieldLeft d-flex text-end fw-semibold"}>
                    <span className="ms-auto me-3 mt-2" >{name}</span>
                </div>

                <div className="W-100 me-0">
                    <Form.Control
                        id={name}
                        value={value}
                        className={(value == null? "": value.length > (maxLen ?? 256) ?"text-danger":"")
                            + ( textSize === 'large' ? " fs-5" : ' fs-6')}
                        onChange={e => {onChange(e.target.value);} }
                        placeholder={placeholder}
                        disabled={disabled}
                        type={type}
                    />

                </div>
            </Form.Group>
        </div>
    )
}

export default PureTextInput