import {alpha, createTheme} from "@mui/material";
import type {} from "@mui/x-data-grid/themeAugmentation";
import type {ThemeMode} from "../../theme/themeMode";

const ODD_OPACITY = 0.15;

// Палитры грида под две темы (значения совпадают с токенами index.css)
const palettes = {
    dark: {
        primary: '#8B93F8',
        background: '#0E1016',
        paper: '#12151D',
        text: '#9AA1B2',
        heading: '#E8EAF0',
        line: 'rgba(255,255,255,0.08)',
        lineStrong: 'rgba(255,255,255,0.16)',
        evenRow: 'rgba(255,255,255,0.03)',
    },
    light: {
        primary: '#5A63E8',
        background: '#F2F4F9',
        paper: '#FFFFFF',
        text: '#4B5268',
        heading: '#181D2C',
        line: 'rgba(17,22,40,0.10)',
        lineStrong: 'rgba(17,22,40,0.20)',
        evenRow: 'rgba(17,22,40,0.03)',
    },
};

export const getGridTheme = (mode: ThemeMode) => {
    const p = palettes[mode];

    return createTheme({
        palette: {
            mode,
            primary: {
                main: p.primary,
            },
            background: {
                default: p.background,
                paper: p.paper,
            },
            text: {
                primary: p.text,
                secondary: p.text,
            },
            divider: p.line,
        },
        components: {
            MuiDataGrid: {
                defaultProps: {
                    getRowClassName: (params) =>
                        params.indexRelativeToCurrentPage % 2 === 0 ? 'even' : 'odd',
                },
                styleOverrides: {
                    root: {
                        // Бордер и фон даёт карточка-обёртка (.page-card--grid)
                        backgroundColor: 'transparent',
                        border: 'none',
                        '--DataGrid-rowBorderColor': p.line,
                    },
                    columnHeaders: {
                        color: p.heading,
                        borderBottom: `1px solid ${p.lineStrong}`,
                    },
                    columnHeaderTitle: {
                        fontWeight: 700,
                    },
                    cell: {
                        borderColor: p.line,
                    },
                    row: ({ theme }) => ({
                        [`&.even`]: {
                            backgroundColor: p.evenRow,
                            '&:hover': {
                                backgroundColor: alpha(theme.palette.primary.main, ODD_OPACITY),
                                '@media (hover: none)': {
                                    backgroundColor: 'transparent',
                                },
                            },
                            '&.Mui-selected': {
                                backgroundColor: alpha(
                                    theme.palette.primary.main,
                                    ODD_OPACITY + theme.palette.action.selectedOpacity
                                ),
                                '&:hover': {
                                    backgroundColor: alpha(
                                        theme.palette.primary.main,
                                        ODD_OPACITY +
                                        theme.palette.action.selectedOpacity +
                                        theme.palette.action.hoverOpacity
                                    ),
                                    // Reset on touch devices, it doesn't add specificity
                                    '@media (hover: none)': {
                                        backgroundColor: alpha(
                                            theme.palette.primary.main,
                                            ODD_OPACITY + theme.palette.action.selectedOpacity
                                        ),
                                    },
                                },
                            },
                        },
                        [`&.odd`]: {
                            '&:hover': {
                                backgroundColor: alpha(theme.palette.primary.main, ODD_OPACITY),
                                '@media (hover: none)': {
                                    backgroundColor: 'transparent',
                                },
                            },
                            '&.Mui-selected': {
                                backgroundColor: alpha(
                                    theme.palette.primary.main,
                                    ODD_OPACITY + theme.palette.action.selectedOpacity
                                ),
                                '&:hover': {
                                    backgroundColor: alpha(
                                        theme.palette.primary.main,
                                        ODD_OPACITY +
                                        theme.palette.action.selectedOpacity +
                                        theme.palette.action.hoverOpacity
                                    ),
                                    // Reset on touch devices, it doesn't add specificity
                                    '@media (hover: none)': {
                                        backgroundColor: alpha(
                                            theme.palette.primary.main,
                                            ODD_OPACITY + theme.palette.action.selectedOpacity
                                        ),
                                    },
                                },
                            },
                        },


                    }),
                },
            },
        },
    });
};
