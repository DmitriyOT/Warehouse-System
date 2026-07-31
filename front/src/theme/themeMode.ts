import {useSyncExternalStore} from 'react';

export type ThemeMode = 'dark' | 'light';

const STORAGE_KEY = 'ws-theme';

const stored = localStorage.getItem(STORAGE_KEY);
let currentMode: ThemeMode = stored === 'light' ? 'light' : 'dark';

const subscribers = new Set<() => void>();

const applyMode = (mode: ThemeMode) => {
    document.documentElement.dataset.theme = mode;
};

// Применяем тему сразу при загрузке модуля
applyMode(currentMode);

export const getThemeMode = (): ThemeMode => currentMode;

export const setThemeMode = (mode: ThemeMode) => {
    currentMode = mode;
    localStorage.setItem(STORAGE_KEY, mode);
    applyMode(mode);
    subscribers.forEach(cb => cb());
};

const subscribe = (cb: () => void) => {
    subscribers.add(cb);
    return () => {
        subscribers.delete(cb);
    };
};

export const useThemeMode = (): [ThemeMode, () => void] => {
    const mode = useSyncExternalStore(subscribe, getThemeMode);
    const toggle = () => setThemeMode(mode === 'dark' ? 'light' : 'dark');
    return [mode, toggle];
};
