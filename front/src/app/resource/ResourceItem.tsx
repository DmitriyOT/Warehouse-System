import type {ItemComponentProps, ResourceEntity} from "../../types/Entities";
import PureTextInput from "../../components/pure/controls/PureTextInput";


const ResourceItem = ({data, onChange}: ItemComponentProps<ResourceEntity>) => {

   return (
       <>
           <PureTextInput value={data?.name ?? ''} onChange={ (e) => onChange({...data!, name: e}) }
                          name={'Наименование'} placeholder={'Введите наименование'} />
       </>
   )
}

export default ResourceItem