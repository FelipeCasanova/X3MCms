app.controller('AppCtrl', ['$scope', '$filter', '$element', '$animate', 'pageService', 'zoneService', 'featureService', 'securityService'
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
                $scope.currentPagePopulated = $filter('filter')(data, function(value) { return value.parent.parentId == ""})[0];
                $scope.childrenPagesPopulated = $filter('filter')(data, function(value) { return value.parent.parentId != ""});
                console.log(data);
                console.log("currentPagePopulated: " + $scope.currentPagePopulated);
                console.log("childrenPagesPopulated: " + $scope.childrenPagesPopulated);
            });

        } else {
            pageService.getPagePopulatedByURL(document.location.pathname, function (data) {
                $scope.currentPagePopulated = $filter('filter')(data, function(value) { return "/" +  value.parent.url == document.location.pathname })[0];
                $scope.currentPage = $scope.currentPagePopulated.parent;
                $scope.z = $filter('filter')(data, function(value) { return "/" +  value.parent.url == document.location.pathname })[0].parent;
                console.log(data);
                console.log("currentPagePopulated: " + $scope.currentPagePopulated);
                console.log("currentPage: " + $scope.currentPage);
                console.log("childrenPagesPopulated: " + $scope.childrenPagesPopulated);
            });

            pageService.getBreadCrumbToTheRootByURL(document.location.pathname, function (data) {
                $scope.breadCrumb = $filter('orderBy')(data, null, true);

            });
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

