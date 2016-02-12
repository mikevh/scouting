(function() {
    'use strict';
    
    var app = angular.module('app');

    app.factory('PlayerData', function($resource) {
        return $resource('/api/player/:id', null, {
            scoresForPlayer: { url: '/api/player/scoresForPlayer' }
        });
    });

    app.controller('scoutController', function ($scope, Notification, PlayerData) {

        $scope.p = {};
        $scope.selectPlayer = 0;

        var handle_error = function (result) {
            if (result.data && result.data.responseStatus) {
                Notification.error(result.data.responseStatus.message);
            } else {
                var msg = "HTTP " + result.status + ": " + result.statusText;
                Notification.error(msg);
            }
        };

        var get_all = function () {
            PlayerData.query().then(function (result) {
                $scope.players = result;
                $scope.players.unshift({ id: 0, playerName: '--Select Player--' });
            }, handle_error);
        };

        $scope.$watch('selectPlayer', function () {
            $scope.get_scores_for_player_id($scope.selectPlayer);
        });

        $scope.get_scores_for_player_id = function (id) {
            if (!id) {
                return;
            }
            
            PlayerData.scoresForPlayer({ id: id }, {}).$promise.then(function (result) {
                console.log(result);
                // todo: handle results
            }, handle_error);
        };

        get_all();
    });

})();