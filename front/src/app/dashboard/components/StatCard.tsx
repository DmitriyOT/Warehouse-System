type StatCardProps = {
    label: string,
    value: string,
    delta?: { text: string, positive: boolean }
}

const StatCard = ({label, value, delta}: StatCardProps) => {
    return (
        <div className='dashboard__card dashboard__stat'>
            <div className='dashboard__stat-label'>{label}</div>
            <div className='dashboard__stat-value'>{value}</div>
            {
                delta &&
                <div className={'dashboard__stat-delta ' + (delta.positive ? 'dashboard__stat-delta--up' : 'dashboard__stat-delta--down')}>
                    {delta.text}
                </div>
            }
        </div>
    )
}

export default StatCard
