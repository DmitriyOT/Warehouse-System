import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import "../../style/DateInput.css"

type PureDateInputProps = {
    value: string | undefined,
    onChange: (value: string) => void,
}

const PureDateInput = ({value, onChange}: PureDateInputProps) => {

    const processResult = (date: Date) => {
        return date.toISOString().split('T')[0];
    }

  return (
      <div className='w-100'>
          <DatePicker selected={ value !== undefined ? new Date(value) : undefined as unknown as Date } onChange={(e) =>
            {let date = e as Date;onChange(processResult(date))} } showYearDropdown
                      dateFormat='yyyy.MM.dd'
          />
      </div>
  )
}

export default PureDateInput