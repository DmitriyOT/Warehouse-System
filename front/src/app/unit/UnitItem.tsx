import type {ItemComponentProps, UnitEntity} from "../../types/Entities";
import PureTextInput from "../../components/pure/controls/PureTextInput";


const UnitItem = ({data, onChange}: ItemComponentProps<UnitEntity>) => {

   return (
       <>
           <PureTextInput value={data?.name ?? ''} onChange={ (e) => onChange({...data!, name: e}) }
                          name={'Наименование'} placeholder={'Введите наименование'} />
       </>
   )
}

export default UnitItem