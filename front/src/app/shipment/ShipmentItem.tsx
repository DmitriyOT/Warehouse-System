import type {
    ItemComponentProps,
    ResourceEntity,
    UnitEntity,
    ClientEntity, ShipmentEntity, ShipmentItemEntity
} from "../../types/Entities";
import PureTextInput from "../../components/pure/controls/PureTextInput";
import PureDateInput from "../../components/pure/controls/PureDateInput";
import FieldComponent from "../../components/pure/controls/FieldComponent";
import {useContext, useEffect, useState} from "react";
import {DataProvider} from "../../api/DataProvider";
import {
    CLIENT_API_PATH,
    RESOURCE_API_PATH,
    SHIPMENT_API_PATH,
    SHIPMENT_PAGE_ROUTE,
    UNIT_API_PATH
} from "../../utils/consts";
import type {SelectOption} from "../../types/Filters";
import type {GridData} from "../../types/Response";
import ItemsGridComponent from "../../components/pure/ItemsGridComponent";
import {ModalContext} from "../../App";
import PureSelectInput from "../../components/pure/controls/PureSelectInput";
import {Button} from "react-bootstrap";
import {createItemApi} from "../../api/Api";
import {useNavigate} from "react-router-dom";

type ShipmentButtonsCode = 'save' | 'approve' | 'disApprove' | 'delete';

const ShipmentItem = ({data, id, onChange}: ItemComponentProps<ShipmentEntity>) => {

    const mContext = useContext(ModalContext)

    const DpResource = new DataProvider<ResourceEntity>(RESOURCE_API_PATH, mContext);
    const DpUnit = new DataProvider<ResourceEntity>(UNIT_API_PATH, mContext);
    const DpClient = new DataProvider<ClientEntity>(CLIENT_API_PATH, mContext);
    const [nextId, setNextId] = useState<number>(-1)

    const [optionsResource, setOptionsResource] = useState<Array<SelectOption>>([])
    const [optionsUnit, setOptionsUnit] = useState<Array<SelectOption>>([])
    const [optionsClient, setOptionsClient] = useState<Array<SelectOption>>([])

    const [buttons, setButtons] = useState<Array<ShipmentButtonsCode>>(['save', 'approve']);

    const {deleteItems, save, archive} = createItemApi<ShipmentEntity>(SHIPMENT_API_PATH, mContext);
    const navigate = useNavigate()

    useEffect(() => {
        DpResource.getData().then(data => {
            let dataT = data as GridData<ResourceEntity>;
            setOptionsResource( dataT.items.map(e => ({value: e.id.toString(), title: e.name}) ) );
        })
        DpUnit.getData().then(data => {
            let dataT = data as GridData<UnitEntity>;
            setOptionsUnit( dataT.items.map(e => ({value: e.id.toString(), title: e.name}) ) )
        })
        DpClient.getData().then(data => {
            let dataT = data as GridData<ClientEntity>;
            setOptionsClient( dataT.items.map(e => ({value: e.id.toString(), title: e.name})))
        })

    }, [])

    const buttonsTemplate: Array<{ code: ShipmentButtonsCode, className: string, variant: string, text: string, onClick: () => void}> = [
        {code: 'save', className: 'me-2', variant: 'outline-dark', text:'Сохранить', onClick: () => {
                save(data!).then(res => { 
                    if(res !== (id ?? 0) && res !== undefined )
                        navigate(SHIPMENT_PAGE_ROUTE + '/' + res);
                    else if(res !== undefined) navigate(SHIPMENT_PAGE_ROUTE)
                } )
            } },
        {code: 'approve', className: 'me-2', variant: 'outline-success', text: 'Сохранить и подписать', onClick: () => {} },
        {code: 'disApprove', className: 'me-2', variant: 'outline-dark', text: 'Отозвать', onClick: () => {} },
        {code: 'delete', className: '', variant: 'outline-danger', text:'Удалить', onClick: () => {}},
    ]

   return (
       <>
           <div className='d-flex flex-wrap mt-3 mb-3'>
               <span className='me-2 mt-1 fs-5'>Действия: </span>
               {buttonsTemplate?.map(b => {
                       let button = buttons.find(x => x === b.code);
                       if (button)
                           return <Button key={b.code} className={b.className} variant={b.variant}
                                          onClick={() => b.onClick()}>{b.text}</Button>
                       else
                           return null;
                   }
               )}
           </div>
           <FieldComponent name='Номер' >
               <PureTextInput value={data?.number ?? ''} onChange={ (e) => onChange({...data!, number: e}) }
                          id={'Номер'} placeholder={'Введите номер'} />
           </FieldComponent>
           <FieldComponent name='Дата' >
               <PureDateInput value={data?.date} onChange={ (e) => { onChange({...data!, date: e}) } } />
           </FieldComponent>
           <FieldComponent name='Клиент' >
               <PureSelectInput options={optionsClient} onChange={(e) => { onChange({...data!, clientId: +e})} }
                                selected={{value:data?.clientId?.toString(), title: data?.clientName ?? ''}}
               />
           </FieldComponent>
           <ItemsGridComponent<ShipmentItemEntity> items={data?.shipmentItems ?? []}
                               onChange={(items) => {onChange({...data!, shipmentItems: items})}}
                               nextId={nextId} setNextId={(id) => {setNextId(id)}}
                               columns={[
                                   {id: 'resource', type: 'select', title: 'Ресурс', field: 'resourceId', options: optionsResource},
                                   {id: 'unit', type: 'select', title: 'Единица измерения', field: 'unitId', options: optionsUnit},
                                   {id: 'quantity', type: 'number', title: 'Количество', field: 'quantity', options: []},
                               ]} />
       </>
   )
}

export default ShipmentItem