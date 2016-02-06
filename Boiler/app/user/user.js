(function() {
    'use strict';

    var app = angular.module('app');

    app.factory('UserData', function ($http, ResourceGenerator) {
        return ResourceGenerator.GetResource('/api/user/:id');
    });

    app.controller('userController', function($scope, $timeout, UserData, Notification) {
        $scope.u = {};

        var get_all = function () {
            UserData.query().then(function (result) {
                $scope.users = result;
            });
        };

        $scope.is_editing = function (u) {
            return $scope.u.id === u.id;
        };

        var select_last_input = function () {
            $timeout(function () {
                var inputs = angular.element(document).find('input');
                inputs[inputs.length - 4].focus();
            });
        };

        $scope.show_add_button = function() {
            return _.isEmpty($scope.u);
        };

        $scope.show_save_button = function() {
            return !$scope.show_add_button();
        }

        $scope.cancel = function () {
            if ($scope.u.id === 0) {
                $scope.users.splice($scope.users.length - 1, 1);
            }
            $scope.u = {};
            Notification.info('Changes cancelled');
        };

        $scope.add = function () {
            $scope.u = { id:0, name: '', username: '', email: '', isAdmin: false }
            $scope.users.push($scope.u);
            
            select_last_input();
        };

        $scope.delete = function (u) {
            if (confirm('Are you sure you want to delete user ' + u.username + '?')) {
                UserData.del(u).then(function (result) {
                    get_all();
                    Notification.success('User deleted');
                }, handle_error);
            }
        };

        var handle_error = function(result) {
            if (result.data && result.data.responseStatus) {
                Notification.error(result.data.responseStatus.message);
            } else {
                Notification.error(result);
            }
        };

        $scope.save = function (u) {
            var api = u.id ? UserData.update : UserData.insert;
            api(u).then(function (result) {
                get_all();
                $scope.u = {};
                Notification.success('User saved');
            }, handle_error);
        };

        $scope.edit = function (u) {
            $scope.u = u;
        };

        get_all();
    });
})();