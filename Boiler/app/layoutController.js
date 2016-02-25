(function() {
    'use strict';

    var app = angular.module('app');

    app.controller('layoutController', function ($scope, $location, Notification, $http) {
        
        $scope.is_active = function(route) {
            return $location.path() === route;
        };

        $scope.go = function (dest) {
            $location.path(dest);
        };

        var login_success = function () {
            $scope.logged_in = true;
        };

        var login_error = function (data) {
            console.log(data);
            Notification.error(data.statusText);
        };

        $scope.login = {
            user_name: 'admin@admin.com',
            password: 'password'
        };

        $http.get('/api/auth').then(function () {
            $scope.logged_in = true;
        }, function() {
            $scope.logged_in = false;
        });

        $scope.submit_logout = function() {
            $http.post('/api/auth/logout');
            $scope.logged_in = false;
            Notification.info('Logged out');
            if ($location.path() !== "/") {
                $location.path("/");
            }
        };

        $scope.submit_login = function() {
            $http.post('/api/auth/credentials',
                {
                    userName: $scope.login.user_name,
                    password: $scope.login.password,
                    rememberMe: true
                }).then(login_success, login_error);
        };
    });

})();