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
            <Form.Select onChange={(e) => {onChange(e.target.value);}}  >
                {
                    options.map(e =>
                        <option key={e.title} selected={e === selected} value={e.value}>{e.title}</option>
                    )
                }
            </Form.Select>
        </>
  )
}

export default PureSelectInput