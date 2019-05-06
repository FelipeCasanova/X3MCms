app.service('zoneService', ['$http', 'settingsCMS', 'securityService', function ($http, settingsCMS, securityService) {
    return {
        addZone: function (zone, callbackOk) {
            settingsCMS.getData(function(serverSettings) {
                if (!securityService.isAuthorize()) {
                    securityService.authorize();
                }
                var url =  serverSettings.PagesUrl + '/api/v1/zones';
                $http.post(url, zone, { "X-Requested-With": "XMLHttpRequest" })
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
                        handleError("There was an error creating the zone. Try it again.");
                    });
            });
        },
        updateZone: function (zone, callbackOk) {
            settingsCMS.getData(function(serverSettings) {
                if (!securityService.isAuthorize()) {
                    securityService.authorize();
                }
                var url =  serverSettings.PagesUrl + '/api/v1/zones/' + zone.id;
                $http.put(url, zone, { "X-Requested-With": "XMLHttpRequest" })
                    .then(function (response, status) {
                        if (response.statusText == 'Created' ) {
                            if (zone._copy) {
                                zone._copy = null;
                            }
                            for (key in response.data) {
                                zone[key] = response.data[key]; 
                            }
                            callbackOk(zone);
                        }
                        else {
                            handleError(response.Message);
                        }
                    }, function (response, status) {
                        if (response.status == 401) {
                            securityService.authorize();
                        }
                        handleError("There was an error updating the zone. Try it again.");
                    });
            });
        },
        deleteZone: function (zone, callbackOk) {
            settingsCMS.getData(function(serverSettings) {
                if (!securityService.isAuthorize()) {
                    securityService.authorize();
                }
                var url =  serverSettings.PagesUrl + '/api/v1/zones/' + zone.id;
                $http.delete(url, { "X-Requested-With": "XMLHttpRequest" })
                    .then(function (response, status) {
                        if (response.statusText == 'OK' ) {
                            callbackOk(response.statusText);
                        }
                        else {
                            handleError(response.Message);
                        }
                    }, function (response, status) {
                        if (response.status == 401) {
                            securityService.authorize();
                        }
                        handleError("There was an error deleting the zone. Try it again.");
                    });
            });
        }
    }
}]);