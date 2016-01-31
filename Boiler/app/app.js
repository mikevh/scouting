(function() {
    'use strict';

    var app = angular.module('app', ['ngRoute', 'ngResource', 'ngMaterial']);

    app.config(function ($routeProvider, $locationProvider) {
        var routes = [
            { url: '/', config: { templateUrl: '/app/index.tmpl.html', controller: 'indexController' } },
            { url: '/bar', config: { templateUrl: '/app/bar.tmpl.html', controller: 'barController' } },
        ];
        _.each(routes, function (x) { $routeProvider.when(x.url, x.config); });
        $routeProvider.otherwise({ redirectTo: '/' });
        $locationProvider.html5Mode(true);
    });

    app.controller('indexController', function($scope) {
        console.log('index');
    });
    app.controller('barController', function ($scope) {
        console.log('bindex');
    });

})();