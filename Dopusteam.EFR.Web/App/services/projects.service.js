(function() {
    angular.module('app').service('projectsService',
            ['$http', '$q', projectsService]);

    function projectsService($http, $q) {
        var service = {
            getAll: getAll,
            create: create,
            update: update,
            remove: remove,
            get: get
        }

        return service;

        function getAll() {
            var deferred = $q.defer();

            $http(
                {
                    method: 'GET',
                    url: '/Projects/All'
                })
                .then(
                    function (response) {
                        deferred.resolve({ projects: response.data.data });
                    },
                    function () {
                        deferred.reject();
                    });

            return deferred.promise;
        }

        function create(project) {
            var deferred = $q.defer();

            $http(
                {
                    method: 'POST',
                    url: '/Projects/Create',
                    data: project
                })
                .then(
                    function () {
                        deferred.resolve();
                    },
                    function () {
                        deferred.reject();
                    });

            return deferred.promise;
        }

        function update(project) {
            var deferred = $q.defer();

            $http(
                {
                    method: 'POST',
                    url: '/Projects/Update',
                    data: project
                })
                .then(
                    function () {
                        deferred.resolve();
                    },
                    function () {
                        deferred.reject();
                    });

            return deferred.promise;
        }

        function remove(projectId) {
            var deferred = $q.defer();

            $http(
                {
                    method: 'POST',
                    url: '/Projects/Delete',
                    data: { projectId: projectId }
                })
                .then(
                    function () {
                        deferred.resolve();
                    },
                    function () {
                        deferred.reject();
                    });

            return deferred.promise;
        }

        function get(projectId) {
            var deferred = $q.defer();

            $http(
                {
                    method: 'GET',
                    url: '/Projects/Get/?id=' + projectId
                })
                .then(
                    function (response) {
                        deferred.resolve({ project: response.data.data });
                    },
                    function () {
                        deferred.reject();
                    });

            return deferred.promise;
        }
    }
})();