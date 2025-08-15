import {Form} from "react-bootstrap";
import type {SelectOption} from "../../../types/Filters";

type SelectInputOptions = {
    options: Array<SelectOption>,
    onChange: (value: string) => void,
    selected?: SelectOption | undefined
}

const PureSelectInput = ({options = [], onChange, selected} : SelectInputOptions) => {

    return(
        <>
            <Form.Select value={selected?.value ?? '-1'} onChange={(e) => {onChange(e.target.value);}}  >
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