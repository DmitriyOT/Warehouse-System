import type {ItemComponentProps,IncomeEntity } from "../../types/Entities";
import PureTextInput from "../../components/pure/controls/PureTextInput";
import PureDateInput from "../../components/pure/controls/PureDateInput";
import FieldComponent from "../../components/pure/controls/FieldComponent";


const IncomeItem = ({data, onChange}: ItemComponentProps<IncomeEntity>) => {

   return (
       <>
           <PureTextInput value={data?.number ?? ''} onChange={ (e) => onChange({...data!, number: e}) }
                          name={'Номер'} placeholder={'Введите номер'} />
           <FieldComponent name='Дата' >
               <PureDateInput value={data?.date} onChange={ (e) => { onChange({...data!, date: e}) } } />
           </FieldComponent>
       </>
   )
}

export default IncomeItem