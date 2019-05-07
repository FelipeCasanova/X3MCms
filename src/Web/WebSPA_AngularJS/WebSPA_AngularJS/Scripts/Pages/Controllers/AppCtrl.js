app.controller('AppCtrl', ['$scope', '$filter', '$element', '$animate', 'pageService', 'zoneService', 'featureService', 'securityService'
    , function($scope, $filter, $element, $animate, pageService, zoneService, featureService, securityService){

        $scope.currentPage = $scope.currentPagePopulated = $scope.rootPage = {};
        $scope.childrenPagesPopulated = [];
        $scope.breadCrumb = [];
        
        $scope.patterURL = {
            word: /^\s*\w*\s*$/
          };
        
        if (window.location.hash) {
            securityService.authorizedCallback();
        }
        
        $scope.isAuthorize = securityService.isAuthorize();
        if ($scope.isAuthorize) {
            securityService.setHeaders();
        }

        pageService.getRootPage(function (data) {
            $scope.rootPage = data;
            if (document.location.pathname == '/') {
                $scope.currentPage = $scope.rootPage;
            }
            console.log($scope.rootPage);
        });

        if (document.location.pathname == '/') {
            pageService.getRootPagePopulated(function (data) {
                $scope.currentPagePopulated = $filter('filter')(data, function(value) { return value.page.parentId == ""})[0];
                $scope.childrenPagesPopulated = $filter('filter')(data, function(value) { return value.page.parentId != ""});
                console.log(data);
                console.log("currentPagePopulated: " + $scope.currentPagePopulated);
                console.log("childrenPagesPopulated: " + $scope.childrenPagesPopulated);
            });

        } else {
            pageService.getPagePopulatedByURL(document.location.pathname, function (data) {
                $scope.currentPagePopulated = $filter('filter')(data, function(value) { return "/" +  value.page.url == document.location.pathname })[0];
                $scope.currentPage = $scope.currentPagePopulated.page;
                $scope.z = $filter('filter')(data, function(value) { return "/" +  value.page.url == document.location.pathname })[0].page;
                console.log(data);
                console.log("currentPagePopulated: " + $scope.currentPagePopulated);
                console.log("currentPage: " + $scope.currentPage);
                console.log("childrenPagesPopulated: " + $scope.childrenPagesPopulated);
            });

            pageService.getBreadCrumbToTheRootByURL(document.location.pathname, function (data) {
                $scope.breadCrumb = $filter('orderBy')(data, null, true);

            });
        }

        $scope.editingMode = function (item, isEditing) {
            if(isEditing){
                var itemCopy = {};
                for (key in item) {
                    itemCopy[key] = item[key];
                }
                item._copy = itemCopy;
            } else if (item._copy) {
                for (key in item._copy) {
                    item[key] = item._copy[key]; 
                }
                delete item._copy; 
            }
            item.isEditing = isEditing;
        }

        $scope.authorize = function () {
            securityService.authorize();
        }

        $scope.logoff = function () {
            securityService.logoff();
        }
}]);

