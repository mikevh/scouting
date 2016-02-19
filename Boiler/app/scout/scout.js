(function() {
    'use strict';
    
    var app = angular.module('app');

    app.factory('PlayerData', function($resource, $http) {
        return $resource('/api/player/:id', null, {
            scoresForPlayer: { url: '/api/player/scoresForPlayer' }

        });
    });

    app.controller('scoutController', function ($scope, Notification, PlayerData, $resource, $http) {

        $scope.p = {};
        $scope.selectPlayer = 0;

        var reset_metrics = function() {
            $scope.f = {
                fielding: {
                    mechanics: 0,
                    range: 0,
                    hands: 0,
                    armStrength: 0,
                    fieldingNote: ''
                },
                hitting: {
                    mechanics: 0,
                    power: 0,
                    contact: 0,
                    hittingNote: '',
                },
                pitching: {
                    mechanics: 0,
                    velocity: 0,
                    command: 0,
                    pitchingNote: ''
                }
            };
        };

        $scope.possible = _.range(1,6);

        $scope.add = function (metric) {
            $scope.f[metric].playerId = $scope.selectPlayer;
            $http.post('/api/' + metric, $scope.f[metric]).then(function (result) {
                reset_metrics();
                Notification.success('Score posted successfully');
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

        $scope.get_scores_for_player_id = function (id) {
            if (!id) {
                return;
            }
            
            PlayerData.scoresForPlayer({ id: id }, {}).$promise.then(function (result) {
                debugger;
                console.log(result);
                // todo: handle results
            }, handle_error);
        };

        reset_metrics();
        get_all();
    });

})();