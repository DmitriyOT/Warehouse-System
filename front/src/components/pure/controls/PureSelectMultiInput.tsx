import {Dropdown} from "react-bootstrap";
import type {SelectOption} from "../../../types/Filters";

type SelectMultiInputOptions = {
    options: Array<SelectOption>,
    selectedOptions: Array<SelectOption>,
    onChange: (values: Array<SelectOption>) => void,
}

const PureSelectMultiInput = ({options = [], selectedOptions = [], onChange} : SelectMultiInputOptions) => {

    const handleToggleOption = (option: SelectOption) => {
        const isSelected = selectedOptions.some(
            selected => selected.value === option.value
        );

        if (isSelected) {
            onChange(
                selectedOptions.filter( selected => selected.value !== option.value )
            );
        } else {
            onChange([...selectedOptions, option]);
        }
    };

    return(
        <Dropdown className='w-100'>
            <Dropdown.Toggle variant="outline-dark" className='w-100 d-flex align-items-center'>
                <div className='d-flex gap-1 flex-wrap me-auto '>
                    {
                        selectedOptions.length > 0 ?
                        selectedOptions.map(e =>
                            <span key={e.value} style={{maxWidth: '50vw'}} className='SelectedItem ps-1 pe-1 rounded-1 overflow-auto'>{e.title}</span>
                        )
                            :
                            <span>Выберите</span>
                    }
                </div>

            </Dropdown.Toggle>

            <Dropdown.Menu className="p-2 overflow-auto" style={{maxHeight: '50vh'}}>
                {options.map(option => {
                    const isSelected = selectedOptions.some(
                        selected => selected.value === option.value
                    );

                    return (
                        <Dropdown.Item
                            key={option.value}
                            as="div"
                            style={{maxWidth: '50vw'}}
                            className={"px-2 overflow-auto " + (isSelected? "bg-secondary bg-opacity-50":"")}
                            onClick={(e) => {
                                e.preventDefault();
                                e.stopPropagation();
                                handleToggleOption(option);
                            }}
                        >
                            <span>{option.title}</span>
                        </Dropdown.Item>
                    );
                })}
            </Dropdown.Menu>
        </Dropdown>
  )
}

export default PureSelectMultiInput