import type {FilterOptions, SelectFilterOptions, SelectOption} from "../types/Filters";
import PureSelectMultiInput from "./pure/controls/PureSelectMultiInput";
import {useContext, useEffect, useState} from "react";
import {DataProvider} from "../api/DataProvider";
import {ModalContext} from "../App";


const FilterComponent = (props:FilterOptions) => {

    const {fieldName, name, type, onChange} = props
    //console.log('filterComponent', type, fieldName, options, onChange)

    const [selectedOptions, setSelectedOptions] = useState<Array<SelectOption>>([])
    const [options, setOptions] = useState<Array<SelectOption>>([])

    const mContext = useContext(ModalContext)

    useEffect(() => {
        switch (type)
        {
            case "select":
            {
                const dp = new DataProvider<any>((props as SelectFilterOptions).apiPath, mContext)
                dp.getData().then(data => {
                    let dataOp: Array<SelectOption> = []
                    data.items.forEach((e: any) => dataOp.push({value: e.id, title: e.name ?? e.number}))
                    setOptions(dataOp)
                })
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