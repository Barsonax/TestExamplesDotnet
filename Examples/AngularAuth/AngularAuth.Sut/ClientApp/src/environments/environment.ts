const apiBase = ''

export const environment = {
    production: false,
    msalConfig: {
        auth: {
            clientId: 'foobar',
            authority: 'https://login.microsoftonline.com/common'
        }
    },
    apiConfig: {
        scopes: ['user.read'],
        uri: `${apiBase}/api/*`
    },
    apiBase: apiBase,
};
