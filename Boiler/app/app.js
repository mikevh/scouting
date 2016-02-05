(function () {
    'use strict';

    var app = angular.module('app', ['ngRoute', 'ngResource']);

    app.config(function ($routeProvider, $locationProvider) {
        var routes = [
            { url: '/', config: { templateUrl: '/app/index.tmpl.html', controller: 'indexController' } },
            { url: '/todo', config: { templateUrl: '/app/todo/todo.tmpl.html', controller: 'todoController' } },
            { url: '/player', config: { templateUrl: '/app/player/player.tmpl.html', controller: 'playerController' } },
            { url: '/scout', config: { templateUrl: '/app/scout/scout.tmpl.html', controller: 'scoutController' } },
            { url: '/fielding', config: { templateUrl: '/app/fielding/fielding.tmpl.html', controller: 'fieldingController' } },
        ];
        _.each(routes, function (x) { $routeProvider.when(x.url, x.config); });
        $routeProvider.otherwise({ redirectTo: '/' });
        //$locationProvider.html5Mode(true);
    });

    app.factory('AuthorizationRedirectInterceptor', function ($q, $window) {
        var responseError = function (responseError) {
            if (responseError.status === 401) { // unauthorized
                $window.location = "/login.html";//"?redirectUrl=" + encodeURIComponent(document.URL);
                return null;
            }
            return $q.reject(responseError);
        };

        return {
            responseError: responseError
        };
    });

    app.factory('ResourceGenerator', function ($resource) {
        var get_resource = function (url) {
            var res = $resource(url, null, {
                'update': { method: 'PUT' },
                'insert': { method: 'POST' },
            });
            return {
                query: function () {
                    return res.query().$promise;
                },
                get: function (id) {
                    return res.get({ id: id }).$promise;
                },
                insert: function (data) {
                    return res.insert(data).$promise;
                },
                del: function (data) {
                    return res.delete(data).$promise;
                },
                update: function (data) {
                    return res.update(data).$promise;
                }
            };
        };
        return {
            GetResource: get_resource
        };
    });

    app.config(function ($httpProvider) {
        $httpProvider.interceptors.push('AuthorizationRedirectInterceptor');
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

})();