var app = angular.module('pagesApp', ['ngRoute', 'ngAnimate'])
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

app.controller('PagesCtrl', ['$scope', '$filter', '$element', '$animate', 'pageService', 'zoneService', 'featureService'
    , function($scope, $filter, $element, $animate, pageService, zoneService, featureService){

        $scope.currentPage = $scope.currentPagePopulated = $scope.rootPage = {};
        $scope.childrenPagesPopulated = [];
        $scope.breadCrumb = [];

        $scope.newPage = { };
        
        $scope.patterURL = {
            word: /^\s*\w*\s*$/
          };

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

        $scope.addNewPage = function () {
            $scope.currentPagePopulated.children.push( { id: "", name: $scope.newPage.name, url: $scope.newPage.url, parentId: $scope.currentPage.id });
            $scope.newPage = { };
        }

        $scope.deletePage = function(page) {
            $scope.currentPagePopulated.children.pop(page);
        }
}]);