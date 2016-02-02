(function () {
    'use strict';

    var app = angular.module('app', ['ngRoute', 'ngResource', 'ngMaterial']);

    app.config(function ($routeProvider, $locationProvider) {
        var routes = [
            { url: '/', config: { templateUrl: '/app/index.tmpl.html', controller: 'indexController' } },
            { url: '/bar', config: { templateUrl: '/app/bar.tmpl.html', controller: 'barController' } },
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

    app.config(function ($httpProvider) {
        $httpProvider.interceptors.push('AuthorizationRedirectInterceptor');
    });

    app.controller('indexController', function ($scope, $http ) {

        $http.get('/api/hello').then(function (result) {
            $scope.hello = result.data.result;
            console.log('empty: ' + result.data.result);
        }, function(err) {
            console.log(err);
        });

        $http.get('/api/hello/bob').then(function(result) {
            $scope.better = result.data;
            console.log('ok: ' + result.data.result);
        }, function(err) {
            console.log(err);
        });

    });
    app.controller('barController', function ($scope) {
        console.log('bindex');
    });

})();