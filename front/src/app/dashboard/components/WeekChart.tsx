import type {DashboardDayDto} from "../../../types/Dashboard";

type WeekChartProps = {
    days: Array<DashboardDayDto>
}

const WIDTH = 560;
const HEIGHT = 200;
const PAD_TOP = 12;
const PAD_BOTTOM = 28;
const BAR_WIDTH = 40;

const WeekChart = ({days}: WeekChartProps) => {

    const values = days.map(d => d.income + d.shipment);
    const maxValue = Math.max(...values, 1);
    const peakIndex = values.indexOf(Math.max(...values));

    const plotHeight = HEIGHT - PAD_TOP - PAD_BOTTOM;
    const baseY = PAD_TOP + plotHeight;
    const step = WIDTH / days.length;

    const dayLabel = (date: string) => {
        const parsed = new Date(date);
        return isNaN(parsed.getTime()) ? date : parsed.toLocaleDateString('ru-RU', {weekday: 'short'});
    }

    return (
        <div className='dashboard__card dashboard__chart'>
            <div className='dashboard__card-title'>Движение товаров · неделя</div>
            <svg viewBox={`0 0 ${WIDTH} ${HEIGHT}`} className='dashboard__chart-svg' role='img'>
                {
                    days.map((day, i) => {
                        const value = values[i];
                        const height = Math.max((value / maxValue) * plotHeight, 2);
                        const x = i * step + (step - BAR_WIDTH) / 2;
                        const isPeak = i === peakIndex && value > 0;
                        return (
                            <g key={day.date}>
                                <rect x={x} y={baseY - height} width={BAR_WIDTH} height={height} rx={4}
                                      className={isPeak ? 'dashboard__chart-bar dashboard__chart-bar--peak' : 'dashboard__chart-bar'}/>
                                <text x={i * step + step / 2} y={HEIGHT - 8} textAnchor='middle'
                                      className='dashboard__chart-label'>{dayLabel(day.date)}</text>
                            </g>
                        )
                    })
                }
                <line x1={0} y1={baseY} x2={WIDTH} y2={baseY} className='dashboard__chart-baseline'/>
            </svg>
        </div>
    )
}

export default WeekChart
