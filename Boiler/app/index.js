(function() {
    'use strict';

    var app = angular.module('app');

    app.controller('indexController', function ($scope, $location, $window) {
        $scope.go = function (dest) {
            $location.path(dest);
        };
    });

})();