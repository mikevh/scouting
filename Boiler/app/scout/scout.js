(function () {
    'use strict';

    var app = angular.module('app');

    app.factory('PlayerData', function ($http, ResourceGenerator) {
        return ResourceGenerator.GetResource('/api/player/:id');
    });

    app.controller('scoutController', function ($scope, $timeout, PlayerData) {

        $scope.p = {};

        var get_all = function () {
            PlayerData.query().then(function (result) {
                $scope.players = result;
            });
        };

        get_all();
    });

    var tab = angular.module('tabApp', []);
    tab.controller('TabController', ['$scope', function ($scope) {
      $scope.tab = 1;

      $scope.setTab = function (newTab) {
          $scope.tab = newTab;
      };

      $scope.isSet = function (tabNum) {
          return $scope.tab === tabNum;
      };
  }]);

})();