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
            })
            .state('studentForm',
            {
                url: '/studentForm?studentId',
                templateUrl: '/App/components/student-form/student-form.html'
            })
            .state('groups',
            {
                url: '/groups',
                templateUrl: '/App/components/groups/groups.html'
            })
            .state('groupForm',
            {
                url: '/groupForm?groupId',
                templateUrl: '/App/components/group-form/group-form.html'
            })
            .state('projects',
            {
                url: '/projects',
                templateUrl: '/App/components/projects/projects.html'
            })
            .state('projectForm',
            {
                url: '/projectForm?projectId',
                templateUrl: '/App/components/project-form/project-form.html'
            });
    }
})();