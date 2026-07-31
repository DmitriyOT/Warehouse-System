import {useContext, useEffect, useState} from "react";
import {ModalContext} from "../../context/ModalContext";
import {createDashboardApi} from "../../api/dashboardApi";
import type {DashboardSummaryDto} from "../../types/Dashboard";
import StatCard from "./components/StatCard";
import WeekChart from "./components/WeekChart";
import OperationsList from "./components/OperationsList";
import HealthBar from "./components/HealthBar";
import TechChips from "./components/TechChips";
import "./DashboardPage.css";

const formatNumber = (value: number) => value.toLocaleString('ru-RU');

const formatSigned = (value: number) => (value > 0 ? '+' : '') + formatNumber(value);

const formatPercent = (value: number) =>
    (value > 0 ? '+' : '') + value.toLocaleString('ru-RU', {maximumFractionDigits: 1}) + '%';

const DashboardPage = () => {

    const [summary, setSummary] = useState<DashboardSummaryDto | undefined>(undefined);

    const mContext = useContext(ModalContext);

    const {getSummary} = createDashboardApi(mContext);

    useEffect(() => {
        getSummary().then(data => { if (data !== undefined) setSummary(data) });
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [])

    if (summary === undefined) {
        return (
            <div className='dashboard'>
                <div className='dashboard__empty'>Загрузка…</div>
            </div>
        )
    }

    const {kpis} = summary;

    return (
        <div className='dashboard'>
            <div className='dashboard__kpis'>
                <StatCard label='Остаток, шт' value={formatNumber(kpis.totalBalance)}
                          delta={{text: formatPercent(kpis.balanceDeltaPercent), positive: kpis.balanceDeltaPercent >= 0}}/>
                <StatCard label='Поступления' value={formatNumber(kpis.incomeCount)}
                          delta={{text: formatSigned(kpis.incomeDelta), positive: kpis.incomeDelta >= 0}}/>
                <StatCard label='Отгрузки' value={formatNumber(kpis.shipmentCount)}
                          delta={{text: formatSigned(kpis.shipmentDelta), positive: kpis.shipmentDelta >= 0}}/>
                <StatCard label='Клиенты' value={formatNumber(kpis.activeClientCount)}
                          delta={{text: formatSigned(kpis.clientDelta), positive: kpis.clientDelta >= 0}}/>
            </div>
            <div className='dashboard__middle'>
                <WeekChart days={summary.weekMovement}/>
                <OperationsList operations={summary.lastOperations}/>
            </div>
            <HealthBar/>
            <TechChips/>
        </div>
    )
}

export default DashboardPage
