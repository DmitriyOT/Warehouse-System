import type {FilterOptions, SelectFilterOptions, SelectOption} from "../types/Filters";
import PureSelectMultiInput from "./pure/controls/PureSelectMultiInput";
import {useContext, useEffect, useState} from "react";
import {DataProvider} from "../api/DataProvider";
import {ModalContext} from "../context/ModalContext";
import PureDateIntervalInput from "./pure/controls/PureDateIntervalInput";
import {DateToStringFormat} from "../utils/functions";
import type {BaseEntityId} from "../types/Entities";
import type {GridData} from "../types/Response";


const FilterComponent = (props:FilterOptions) => {

    const {fieldName, name, type, onChange} = props
    const apiPath = type === 'select' ? (props as SelectFilterOptions).apiPath : undefined;

    const [selectedOptions, setSelectedOptions] = useState<Array<SelectOption>>([])
    const [options, setOptions] = useState<Array<SelectOption>>([])

    const [startDate, setStartDate] = useState<Date | undefined>(undefined);
    const [endDate, setEndDate] = useState<Date | undefined>(undefined);

    const mContext = useContext(ModalContext)

    useEffect(() => {
        if (type !== 'select' || apiPath === undefined) {
            return;
        }

        const dp = new DataProvider<BaseEntityId>(apiPath, mContext, true)
        dp.getData().then(data => {
            const dataT = data as GridData<BaseEntityId>;
            const dataOp: Array<SelectOption> = dataT.items.map(e => ({value: e.id.toString(), title: (e as {name?: string, number?: string}).name ?? (e as {name?: string, number?: string}).number ?? ''}))
            setOptions(dataOp)
        })
    }, [type, apiPath, mContext])

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
      <div className='d-flex flex-column align-items-center' style={{minWidth: '200px'}}>
        <span className='fs-5'>{name}</span>
          {
              returnSelect()
          }
      </div>
  )
}

export default FilterComponent
