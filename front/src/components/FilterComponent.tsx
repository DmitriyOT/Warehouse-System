import type {FilterOptions, SelectOption} from "../types/Filters";
import PureSelectMultiInput from "./pure/controls/PureSelectMultiInput";
import {useEffect, useState} from "react";


const FilterComponent = ({fieldName, name, type, onChange}:FilterOptions) => {

    //console.log('filterComponent', type, fieldName, options, onChange)

    const [selectedOptions, setSelectedOptions] = useState<Array<SelectOption>>([])
    const [options, setOptions] = useState<Array<SelectOption>>([])

    useEffect(() => {
        switch (type)
        {
            case "select":
            {
                //load options ?
                setOptions([{value:'1',title:'1'}]);
                break;
            }
            case "date":
                break;
        }
    }, [])

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
      <div className='d-flex flex-column align-items-center' style={{minWidth: '200px'}}>
        <span className='fs-5'>{name}</span>
          {
              returnSelect()
          }
      </div>
  )
}

export default FilterComponent