import {Form} from "react-bootstrap";
import type {SelectOption} from "../../../types/Filters";

type SelectMultiInputOptions = {
    options: Array<SelectOption>,
    selectOptions: Array<SelectOption>,
    onChange: (values: Array<SelectOption>) => void,
}

const PureSelectMultiInput = ({options = [], selectOptions = [], onChange} : SelectMultiInputOptions) => {

    return(
        <>
            <Form.Select  onChange={(e) => {onChange(e.target.value);}}  >
                {
                    options.map(e =>
                        <option key={e.title} value={e.value}>{e.title}</option>
                    )
                }
            </Form.Select>
        </>
  )
}

export default PureSelectMultiInput