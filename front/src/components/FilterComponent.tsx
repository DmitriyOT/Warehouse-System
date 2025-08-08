import type {FilterOptions, SelectOption} from "../types/Filters";
import PureSelectMultiInput from "./pure/controls/PureSelectMultiInput";
import {useState} from "react";


const FilterComponent = ({fieldName, name, type, options, onChange}:FilterOptions) => {

    //console.log('filterComponent', type, fieldName, options, onChange)

    const [selectedOptions, setSelectedOptions] = useState<Array<SelectOption>>([])

    const returnSelect = () => {
        switch (type) {
            case "select":
                return <PureSelectMultiInput options={options}
                                             selectedOptions={selectedOptions}
                                        onChange={(value) => {setSelectedOptions(value); onChange!({options: value, fieldName: fieldName}); } }/>
            case "date":
                return <></>
        }
    }

    return(
      <div className='d-flex flex-column'>
        <span className='fs-5'>{name}</span>
          {
              returnSelect()
          }
      </div>
  )
}

export default FilterComponent