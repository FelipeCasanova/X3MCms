app.controller('PagesCtrl', ['$scope', '$filter', '$element', '$animate', 'pageService', 'zoneService', 'featureService', 'securityService'
    , function($scope, $filter, $element, $animate, pageService, zoneService, featureService, securityService){

    $scope.newPage = { };

    $scope.addPage = function () {
        var newPage = { id: "", name: $scope.newPage.name, url: $scope.newPage.url, parentId: $scope.currentPage.id };
        if (securityService.isAuthorize()) {
            pageService.addPage(newPage, function(page){
                $scope.currentPagePopulated.children.push(page);
                $scope.newPage = { };
            });
        } else {
            securityService.authorize();
        }
    }

    $scope.updatePage = function (page) {
        if (securityService.isAuthorize()) {
            pageService.updatePage(page, function(pageUpdated){
                $scope.editingMode(pageUpdated, false);
            });
        } else {
            securityService.authorize();
        }
    }

    $scope.deletePage = function(page, index) {
        if (securityService.isAuthorize()) {
            pageService.deletePage(page, function() {
                $scope.currentPagePopulated.children.splice(index, 1);
            });
        } else {
            securityService.authorize();
        }
    }
}]);
