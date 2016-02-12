(function() {
    'use strict';
    
    var app = angular.module('app');

    app.factory('PlayerData', function($resource) {
        return $resource('/api/player/:id', null, {
            scoresForPlayer: { url: '/api/player/scoresForPlayer' },
            postScore: { url: '/api/player/addscore', method: 'POST'}
        });
    });

    app.controller('scoutController', function ($scope, Notification, PlayerData) {

        $scope.p = {};
        $scope.selectPlayer = 0;

        $scope.f = {
            fielding: {
                mechanics: 1,
                range: 1,
                hands: 1,
                armStrength: 1
            },
            hitting: {
                mechanics: 1,
                power: 1,
                contact: 1
            },
            pitching: {
                mechanics: 1,
                velocity: 1,
                command: 1
            }
        };

        $scope.possible = [1, 2, 3, 4, 5];

        $scope.add = function(metric) {

            var data_to_send = {
                data: $scope.f[metric],
                playerId: $scope.selectPlayer
            };

            PlayerData.postScore(data_to_send).$promise.then(function (result) {

            }, handle_error);
        };

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