app.factory('settingsCMS', ['$http', function ($http) {

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
}]);