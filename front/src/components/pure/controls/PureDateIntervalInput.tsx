import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import "../../style/DateInput.css"

type PureDateIntervalInputProps = {
    valueStart: string | undefined,
    valueEnd: string | undefined,
    onChange: (value: string) => void,
}

const PureDateIntervalInput = ({valueStart, valueEnd, onChange}: PureDateIntervalInputProps) => {

    const processResult = (date: Date) => {
        return date.toISOString().split('T')[0];
    }

  return (
      <div className='w-100'>
          <DatePicker selected={ valueStart !== undefined ? new Date(valueStart) : undefined as unknown as Date }
                      onChange={(e) => {let date = e as Date;onChange(processResult(date))} }
                      showYearDropdown />

          <DatePicker selected={ valueEnd !== undefined ? new Date(valueEnd) : undefined as unknown as Date }
                      onChange={(e) => {let date = e as Date;onChange(processResult(date))} }
                      showYearDropdown />
      </div>
  )
}

export default PureDateIntervalInput