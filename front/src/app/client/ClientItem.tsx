import type {ClientEntity, ItemComponentProps} from "../../types/Entities";
import PureTextInput from "../../components/pure/controls/PureTextInput";


const ClientItem = ({data, onChange}: ItemComponentProps<ClientEntity>) => {

    return (
        <>
            <PureTextInput value={data?.name ?? ''} onChange={ (e) => onChange({...data!, name: e}) }
                           name={'Наименование'} placeholder={'Введите наименование'} />
            <PureTextInput value={data?.address ?? ''} onChange={ (e) => onChange({...data!, address: e}) }
                           name={'Адрес'} placeholder={'Введите адрес'} />
        </>
    )
}

export default ClientItem