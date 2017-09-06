(function () {
    'use strict';

    var app = angular.module('app', [
        'ui.router'
    ]);

    app.config(['$stateProvider', '$urlRouterProvider', '$urlMatcherFactoryProvider', config]);

    function config($stateProvider, $urlRouterProvider, $urlMatcherFactoryProvider) {
        $urlMatcherFactoryProvider.strictMode(false);

        $urlRouterProvider.otherwise('/');

        $stateProvider
            .state('students',
            {
                url: '/',
                templateUrl: '/App/components/students/students.html'
            });
    }
})();