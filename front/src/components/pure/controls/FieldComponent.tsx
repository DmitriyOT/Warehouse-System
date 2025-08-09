type FieldOptions = {
    name: string,
    children: React.JSX.Element
}

const FieldComponent = ({children, name}: FieldOptions) => {
  return(
      <div className={"mb-1 mt-1 w-100 d-flex fs-5"}>
          <div className={"FieldLeft d-flex text-end fw-semibold"}>
              <span className="ms-auto me-3 mt-2" >{name}</span>
          </div>
          {children}
      </div>
  )
}

export default FieldComponent