(function () {
    'use strict';

    var app = angular.module('app', ['ngRoute', 'ngResource', 'ngMaterial']);

    app.config(function ($routeProvider, $locationProvider) {
        var routes = [
            { url: '/', config: { templateUrl: '/app/index.tmpl.html', controller: 'indexController' } },
            { url: '/todo', config: { templateUrl: '/app/todo/todo.tmpl.html', controller: 'todoController' } },
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

    app.controller('indexController', function ($scope, $http ) {

        $http.get('/api/hello').then(function (result) {
            $scope.hello = result.data.result;
        }, function(err) {
            console.log(err);
        });

        $http.get('/api/hello/bob').then(function(result) {
            $scope.better = result.data;
        }, function(err) {
            console.log(err);
        });

    });
})();