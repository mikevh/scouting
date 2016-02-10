(function() {
    'use strict';

    var app = angular.module('app');

    app.controller('indexController', function ($scope, $location) {

        $scope.is_active = function(route) {
            return $location.path() === route;
        };

        $scope.go = function (dest) {
            $location.path(dest);
        };
    });

})();