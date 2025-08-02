import {Form} from "react-bootstrap";

type PureTextInputProps = {
    value: string,
    onChange: (value: string) => void,
    name: string,
    rows?: number,
    placeholder?: string,
    disabled?: boolean,
    maxLen?: number
}

const PureTextInput = ({value, onChange, name, rows, placeholder, disabled, maxLen} : PureTextInputProps) => {

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
                        as={rows !== undefined?'textarea':"input"}
                        rows={rows}
                        className={(value == null? "": value.length > maxLen?"text-danger":"")
                            + " fs-5"}
                        onChange={e => {onChange(e.target.value);if(rows != null)
                            e.target.style.height = (e.target.scrollHeight + "px");} }
                        placeholder={placeholder}
                        disabled={disabled}
                    />

                </div>
            </Form.Group>
        </div>
    )
}

export default PureTextInput