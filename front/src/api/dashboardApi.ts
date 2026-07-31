import {$host, errorHandle} from "./Api";
import type {ResponseDto} from "../types/Response";
import type {ModalContextType} from "../types/Modal";
import type {DashboardSummaryDto} from "../types/Dashboard";

const createDashboardApi = function (modalC: ModalContextType) {
    return {
        getSummary: async () => {
            return await errorHandle(async () => {
                const {data} = await $host.get<ResponseDto<DashboardSummaryDto>>('/Dashboard/summary');
                return data;
            }, modalC)
        }
    }
}

const getHealth = async (): Promise<string> => {
    const {data} = await $host.get('/health');
    if (typeof data === 'string') {
        return data;
    }
    if (data?.status) {
        const entries = data.entries
            ? Object.entries(data.entries as Record<string, { status?: string }>)
                .map(([name, entry]) => `${name}: ${entry.status ?? 'OK'}`)
            : [];
        return [data.status === 'Healthy' ? 'health: OK' : `health: ${data.status}`, ...entries].join(' · ');
    }
    return String(data);
}

export { createDashboardApi, getHealth }
