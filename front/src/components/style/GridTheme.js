import {alpha, createTheme} from "@mui/material";

const ODD_OPACITY = 0.2;

const GridTheme = createTheme({
    components: {
        MuiDataGrid: {
            defaultProps: {
                getRowClassName: (params) =>
                    params.indexRelativeToCurrentPage % 2 === 0 ? 'even' : 'odd',
            },
            styleOverrides: {
                row: ({ theme }) => ({
                    [`&.even`]: {
                        backgroundColor: theme.palette.grey[300],
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
                        //backgroundColor: theme.palette.grey[200],
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

export default GridTheme