import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import "../../style/DateInput.css"
import {DATE_FORMAT} from "../../../utils/consts";

type PureDateIntervalInputProps = {
    valueStart: Date | undefined,
    valueEnd: Date | undefined,
    onChange: (values: Array<Date|undefined>) => void,
}

const PureDateIntervalInput = ({valueStart, valueEnd, onChange}: PureDateIntervalInputProps) => {

    const processResult = (date: Date | null) => {
        if(date === null) return undefined;
        return date;
    }

    const processValue = (value: Date | undefined) => {
        return value as unknown as Date;
    }

  return (
      <div className='w-100 d-flex'>
          <DatePicker selected={ processValue(valueStart) }
                      onChange={(e) => {let date = e as Date;onChange([processResult(date), valueEnd])} }
                      showYearDropdown
                      placeholderText={'От (включая)'}
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
                      placeholderText={'До (включая)'}
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