import {useLocation, useNavigate, useParams} from "react-router-dom";
import {useContext, useEffect, useState} from "react";
import {createItemApi} from "../api/Api";
import EntityCardComponent from "./pure/EntityCardComponent";
import type {ItemButtonCode} from "./pure/EntityCardComponent";
import type {BaseEntityId, BaseEntityIdArchive} from "../types/Entities";
import {ModalContext} from "../context/ModalContext";
import type {ItemComponentProps} from "../types/Entities";
import type {ComponentType} from "react";

const createItemPage = function<T extends BaseEntityId>
    (apiPath: string, navPath: string, title: string, Component: ComponentType<ItemComponentProps<T>>, isArchive: boolean = false, hideButtons: boolean = false, editPath: string = 'EditItem'){
    const ItemPage = () => {

        const {id} = useParams()
        const [data, setData] = useState<T | undefined>(undefined)

        const mContext = useContext(ModalContext);

        const {load, deleteItems, save, archive} = createItemApi<T>(apiPath, mContext, editPath);

        const navigate = useNavigate();
        const location = useLocation();

        const LoadData = async () => {
            const response = await load(+(id ?? '0') );
            setData(response);
        }

        useEffect(() => {
            LoadData()
            // eslint-disable-next-line react-hooks/exhaustive-deps
        }, [location])

        const saveB: { code: ItemButtonCode, onClick: () => void } = {code:'save', onClick: () => {
            save(data!).then(res => { if(res !== +(id ?? '0') && res !== undefined ) navigate(navPath + '/' + res); else if(res !== undefined) navigate(navPath) } )} }

        const deleteB: { code: ItemButtonCode, onClick: () => void } = {code:'delete',
            onClick: () => { deleteItems(data!.id).then(() => navigate(navPath))} }

        return (
            <>
                <EntityCardComponent title={title}  Component={<Component id={+(id ?? '0') } data={data} onChange={setData} />}
                                     isArchive={isArchive ? (data! as unknown as BaseEntityIdArchive)?.isArchive : false}
                                     buttons={ +(id ?? '0') !== 0 ? (
                                             isArchive ?
                                            [ saveB, deleteB, {code:'archiveToggle',
                                                onClick: () => {archive(data!.id, !(data! as unknown as BaseEntityIdArchive).isArchive)
                                                    .then(() => {setData({...data!, isArchive:!(data! as unknown as BaseEntityIdArchive).isArchive} as T)})} }
                                            ]
                                             :
                                            [saveB, deleteB]
                                         )
                                         :
                                         [saveB]
                                     } hideButtons={hideButtons}
                />
            </>
        )
    }

    return ItemPage;
}

export default createItemPage;
