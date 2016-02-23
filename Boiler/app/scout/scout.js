(function() {
    'use strict';
    
    var app = angular.module('app');

    app.controller('scoutController', function ($scope, $http, Notification, PlayerData) {

        $scope.selectPlayer = 0;

        var reset_metrics = function() {
            $scope.f = {
                fielding: { mechanics: 0, range: 0, hands: 0, armStrength: 0, fieldingNote: '' },
                hitting: { mechanics: 0, power: 0, contact: 0, hittingNote: '' },
                pitching: { mechanics: 0, velocity: 0, command: 0, pitchingNote: '' }
            };
        };

        $scope.possible = _.range(1,6);

        $scope.add = function (metric) {
            $scope.f[metric].playerId = $scope.selectPlayer;
            $http.post('/api/' + metric, $scope.f[metric]).then(function (result) {
                reset_metrics();
                $scope.get_scores_for_selectPlayer();
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
                flatten_scores(result.data);
            }, handle_error);
        };

        reset_metrics();
        get_all();
    });

    app.directive('bsDropdown', function ($compile) {
        return {
            scope: {
                items: '=dropdownData',
                doSelect: '&select',
                selectedItem: '=preselectedItem'
            },
            link: function (scope, element, attrs) {
                var html = '<div class="btn-group" >' +
                    '<button id="label" class="btn btn-label btn-info">Select Player</button>' +
                    '<button class="btn btn-info dropdown-toggle" data-toggle="dropdown">' +
                    '<span class="caret"></span></button>'

                + '<ul class="dropdown-menu">' +
                    '<li ng-repeat="i in items"><a data-ng-click="selectVal(i)">{{i.playerNumber}}  - {{i.playerName}}</a></li>' +
                    '</ul>' +
                    '</div>';

                element.append($compile(html)(scope));

                for (var i = 0; i < scope.items.length; i++) {
                    if (scope.items[i].id === scope.selectedItem) {
                        scope.bSelectedItem = scope.items[i];
                        break;
                    }
                }

                scope.selectVal = function (item) {
                    $('button#label', element).html(item.playerNumber + '-' + item.playerName);
                    scope.doSelect({ selectedVal: item.id });
                };
            }
        };
    });
})();