import {Form} from "react-bootstrap";

type SelectInputOptions = {
    options: Array<{value: string, title: string}>,
    onChange: (value: string) => void,
}

const PureSelectInput = ({options = [], onChange} : SelectInputOptions) => {

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

export default PureSelectInput