import type {FilterOptions} from "../../types/Filters";
import PureSelectMultiInput from "./controls/PureSelectMultiInput";


const FilterComponent = ({fieldName, name, type, options, selectedOptions}:FilterOptions) => {

    //console.log('filterComponent', type, fieldName, options, selectedOptions)

    const returnSelect = () => {
        switch (type) {
            case "select":
                return <PureSelectMultiInput options={options}
                                             selectOptions={selectedOptions}
                                        onChange={(value) => console.log(value)}/>
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