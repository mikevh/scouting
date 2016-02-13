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

        $scope.f = {
            fielding: {
                mechanics: 0,
                range: 0,
                hands: 0,
                armStrength: 0,
                fieldingNote: ''
            },
            hitting: {
                mechanics: 1,
                power: 1,
                contact: 1,
                hittingNote: '',
            },
            pitching: {
                mechanics: 1,
                velocity: 1,
                command: 1,
                pitchingNote: ''
            }
        };

        $scope.possible = _.range(1,6);

        var postFielding = function() {
            $scope.f.fielding.playerId = $scope.selectPlayer;
            $http.post('/api/fielding', $scope.f.fielding).then(function (result) {
                debugger;
            }, handle_error);
        };

        $scope.add = function(metric) {
            if (metric === 'fielding') {
                postFielding();
            }
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

        //$scope.$watch('selectPlayer', function () {
        //    $scope.get_scores_for_player_id($scope.selectPlayer);
        //});

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

        get_all();
    });

})();