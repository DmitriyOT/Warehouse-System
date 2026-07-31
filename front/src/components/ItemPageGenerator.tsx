import {useNavigate, useParams} from "react-router-dom";
import {useContext, useEffect, useState} from "react";
import {createItemApi} from "../api/Api";
import EntityCardComponent from "./pure/EntityCardComponent";
import type {ItemButtonCode} from "./pure/EntityCardComponent";
import type {BaseEntityId, BaseEntityIdArchive} from "../types/Entities";
import {ModalContext} from "../context/ModalContext";
import type {ItemComponentProps} from "../types/Entities";
import type {ComponentType} from "react";
import {useMutation, useQuery, useQueryClient} from "@tanstack/react-query";
import {gridKey, itemKey} from "../api/queries";

const createItemPage = function<T extends BaseEntityId>
    (apiPath: string, navPath: string, title: string, Component: ComponentType<ItemComponentProps<T>>,
     isArchive: boolean = false, hideButtons: boolean = false, editPath: string = 'EditItem',
     validate?: (data: T) => string | null){
    const ItemPage = () => {

        const {id} = useParams()
        const itemId = +(id ?? '0')
        const [data, setData] = useState<T | undefined>(undefined)

        const mContext = useContext(ModalContext);
        const queryClient = useQueryClient();

        const {load, deleteItems, save, archive} = createItemApi<T>(apiPath, mContext, editPath);

        const navigate = useNavigate();

        const {data: loaded} = useQuery({
            queryKey: [...itemKey(apiPath), itemId],
            // id=0 (новая карточка) — данных нет, react-query не принимает undefined, отдаём null
            queryFn: async () => (await load(itemId)) ?? null
        })

        useEffect(() => {
            setData(loaded ?? undefined)
        }, [loaded])

        // После мутаций помечаем списки и карточку устаревшими
        const invalidate = () => {
            queryClient.invalidateQueries({queryKey: gridKey(apiPath)})
            queryClient.invalidateQueries({queryKey: itemKey(apiPath)})
        }

        const saveMutation = useMutation({mutationFn: (item: T) => save(item), onSuccess: invalidate})
        const deleteMutation = useMutation({mutationFn: (id: number) => deleteItems(id), onSuccess: invalidate})
        const archiveMutation = useMutation({
            mutationFn: (args: {itemId: number, newState: boolean}) => archive(args.itemId, args.newState),
            onSuccess: invalidate
        })

        // Перед сохранением проверяем заполненность карточки (если передана validate)
        const validateData = (): boolean => {
            if (validate === undefined)
                return true;
            const error = validate(data!);
            if (error !== null) {
                mContext?.setModal({header: 'Ошибка', content: error, buttonText: 'Ок',
                    onClose: () => mContext?.setModal(null)})
                return false;
            }
            return true;
        }

        const saveB: { code: ItemButtonCode, onClick: () => void } = {code:'save', onClick: () => {
            if (!validateData()) return;
            saveMutation.mutateAsync(data!).then(res => { if(res !== itemId && res !== undefined ) navigate(navPath + '/' + res); else if(res !== undefined) navigate(navPath) } )} }

        const deleteB: { code: ItemButtonCode, onClick: () => void } = {code:'delete',
            onClick: () => { deleteMutation.mutateAsync(data!.id).then(() => navigate(navPath))} }

        return (
            <>
                <EntityCardComponent title={title}  Component={<Component id={itemId } data={data} onChange={setData} />}
                                     isArchive={isArchive ? (data! as unknown as BaseEntityIdArchive)?.isArchive : false}
                                     buttons={ itemId !== 0 ? (
                                             isArchive ?
                                            [ saveB, deleteB, {code:'archiveToggle',
                                                onClick: () => {archiveMutation.mutateAsync({itemId: data!.id, newState: !(data! as unknown as BaseEntityIdArchive).isArchive})
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
