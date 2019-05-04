var app = angular.module('pagesApp', ['ngRoute'])
    .factory('settingsCMS', ['$http', function ($http) {

        var serverSettings = null;
        return {
            getData: function (callback) {
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
    .service('pageService', ['$http', 'settingsCMS', function ($http, settingsCMS) {
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
            }
        }
    }])
    .service('zoneService', ['$http', function ($http){
        return {
            
        }
    }])
    .service('featureService', ['$http', function ($http){
        return {
            
        }
    }]);

app.controller('PagesCtrl', ['$scope', '$filter', 'pageService', 'zoneService', 'featureService'
    , function($scope, $filter, pageService, zoneService, featureService){

        $scope.currentPage = $scope.rootPage = $scope.rootPagePopulated = {};
        $scope.childrenPagesPopulated = [];

        pageService.getRootPage(function (response) {
            $scope.rootPage = response.data;
            if (document.location.pathname == '/') {
                $scope.currentPage = $scope.rootPage;
            }
            console.log($scope.rootPage);
        });

        if (document.location.pathname == '/') {
            pageService.getRootPagePopulated(function (response) {
                $scope.rootPagePopulated = $filter('filter')(response.data, function(value) { return value.parent.parentId == ""})[0];
                $scope.childrenPagesPopulated = $filter('filter')(response.data, function(value) { return value.parent.parentId != ""});
                console.log(response.data);
                console.log($scope.rootPagePopulated);
                console.log($scope.childrenPagesPopulated);
            });

        } else {
            pageService.getPageByURL(document.location.pathname, function (response) {
                $scope.currentPage = response.data;
                //$scope.currentPage = $filter('filter')(response.data, function(value) { return "/" +  value.parent.url == "document.location.pathname"})[0];
            });
        }
        
}]);