import EntityGridComponent from "../components/pure/EntityGridComponent";

const ResourcePage = () => {
    return (
        <>
            <EntityGridComponent title='Ресурсы' buttons={[
                {id: "add", className:"me-2", variant:"outline-success", text:"Добавить", onClick: () => {} },
                {id: "В архив", className:"", variant:"outline-secondary", text:"В архив", onClick: () => {} }
            ]} />
        </>
    )
}

export default ResourcePage