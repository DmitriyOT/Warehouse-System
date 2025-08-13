import type {ItemComponentProps, UnitEntity} from "../../types/Entities";
import PureTextInput from "../../components/pure/controls/PureTextInput";
import FieldComponent from "../../components/pure/controls/FieldComponent";


const UnitItem = ({data, onChange}: ItemComponentProps<UnitEntity>) => {

   return (
       <>
           <FieldComponent name={'Наименование'} >
               <PureTextInput value={data?.name ?? ''} onChange={ (e) => onChange({...data!, name: e}) }
                          id={'Наименование'} placeholder={'Введите наименование'} />
           </FieldComponent>
       </>
   )
}

export default UnitItem