﻿(function () {
    'use strict';

    var app = angular.module('app', ['ngRoute', 'ngResource', 'ui-notification']);

    app.config(function ($routeProvider) {
        var routes = [
            { url: '/', config: { templateUrl: '/app/index.tmpl.html', controller: 'indexController' } },
            { url: '/todo', config: { templateUrl: '/app/todo/todo.tmpl.html', controller: 'todoController' } },
            { url: '/player', config: { templateUrl: '/app/player/player.tmpl.html', controller: 'playerController' } },
            { url: '/scout', config: { templateUrl: '/app/scout/scout.tmpl.html', controller: 'scoutController' } },
            { url: '/fielding', config: { templateUrl: '/app/fielding/fielding.tmpl.html', controller: 'fieldingController' } },
            { url: '/user', config: { templateUrl: '/app/user/user.tmpl.html', controller: 'userController' } },
        ];
        _.each(routes, function (x) { $routeProvider.when(x.url, x.config); });
        $routeProvider.otherwise({ redirectTo: '/' });
    });

    app.factory('AuthorizationRedirectInterceptor', function ($q, $window) {
        var responseError = function (responseError) {
            if (responseError.status === 401 && responseError.config.url.indexOf('/api/auth') !== 0) { // unauthorized
                $window.location = "/index.html";//"?redirectUrl=" + encodeURIComponent(document.URL);
                return null;
            }
            return $q.reject(responseError);
        };

        return {
            responseError: responseError
        };
    });

    app.config(function(NotificationProvider) {
        NotificationProvider.setOptions({
            positionX: 'left'
        });
    });

    app.config(function ($httpProvider) {
        $httpProvider.interceptors.push('AuthorizationRedirectInterceptor');
    });

    app.factory('PlayerData', function ($http) {
        var query = function () {
            return $http.get('/api/player/');
        };

        var scoresForPlayer = function (id) {
            return $http.get('/api/player/scoresForPlayer/' + id);
        };

        var update = function(data) {
            return $http.put('/api/player', data);
        };

        var insert = function(data) {
            return $http.post('/api/player', data);
        }

        return {
            query: query,
            update: update,
            insert: insert,
            scoresForPlayer: scoresForPlayer
        }
    });

    app.directive('selectOnClick', ['$window', function ($window) {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                element.on('click', function () {
                    if (!$window.getSelection().toString()) {
                        // Required for mobile Safari
                        this.setSelectionRange(0, this.value.length);
                    }
                });
            }
        };
    }]);

    app.directive('showtab', function() {
        return {
            link: function(scope, element, attrs) {
                element.click(function (e) {
                    e.preventDefault();
                    $(element).tab('show');
                });
            }
        };
    });
})();