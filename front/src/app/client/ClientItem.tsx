import type {ClientEntity, ItemComponentProps} from "../../types/Entities";
import PureTextInput from "../../components/pure/controls/PureTextInput";
import FieldComponent from "../../components/pure/controls/FieldComponent";


const ClientItem = ({data, onChange}: ItemComponentProps<ClientEntity>) => {

    return (
        <>
            <FieldComponent name={'Наименование'} >
                <PureTextInput value={data?.name ?? ''} onChange={ (e) => onChange({...data!, name: e}) }
                           id={'Наименование'} placeholder={'Введите наименование'} />
            </FieldComponent>
            <FieldComponent name={'Адрес'} >
                <PureTextInput value={data?.address ?? ''} onChange={ (e) => onChange({...data!, address: e}) }
                           id={'Адрес'} placeholder={'Введите адрес'} />
            </FieldComponent>
        </>
    )
}

export default ClientItem