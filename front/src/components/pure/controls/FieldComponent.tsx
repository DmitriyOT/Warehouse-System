type FieldOptions = {
    name: string,
    children: React.JSX.Element
}

const FieldComponent = ({children, name}: FieldOptions) => {
  return(
      <div className={"mb-3 w-100"}>
          <div className={"FieldLabel"}>{name}</div>
          {children}
      </div>
  )
}

export default FieldComponent
