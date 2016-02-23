(function() {
    'use strict';

    var app = angular.module('app');

    app.controller('playerController', function ($scope, $timeout, PlayerData) {

        $scope.p = {};

        var get_all = function() {
            PlayerData.query().then(function (result) {
                $scope.players = result.data;
            });
        };

        $scope.is_editing = function(p) {
            return $scope.p.id === p.id;
        };

        var select_last_input = function() {
            $timeout(function() {
                var inputs = angular.element(document).find('input');
                inputs[inputs.length - 3].focus();
            });
        };

        $scope.add = function() {
            $scope.players.push({ playerNumber: '', playerName: '', leagueAge: '' });
            select_last_input();
        };

        $scope.delete = function(p) {
            PlayerData.del(p).then(function () {
                get_all();
            });
        };

        $scope.save = function (p, event) {
            if (event && event.keyCode !== 13) {
                return;
            }
            var api = p.id ? PlayerData.update : PlayerData.insert;
            api(p).then(function(result) {
                get_all();
                $scope.p = {};
                if (event) {
                    event.target.blur();
                }
            });
        };

        $scope.edit = function(p) {
            $scope.p = p;
        };

        get_all();
    });
})();