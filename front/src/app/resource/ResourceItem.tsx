import type {ResourceEntity} from "../../types/ResourceEntity";
import PureTextInput from "../../components/pure/controls/PureTextInput";

type ResourceItemProps = {
    id: number,
    data: ResourceEntity | undefined,
    onChange: (item: ResourceEntity) => void
}

const ResourceItem = ({data, onChange}: ResourceItemProps) => {

   return (
       <>
           <PureTextInput value={data?.name ?? ''} onChange={ (e) => onChange({...data!, name: e}) }
                          name={'Наименование'} placeholder={'Введите наименование'} />
       </>
   )
}

export default ResourceItem