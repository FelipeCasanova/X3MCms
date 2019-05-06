app.service('securityService', ['$http', 'settingsCMS', function ($http, settingsCMS) {

    var authorityUrl = '';
    var isAuthorized;
    var userData;

    getDataFromToken = function(token) {
        var data = {};
        if (typeof token !== 'undefined') {
            var encoded = token.split('.')[1];
            data = JSON.parse(this.urlBase64Decode(encoded));
        }
        return data;
    };

    urlBase64Decode = function(str) {
        var output = str.replace('-', '+').replace('_', '/');
        switch (output.length % 4) {
            case 0:
                break;
            case 2:
                output += '==';
                break;
            case 3:
                output += '=';
                break;
            default:
                throw 'Illegal base64url string!';
        }

        return window.atob(output);
    };

    setHeaders = function() {

        $http.defaults.headers.common["Content-Type"] = 'application/json';
        $http.defaults.headers.common["Accept"] = 'application/json';

        var token = getToken();
        if (token !== '') {
            $http.defaults.headers.common.Authorization = 'Bearer ' + token;
        }
    };

    getToken = function() {
        return localStorage.getItem('authorizationData');
    }

    getUserData = function (callbackOk) {
        settingsCMS.getData(function(serverSettings) {
            setHeaders();
            var userInfoUrl =  serverSettings.IdentityUrl + '/connect/userinfo';
            $http.get(userInfoUrl, { "X-Requested-With": "XMLHttpRequest" })
                .then(function (data, status) {

                    if (data.statusText == 'OK' ) {
                        callbackOk(data);
                    }
                    else {
                        handleError(data.Message);
                    }
                }, function (data, status) {
                    handleError("There was an error retrieving the user information. Try it again.");
                });
        });
    };

    return {
        authorize: function () {
            settingsCMS.getData(function(serverSettings) {
                var authorizationUrl =  serverSettings.IdentityUrl + '/connect/authorize';
                var client_id = 'js';
                var redirect_uri = location.origin + '/';
                var response_type = 'id_token token';
                var scope = 'openid profile pages';
                var nonce = 'N' + Math.random() + '' + Date.now();
                var state = Date.now() + '' + Math.random();

                localStorage.setItem("authStateControl", state);
                localStorage.setItem("authNonce", nonce);

                var url =
                    authorizationUrl + '?' +
                    'response_type=' + encodeURI(response_type) + '&' +
                    'client_id=' + encodeURI(client_id) + '&' +
                    'redirect_uri=' + encodeURI(redirect_uri) + '&' +
                    'scope=' + encodeURI(scope) + '&' +
                    'nonce=' + encodeURI(nonce) + '&' +
                    'state=' + encodeURI(state);

                window.location.href = url;
            });
        },
        authorizedCallback: function () {
        
            var hash = window.location.hash.substr(3);

            var result = hash.split('&').reduce(function (result, item) {
                var parts = item.split('=');
                result[parts[0]] = parts[1];
                return result;
            }, {});

            if(!result.access_token)
                return;

            this.resetAuthorizationData();

            console.log(result);

            var token = '';
            var id_token = '';
            var authResponseIsValid = false;

            if (!result.error) {

                if (result.state !== localStorage.getItem('authStateControl')) {
                    console.log('AuthorizedCallback incorrect state');
                } else {

                    token = result.access_token;
                    id_token = result.id_token;

                    var dataIdToken = getDataFromToken(id_token);

                    // validate nonce
                    if (dataIdToken.nonce !== localStorage.getItem('authNonce')) {
                        console.log('AuthorizedCallback incorrect nonce');
                    } else {
                        localStorage.setItem('authNonce', '');
                        localStorage.setItem('authStateControl', '');

                        authResponseIsValid = true;
                        console.log('AuthorizedCallback state and nonce validated, returning access token');
                    }
                }
            }


            if (authResponseIsValid) {
                this.setAuthorizationData(token, id_token);
            }
        },
        resetAuthorizationData: function () {
            localStorage.setItem('authorizationData', '');
            localStorage.setItem('authorizationDataIdToken', '');

            isAuthorized = false;
            localStorage.setItem('isAuthorized', false);
            localStorage.setItem('userData', '');
        },
        getToken: function() {
            getToken();
        },
        setAuthorizationData: function (token, id_token) {
            if (localStorage.getItem('authorizationData') !== '') {
                localStorage.setItem('authorizationData', '');
            }

            localStorage.setItem('authorizationData', token);
            localStorage.setItem('authorizationDataIdToken', id_token);
            isAuthorized = true;
            localStorage.setItem('isAuthorized', true);

            getUserData(function(response) {
                userData = response.data;
                localStorage.setItem('userData', userData);
                window.location.href = location.origin;
            });
        },
        isAuthorize: function () {
            if (!isAuthorized) {
                isAuthorized = localStorage.getItem('isAuthorized') === 'true';
            }
            return isAuthorized;
        },
        setHeaders: function () {
            setHeaders();
        },
        logoff: function () {
            var _this = this;
            settingsCMS.getData(function(serverSettings) {
                var authorizationUrl = serverSettings.IdentityUrl + '/connect/endsession';
                var id_token_hint = localStorage.getItem('authorizationDataIdToken');
                var post_logout_redirect_uri = location.origin + '/';

                var url =
                    authorizationUrl + '?' +
                    'id_token_hint=' + encodeURI(id_token_hint) + '&' +
                    'post_logout_redirect_uri=' + encodeURI(post_logout_redirect_uri);

                _this.resetAuthorizationData();
                window.location.href = url;
            });
        }
    }
}]);