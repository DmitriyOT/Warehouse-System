import type {ItemComponentProps,IncomeEntity } from "../../types/Entities";
import PureTextInput from "../../components/pure/controls/PureTextInput";


const IncomeItem = ({data, onChange}: ItemComponentProps<IncomeEntity>) => {

   return (
       <>
           <PureTextInput value={data?.number ?? ''} onChange={ (e) => onChange({...data!, number: e}) }
                          name={'Номер'} placeholder={'Введите номер'} />
           <PureTextInput value={data?.date.toString() ?? ''} onChange={ (e) => onChange({...data!, date: e}) }
                          name={'Дата'} placeholder={'Введите дату'} />
       </>
   )
}

export default IncomeItem