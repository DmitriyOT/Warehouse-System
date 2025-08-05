import type {FilterOptions} from "../../types/Filters";
import PureSelectInput from "./controls/PureSelectInput";


const FilterComponent = ({fieldName, name, type}:FilterOptions) => {

    const returnSelect = () => {
        switch (type) {
            case "select":
                return <PureSelectInput options={[{title:'1', value:'1'},{title:'2', value:'2'},{title:'3', value:'3'}]}
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