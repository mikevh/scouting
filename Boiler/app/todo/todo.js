(function() {
    'use strict';

    var app = angular.module('app');

    app.factory('TodoData', function ($http, ResourceGenerator) {
		return ResourceGenerator.GetResource('/api/todo/:id');
	});

    app.controller('todoController', function($scope, $timeout, TodoData) {

        $scope.t = {};

        var get_todos = function() {
            TodoData.query().then(function(result) {
                $scope.todos = result;
            });
        };

        $scope.is_editing = function(t) {
            return $scope.t.id === t.id;
        };

        var select_last_input = function() {
            $timeout(function() {
                var inputs = angular.element(document).find('input');
                inputs[inputs.length - 1].focus();
            });
        };

        $scope.add = function() {
            $scope.todos.push({ name: '', isDone: false });
            select_last_input();
        };

        $scope.delete = function(t) {
            TodoData.del(t).then(function(result) {
                get_todos();
            });
        };

        $scope.save = function (t, event) {
            if (event && event.keyCode !== 13) {
                return;
            }
            var api = t.id ? TodoData.update : TodoData.insert;
            api(t).then(function(result) {
                get_todos();
                $scope.t = {};
                if (event) {
                    event.target.blur();
                }
            });
        };

        $scope.edit = function(t) {
            $scope.t = t;
        };

        get_todos();
    });
})();