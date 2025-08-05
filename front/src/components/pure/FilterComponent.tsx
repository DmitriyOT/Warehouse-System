import {FilterOptions} from "../../types/Filters";
import PureSelectInput from "./controls/PureSelectInput";


const FilterComponent = ({fieldName, name, type}:FilterOptions) => {



    return(
      <div className='d-flex flex-column'>
        <span className='fs-5'>{name}</span>
        <PureSelectInput/>
      </div>
  )
}

export default FilterComponent