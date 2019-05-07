app.controller('ZonesCtrl', ['$scope', '$filter', '$element', '$animate', 'zoneService', 'securityService'
    , function($scope, $filter, $element, $animate, zoneService, securityService) {

    $scope.newZone = { type: 'CENTRE' };
    
    $scope.addZone = function () {
        var newZone = { id: "", name: $scope.newZone.name, type: $scope.newZone.type, pageId: $scope.currentPage.id };
        if (securityService.isAuthorize()) {
            zoneService.addZone(newZone, function(zone){
                $scope.currentPagePopulated.zones.push(zone);
                $scope.newZone = { type: 'CENTRE' };
            });
        } else {
            securityService.authorize();
        }
    }

    $scope.updateZone = function (zone) {
        if (securityService.isAuthorize()) {
            zoneService.updateZone(zone, function(zoneUpdated){
                $scope.editingMode(zoneUpdated, false);
            });
        } else {
            securityService.authorize();
        }
    }

    $scope.deleteZone = function(zone, index) {
        if (securityService.isAuthorize()) {
            zoneService.deleteZone(zone, function(data){
                if(data == 'OK') {
                    $scope.currentPagePopulated.zones.splice(index, 1);
                }
            });
        } else {
            securityService.authorize();
        }
    }
}]);
