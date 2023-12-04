const apiBase = ''

export const environment = {
    production: false,
    msalConfig: {
        auth: {
            clientId: '35a6cb55-8e13-42f0-ac46-7d0189e035c4',
            authority: 'https://login.microsoftonline.com/common'
        }
    },
    apiConfig: {
        scopes: ['user.read'],
        uri: `${apiBase}/api/*`
    },
    apiBase: apiBase,
};
