import {useLocation, useNavigate, useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import {createItemApi} from "../api/Api";
import EntityCardComponent from "./pure/EntityCardComponent";
import type {BaseEntityIdArchive} from "../types/Entities";

const createItemPage = function<T extends BaseEntityIdArchive>
    (apiPath: string, navPath: string, title: string, Component: any){
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

        return (
            <>
                <EntityCardComponent title={title}  Component={<Component id={+(id ?? '0') } data={data} onChange={setData} />} isArchive={data?.isArchive}
                                     buttons={ +(id ?? '0') !== 0 ? [
                                             {code:'save', onClick: () => { save(data!).then(res => { if(res !== +(id ?? '0') ) navigate(navPath + '/' + res) } )} },
                                             {code:'delete', onClick: () => { deleteItems(data!.id).then(() => navigate(navPath))} },
                                             {code:'archiveToggle', onClick: () => {archive(data!.id, !data!.isArchive).then(() => {setData({...data!, isArchive:!data!.isArchive})})} }
                                         ]
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