import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import "../../style/DateInput.css"
import {DATE_FORMAT} from "../../../utils/consts";

type PureDateInputProps = {
    value: Date | undefined,
    onChange: (value: Date) => void,
}

const PureDateInput = ({value, onChange}: PureDateInputProps) => {

  return (
      <div className='w-100'>
          <DatePicker selected={ value as unknown as Date }
                      onChange={(e) => {let date = e as Date;onChange(date)} }
                      showYearDropdown
                      dateFormat={DATE_FORMAT}
          />
      </div>
  )
}

export default PureDateInput