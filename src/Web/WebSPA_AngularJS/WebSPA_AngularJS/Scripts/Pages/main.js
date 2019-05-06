var app = angular.module('pagesApp', ['ngRoute', 'ngAnimate'])
    .factory('settingsCMS', ['$http', function ($http) {

        var serverSettings = null;
        return {
            getData: function (callback) {
                console.log('Headers:');
                console.log($http.defaults.headers);
                if(serverSettings == null){
                    var baseURI = document.location.origin.endsWith('/') ? document.location.origin : document.location.origin + '/';
                    var url = baseURI + 'Home/Configuration';
                    $http.get(url, { "X-Requested-With": "XMLHttpRequest" }).then(function(result) {
                        console.log('server settings loaded');
                        serverSettings = result.data;
                        callback(serverSettings);
                    });
                } else {
                    callback(serverSettings);
                }
            }
        }
    }])
    .service('pageService', ['$http', 'settingsCMS', 'securityService', function ($http, settingsCMS, securityService) {
        return {
            getRootPage: function (callbackOk) {
                settingsCMS.getData(function(serverSettings) {
                    var url =  serverSettings.PagesUrl + '/api/v1/pages/root';
                    $http.get(url, { "X-Requested-With": "XMLHttpRequest" })
                        .then(function (data, status) {

                            if (data.statusText == 'OK' ) {
                                callbackOk(data);
                            }
                            else {
                                handleError(data.Message);
                            }
                        }, function (data, status) {
                            handleError("There was an error retrieving the page. Try it again.");
                        });
                });
            },
            getRootPagePopulated: function (callbackOk) {
                settingsCMS.getData(function(serverSettings) {
                    var url =  serverSettings.PagesUrl + '/api/v1/pages/populated/root';
                    $http.get(url, { "X-Requested-With": "XMLHttpRequest" })
                        .then(function (data, status) {

                            if (data.statusText == 'OK' ) {
                                callbackOk(data);
                            }
                            else {
                                handleError(data.Message);
                            }
                        }, function (data, status) {
                            handleError("There was an error retrieving the page. Try it again.");
                        });
                });
            },
            getPageByURL: function (pathname, callbackOk) {
                settingsCMS.getData(function(serverSettings) {
                    var url =  serverSettings.PagesUrl + '/api/v1/pages/url' + pathname;
                    $http.get(url, { "X-Requested-With": "XMLHttpRequest" })
                        .then(function (data, status) {

                            if (data.statusText == 'OK' ) {
                                callbackOk(data);
                            }
                            else {
                                handleError(data.Message);
                            }
                        }, function (data, status) {
                            handleError("There was an error retrieving the page. Try it again.");
                        });
                });
            },
            getPagePopulatedByURL: function (pathname, callbackOk) {
                settingsCMS.getData(function(serverSettings) {
                    var url =  serverSettings.PagesUrl + '/api/v1/pages/populated/url' + pathname;
                    $http.get(url, { "X-Requested-With": "XMLHttpRequest" })
                        .then(function (data, status) {

                            if (data.statusText == 'OK' ) {
                                callbackOk(data);
                            }
                            else {
                                handleError(data.Message);
                            }
                        }, function (data, status) {
                            handleError("There was an error retrieving the page. Try it again.");
                        });
                });
            },
            getBreadCrumbToTheRootByURL: function (pathname, callbackOk) {
                settingsCMS.getData(function(serverSettings) {
                    var url =  serverSettings.PagesUrl + '/api/v1/pages/breadcrumb/url' + pathname;
                    $http.get(url, { "X-Requested-With": "XMLHttpRequest" })
                        .then(function (data, status) {

                            if (data.statusText == 'OK' ) {
                                callbackOk(data);
                            }
                            else {
                                handleError(data.Message);
                            }
                        }, function (data, status) {
                            handleError("There was an error retrieving the breadcrumb. Try it again.");
                        });
                });
            },
            addPage: function (page, callbackOk) {
                settingsCMS.getData(function(serverSettings) {
                    if (!securityService.isAuthorize()) {
                        securityService.authorize();
                    }
                    var url =  serverSettings.PagesUrl + '/api/v1/pages';
                    $http.post(url, page, { "X-Requested-With": "XMLHttpRequest" })
                        .then(function (data, status) {
                            if (data.statusText == 'Created' ) {
                                callbackOk(data);
                            }
                            else {
                                handleError(data.Message);
                            }
                        }, function (data, status) {
                            if (data.status = 401) {
                                securityService.authorize();
                            }
                            handleError("There was an error creating the page. Try it again.");
                        });
                });
            },
            updatePage: function (page, callbackOk) {
                settingsCMS.getData(function(serverSettings) {
                    if (!securityService.isAuthorize()) {
                        securityService.authorize();
                    }
                    var url =  serverSettings.PagesUrl + '/api/v1/pages/' + page.id;
                    $http.put(url, page, { "X-Requested-With": "XMLHttpRequest" })
                        .then(function (data, status) {
                            if (data.statusText == 'Created' ) {
                                if (page._copy) {
                                    page._copy = null;
                                }
                                callbackOk(data);
                            }
                            else {
                                handleError(data.Message);
                            }
                        }, function (data, status) {
                            if (data.status = 401) {
                                securityService.authorize();
                            }
                            handleError("There was an error updating the page. Try it again.");
                        });
                });
            },
            deletePage: function (page, callbackOk) {
                settingsCMS.getData(function(serverSettings) {
                    if (!securityService.isAuthorize()) {
                        securityService.authorize();
                    }
                    var url =  serverSettings.PagesUrl + '/api/v1/pages/' + page.id;
                    $http.delete(url, { "X-Requested-With": "XMLHttpRequest" })
                        .then(function (data, status) {
                            if (data.statusText == 'OK' ) {
                                callbackOk(data);
                            }
                            else {
                                handleError(data.Message);
                            }
                        }, function (data, status) {
                            if (data.status = 401) {
                                securityService.authorize();
                            }
                            handleError("There was an error updating the page. Try it again.");
                        });
                });
            }
        }
    }])
    .service('zoneService', ['$http', 'settingsCMS', function ($http, settingsCMS) {
        return {
            
        }
    }])
    .service('featureService', ['$http', 'settingsCMS', function ($http, settingsCMS) {
        return {
            
        }
    }])
    .service('securityService', ['$http', 'settingsCMS', function ($http, settingsCMS) {

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
                        console.log(dataIdToken);

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


function animation() {
  return {
    // make note that other events (like addClass/removeClass)
    // have different function input parameters
    enter: function(element, doneFn) {
      jQuery(element).fadeIn(1000, doneFn);
      
      // remember to call doneFn so that AngularJS
      // knows that the animation has concluded
    },

    move: function(element, doneFn) {
      jQuery(element).fadeIn(1000, doneFn);
    },

    leave: function(element, doneFn) {
      jQuery(element).fadeOut(1000, doneFn);
    }
  }
}

app.animation('.list-group-item', [animation]);
app.animation('.page-item', [animation]);

app.controller('PagesCtrl', ['$scope', '$filter', '$element', '$animate', 'pageService', 'zoneService', 'featureService', 'securityService'
    , function($scope, $filter, $element, $animate, pageService, zoneService, featureService, securityService){

        $scope.currentPage = $scope.currentPagePopulated = $scope.rootPage = {};
        $scope.childrenPagesPopulated = [];
        $scope.breadCrumb = [];

        $scope.newPage = { };
        
        $scope.patterURL = {
            word: /^\s*\w*\s*$/
          };
        
        if (window.location.hash) {
            securityService.authorizedCallback();
        }

        console.log('isAuthorize:' + securityService.isAuthorize());
        $scope.isAuthorize = securityService.isAuthorize();
        if ($scope.isAuthorize) {
            securityService.setHeaders();
        }

        pageService.getRootPage(function (response) {
            $scope.rootPage = response.data;
            if (document.location.pathname == '/') {
                $scope.currentPage = $scope.rootPage;
            }
            console.log($scope.rootPage);
        });

        if (document.location.pathname == '/') {
            pageService.getRootPagePopulated(function (response) {
                $scope.currentPagePopulated = $filter('filter')(response.data, function(value) { return value.parent.parentId == ""})[0];
                $scope.childrenPagesPopulated = $filter('filter')(response.data, function(value) { return value.parent.parentId != ""});
                console.log(response.data);
                console.log("currentPagePopulated: " + $scope.currentPagePopulated);
                console.log("childrenPagesPopulated: " + $scope.childrenPagesPopulated);
            });

        } else {
            pageService.getPagePopulatedByURL(document.location.pathname, function (response) {
                $scope.currentPagePopulated = $filter('filter')(response.data, function(value) { return "/" +  value.parent.url == document.location.pathname })[0];
                $scope.currentPage = $scope.currentPagePopulated.parent;
                $scope.z = $filter('filter')(response.data, function(value) { return "/" +  value.parent.url == document.location.pathname })[0].parent;
                console.log(response.data);
                console.log("currentPagePopulated: " + $scope.currentPagePopulated);
                console.log("currentPage: " + $scope.currentPage);
                console.log("childrenPagesPopulated: " + $scope.childrenPagesPopulated);
            });

            pageService.getBreadCrumbToTheRootByURL(document.location.pathname, function (response) {
                $scope.breadCrumb = $filter('orderBy')(response.data, null, true);

            });
        }

        $scope.addPage = function () {
            var newPage = { id: "", name: $scope.newPage.name, url: $scope.newPage.url, parentId: $scope.currentPage.id };
            if (securityService.isAuthorize()) {
                pageService.addPage(newPage, function(data){
                    $scope.currentPagePopulated.children.push( { id: "", name: $scope.newPage.name, url: $scope.newPage.url, parentId: $scope.currentPage.id });
                    $scope.newPage = { };
                });
            } else {
                securityService.authorize();
            }
        }

        $scope.updatePage = function (page) {
            if (securityService.isAuthorize()) {
                pageService.updatePage(page, function(data){
                    $scope.editingMode(page, false);
                });
            } else {
                securityService.authorize();
            }
        }

        $scope.deletePage = function(page, index) {
            if (securityService.isAuthorize()) {
                pageService.deletePage(page, function(data){
                    $scope.currentPagePopulated.children.splice(index, 1);
                });
            } else {
                securityService.authorize();
            }
        }

        $scope.editingMode = function (page, isEditing) {
            if(isEditing){
                var pageCopy = {};
                for (key in page) {
                    pageCopy[key] = page[key];
                }
                page._copy = pageCopy;
            } else if (page._copy) {
                for (key in page._copy) {
                    page[key] = page._copy[key]; 
                }
            }
            page.isEditing = isEditing;
        }

        $scope.authorize = function () {
            securityService.authorize();
        }

        $scope.logoff = function () {
            securityService.logoff();
        }
}]);