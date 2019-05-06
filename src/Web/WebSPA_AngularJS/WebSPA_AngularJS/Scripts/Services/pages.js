app.service('pageService', ['$http', 'settingsCMS', 'securityService', function ($http, settingsCMS, securityService) {
    return {
        getRootPage: function (callbackOk) {
            settingsCMS.getData(function(serverSettings) {
                var url =  serverSettings.PagesUrl + '/api/v1/pages/root';
                $http.get(url, { "X-Requested-With": "XMLHttpRequest" })
                    .then(function (response, status) {

                        if (response.statusText == 'OK' ) {
                            callbackOk(response.data);
                        }
                        else {
                            handleError(response.Message);
                        }
                    }, function (response, status) {
                        handleError("There was an error retrieving the page. Try it again.");
                    });
            });
        },
        getRootPagePopulated: function (callbackOk) {
            settingsCMS.getData(function(serverSettings) {
                var url =  serverSettings.PagesUrl + '/api/v1/pages/populated/root';
                $http.get(url, { "X-Requested-With": "XMLHttpRequest" })
                    .then(function (response, status) {
                        if (response.statusText == 'OK' ) {
                            callbackOk(response.data);
                        }
                        else {
                            handleError(response.Message);
                        }
                    }, function (response, status) {
                        handleError("There was an error retrieving the page. Try it again.");
                    });
            });
        },
        getPageByURL: function (pathname, callbackOk) {
            settingsCMS.getData(function(serverSettings) {
                var url =  serverSettings.PagesUrl + '/api/v1/pages/url' + pathname;
                $http.get(url, { "X-Requested-With": "XMLHttpRequest" })
                    .then(function (response, status) {
                        if (response.statusText == 'OK' ) {
                            callbackOk(response.data);
                        }
                        else {
                            handleError(response.Message);
                        }
                    }, function (response, status) {
                        handleError("There was an error retrieving the page. Try it again.");
                    });
            });
        },
        getPagePopulatedByURL: function (pathname, callbackOk) {
            settingsCMS.getData(function(serverSettings) {
                var url =  serverSettings.PagesUrl + '/api/v1/pages/populated/url' + pathname;
                $http.get(url, { "X-Requested-With": "XMLHttpRequest" })
                    .then(function (response, status) {
                        if (response.statusText == 'OK' ) {
                            callbackOk(response.data);
                        }
                        else {
                            handleError(response.Message);
                        }
                    }, function (response, status) {
                        handleError("There was an error retrieving the page. Try it again.");
                    });
            });
        },
        getBreadCrumbToTheRootByURL: function (pathname, callbackOk) {
            settingsCMS.getData(function(serverSettings) {
                var url =  serverSettings.PagesUrl + '/api/v1/pages/breadcrumb/url' + pathname;
                $http.get(url, { "X-Requested-With": "XMLHttpRequest" })
                    .then(function (response, status) {
                        if (response.statusText == 'OK' ) {
                            callbackOk(response.data);
                        }
                        else {
                            handleError(response.Message);
                        }
                    }, function (response, status) {
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
                    .then(function (response, status) {
                        if (response.statusText == 'Created' ) {
                            callbackOk(response.data);
                        }
                        else {
                            handleError(response.Message);
                        }
                    }, function (response, status) {
                        if (response.status == 401) {
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
                    .then(function (response, status) {
                        if (response.statusText == 'Created' ) {
                            if (page._copy) {
                                page._copy = null;
                            }
                            for (key in response.data) {
                                page[key] = response.data[key]; 
                            }
                            callbackOk(page);
                        }
                        else {
                            handleError(response.Message);
                        }
                    }, function (response, status) {
                        if (response.status == 401) {
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
                    .then(function (response, status) {
                        if (response.statusText == 'OK' ) {
                            callbackOk();
                        }
                        else {
                            handleError(response.Message);
                        }
                    }, function (response, status) {
                        if (response.status == 401) {
                            securityService.authorize();
                        }
                        handleError("There was an error deleting the page. Try it again.");
                    });
            });
        }
    }
}]);