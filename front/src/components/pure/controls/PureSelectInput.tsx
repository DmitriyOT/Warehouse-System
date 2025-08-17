import {Form} from "react-bootstrap";
import type {SelectOption} from "../../../types/Filters";

type SelectInputOptions = {
    options: Array<SelectOption>,
    onChange: (value: string) => void,
    selected?: SelectOption | undefined,
    size?: 'large' | 'medium'
}

const PureSelectInput = ({options = [], onChange, selected, size = 'large'} : SelectInputOptions) => {

    return(
        <>
            <Form.Select className={ size === "large" ? 'fs-5' : 'fs-6'}
                         value={selected?.value ?? '-1'}
                         onChange={(e) => {onChange(e.target.value);}}
            >
                {
                    options.map(e =>
                        <option key={e.title} value={e.value}>{e.title}</option>
                    )
                }
                {
                    options.find(x => x.value === selected?.value) === undefined
                        && <option value={selected?.value ?? '-1'} >{selected?.title ?? 'Не выбрано'}</option>
                }
            </Form.Select>
        </>
  )
}

export default PureSelectInput