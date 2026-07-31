import type {FilterOptions, SelectFilterOptions, SelectOption} from "../types/Filters";
import PureSelectMultiInput from "./pure/controls/PureSelectMultiInput";
import {useState} from "react";
import PureDateIntervalInput from "./pure/controls/PureDateIntervalInput";
import {DateToStringFormat} from "../utils/functions";
import type {BaseEntityId} from "../types/Entities";
import {useSelectOptions} from "../api/queries";


const FilterComponent = (props:FilterOptions) => {

    const {fieldName, name, type, onChange} = props
    const apiPath = type === 'select' ? (props as SelectFilterOptions).apiPath : undefined;

    const [selectedOptions, setSelectedOptions] = useState<Array<SelectOption>>([])

    const [startDate, setStartDate] = useState<Date | undefined>(undefined);
    const [endDate, setEndDate] = useState<Date | undefined>(undefined);

    const {data: fetchedOptions = []} = useSelectOptions<BaseEntityId>(type === 'select' ? apiPath : undefined, true)
    const options = fetchedOptions

    const returnSelect = () => {
        switch (type) {
            case "select":
                return <PureSelectMultiInput options={options}
                                             selectedOptions={selectedOptions}
                                        onChange={(value) => {setSelectedOptions(value);
                                            onChange!({argument: value.map(e => e.value).join(','),
                                                fieldName: fieldName, type: 'equal'});
                                        } }/>
            case "date":
                return <PureDateIntervalInput valueStart={startDate} valueEnd={endDate}
                                              onChange={([start, end]) =>
                                                {setStartDate(start); setEndDate(end); onChange!(
                                                    {argument: (start ? DateToStringFormat(start) : '') + ','
                                                        + (end ? DateToStringFormat(end) : ''),
                                                        fieldName: fieldName, type: 'dateRange'})
                                                } } />
        }
    }

    return(
      <div className='d-flex flex-column' style={{minWidth: '200px'}}>
        <span className='filter-bar__label'>{name}</span>
          {
              returnSelect()
          }
      </div>
  )
}

export default FilterComponent
