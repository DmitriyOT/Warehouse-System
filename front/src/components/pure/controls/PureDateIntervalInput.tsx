import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import "../../style/DateInput.css"
import {DATE_FORMAT} from "../../../utils/consts";

type PureDateIntervalInputProps = {
    valueStart: string | undefined,
    valueEnd: string | undefined,
    onChange: (values: Array<string|undefined>) => void,
}

const PureDateIntervalInput = ({valueStart, valueEnd, onChange}: PureDateIntervalInputProps) => {

    const processResult = (date: Date | null) => {
        if(date === null) return undefined;
        return date.toISOString().split('T')[0];
    }

    const processValue = (value: string | undefined) => {
        return value !== undefined ? new Date(value) : undefined as unknown as Date;
    }

  return (
      <div className='w-100 d-flex'>
          <DatePicker selected={ processValue(valueStart) }
                      onChange={(e) => {let date = e as Date;onChange([processResult(date), valueEnd])} }
                      showYearDropdown
                      startDate={ processValue(valueStart) }
                      endDate={ processValue(valueEnd) }
                      selectsStart
                      isClearable={valueStart !== undefined}
                      dateFormat={DATE_FORMAT}
          />
          <div className='me-1'></div>
          <DatePicker selected={ processValue(valueEnd) }
                      onChange={(e) => {let date = e as Date;onChange([valueStart, processResult(date)])} }
                      showYearDropdown
                      startDate={ processValue(valueStart) }
                      endDate={ processValue(valueEnd) }
                      selectsEnd
                      isClearable={valueEnd !== undefined}
                      dateFormat={DATE_FORMAT}
          />
      </div>
  )
}

export default PureDateIntervalInput