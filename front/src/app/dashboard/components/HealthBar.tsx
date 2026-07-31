import {useEffect, useState} from "react";
import {getHealth} from "../../../api/dashboardApi";

type HealthState =
    { status: 'loading' } |
    { status: 'ok', text: string } |
    { status: 'error', text: string };

const HealthBar = () => {

    const [health, setHealth] = useState<HealthState>({status: 'loading'});

    useEffect(() => {
        getHealth()
            .then(text => setHealth({
                status: 'ok',
                text: text === 'Healthy' ? 'health: OK · postgres: OK · миграции применены' : text
            }))
            .catch((e) => setHealth({status: 'error', text: (e as unknown as Error).message}));
    }, [])

    return (
        <div className={'dashboard__health ' +
            (health.status === 'error' ? 'dashboard__health--error' : 'dashboard__health--ok')}>
            <span className={'dashboard__health-dot ' +
                (health.status === 'error' ? 'dashboard__health-dot--error' : 'dashboard__health-dot--ok')}/>
            <span className='dashboard__health-text'>
                {
                    health.status === 'loading' ? 'Проверка состояния системы…' :
                    health.status === 'ok' ? health.text :
                    'Сервис недоступен: ' + health.text
                }
            </span>
        </div>
    )
}

export default HealthBar
