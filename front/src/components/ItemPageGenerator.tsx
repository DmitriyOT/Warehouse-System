import {useLocation, useNavigate, useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import {createItemApi} from "../api/Api";
import EntityCardComponent, {ItemButtonCode} from "./pure/EntityCardComponent";
import type {BaseEntityId} from "../types/Entities";

const createItemPage = function<T extends BaseEntityId>
    (apiPath: string, navPath: string, title: string, Component: any, isArchive: boolean = false){
    const ItemPage = () => {

        const {id} = useParams()
        const [data, setData] = useState<T | undefined>(undefined)

        const {load, deleteItems, save, archive} = createItemApi<T>(apiPath);

        const navigate = useNavigate();
        const location = useLocation();

        const LoadData = async () => {
            const response = await load(+(id ?? '0') );
            setData(response);
        }

        useEffect(() => {
            LoadData()
        }, [location])

        let saveB: { code: ItemButtonCode, onClick: () => void } = {code:'save', onClick: () => {
            save(data!).then(res => { if(res !== +(id ?? '0') ) navigate(navPath + '/' + res) } )} }

        let deleteB: { code: ItemButtonCode, onClick: () => void } = {code:'delete',
            onClick: () => { deleteItems(data!.id).then(() => navigate(navPath))} }

        let archiveB: { code: ItemButtonCode, onClick: () => void } = {code:'archiveToggle',
            onClick: () => {archive(data!.id, !data!.isArchive).then(() => {setData({...data!, isArchive:!data!.isArchive})})} }

        return (
            <>
                <EntityCardComponent title={title}  Component={<Component id={+(id ?? '0') } data={data} onChange={setData} />} isArchive={data?.isArchive}
                                     buttons={ +(id ?? '0') !== 0 ? (
                                             isArchive ?
                                            [ saveB, deleteB, archiveB ]
                                             :
                                            [saveB, deleteB]
                                         )
                                         :
                                         [{code:'save', onClick: () => { save(data!).then(res => { if(res !== +(id ?? '0') ) navigate(navPath + '/' + res) } )} }]
                                     }
                />
            </>
        )
    }

    return ItemPage;
}

export default createItemPage;