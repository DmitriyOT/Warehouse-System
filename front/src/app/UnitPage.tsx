import EntityGridComponent from "../components/pure/EntityGridComponent";

const UnitPage = () => {
    return (
        <>
            <EntityGridComponent title='Единицы измерения'
                                 columns={[{field:'name', headerName:'Наименование', width: 300}]} rows={[]}
            />
        </>
    )
}

export default UnitPage