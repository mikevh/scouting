(function () {
    'use strict';

    var app = angular.module('app');

    app.factory('FieldingData', function ($http, ResourceGenerator) {
        return ResourceGenerator.GetResource('/api/fielding/:id');
    });

    app.controller('fieldingController', function ($scope, $timeout, FieldingData) {

        $scope.f = {
            mechanics: 3.0,
            range: 3.0,
            hands: 3.0,
            arm: 3.0,
        };

        $scope.possible = [1,2,3,4,5];

        $scope.$watch('f', function (a) {
            console.log(a);
        }, true);
               
        $scope.save = function (f, event) {
            
            if (event && event.keyCode !== 13) {
                return;
            }
            FieldingData.insert(f).then(function (result) {
                $scope.f = {};
                if (event) {
                    event.target.blur();
                }
            });
        };




    });
})();