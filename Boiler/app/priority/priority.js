(function() {
    'use strict';

    var app = angular.module('app');

    app.factory('PriorityData', function ($http, ResourceGenerator) {
		return ResourceGenerator.GetResource('/api/priority/:id');
	});

    app.controller('priorityController', function ($scope, $timeout, PriorityData) {

        $scope.p = {};

        var get_all = function() {
            PriorityData.query().then(function(result) {
                $scope.priorities = result;
            });
        };

        $scope.is_editing = function(p) {
            return $scope.p.id === p.id;
        };

        var select_last_input = function() {
            $timeout(function() {
                var inputs = angular.element(document).find('input');
                inputs[inputs.length - 1].focus();
            });
        };

        $scope.add = function() {
            $scope.priorities.push({ name: '', ordinal: 1 });
            select_last_input();
        };

        $scope.delete = function(p) {
            PriorityData.del(p).then(function (result) {
                get_all();
            });
        };

        $scope.save = function (p, event) {
            if (event && event.keyCode !== 13) {
                return;
            }
            var api = p.id ? PriorityData.update : PriorityData.insert;
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