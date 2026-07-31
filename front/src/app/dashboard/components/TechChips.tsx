const TECHS = ['.NET 9', 'EF Core', 'PostgreSQL', 'React 19', 'Docker'];

const TechChips = () => {
    return (
        <div className='dashboard__chips'>
            {
                TECHS.map(tech => <span key={tech} className='dashboard__chip'>{tech}</span>)
            }
        </div>
    )
}

export default TechChips
