(function() {
    'use strict';
    
    var app = angular.module('app');

    app.controller('scoutController', function ($scope, Notification, PlayerData) {

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

        $scope.players = [];

        var get_all = function () {
            PlayerData.query().then(function (result) {
                $scope.players = result.data;
                //$scope.players.unshift({ id: 0, playerName: '--Select Player--' });
            }, handle_error);
        };

        var flatten_scores = function(scores) {

            var metrics = ['fielding', 'hitting', 'pitching'];

            var flat = [];

            _.each(metrics, function(metric) {
                _.each(scores[metric], function(score) {
                    var score_moment = moment(score.createdDate);
                    score.from_now = score_moment.fromNow();
                    score.metric = metric;

                    flat.push(score);
                });
            });

            $scope.scores = _.sortBy(flat, function (s) { return s.createdDate; }).reverse();
        };

        $scope.$watch('selectPlayer', function () {
            $scope.get_scores_for_selectPlayer();
        });

        $scope.get_scores_for_selectPlayer = function () {
            if (!$scope.selectPlayer) {
                return;
            }
            PlayerData.scoresForPlayer($scope.selectPlayer).then(function (result) {
                
                flatten_scores(result.data)

                // flatten the fie

                //$scope.scores = result.data;
            }, handle_error);
        };

        reset_metrics();
        get_all();
    });

})();