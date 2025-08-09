import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import "../../style/DateInput.css"

type PureDateInputProps = {
    value: string | undefined,
    onChange: (value: string) => void,
}

const PureDateInput = ({value, onChange}: PureDateInputProps) => {
  return (
      <div className='w-100'>
          <DatePicker selected={ value !== undefined ? new Date(value) : new Date() } onChange={(e) =>
            {let date = e as Date;onChange(date.toISOString().split('T')[0] )} } showYearDropdown />
      </div>
  )
}

export default PureDateInput