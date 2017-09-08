(function() {
    angular.module('app').service('studentsService', ['$http', '$q',service]);

    function service($http, $q) {
        var me = {
            getAll: getAll,
            create: create,
            update: update,
            remove: remove,
            getProjects: getProjects,
            getGroups: getGroups,
            get: get
        }

        return me;

        function getAll(sortField, sortOrder, limit, showGroup, showProjects) {
            var deferred = $q.defer();

            $http(
                {
                    method: 'GET',
                    url: '/Students/All?limit=' + limit + '&sortField=' + sortField + '&sortOrder=' + sortOrder + '&showGroup=' + showGroup + '&showProjects=' + showProjects
                })
                .then(
                    function (response) {
                        deferred.resolve({ students: response.data.data });
                    },
                    function() {
                        deferred.reject();
                    });

            return deferred.promise;
        }

        function create(student) {
            var deferred = $q.defer();

            $http(
                {
                    method: 'POST',
                    url: '/Students/Create',
                    data: student
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

        function update(student) {
            var deferred = $q.defer();

            $http(
                {
                    method: 'POST',
                    url: '/Students/Update',
                    data: student
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

        function remove(studentId) {
            var deferred = $q.defer();

            $http(
                {
                    method: 'POST',
                    url: '/Students/Delete',
                    data: { studentId: studentId }
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

        function get(studentId) {
            var deferred = $q.defer();

            $http(
                {
                    method: 'GET',
                    url: '/Students/Get/?id=' + studentId
                })
                .then(
                    function (response) {
                        deferred.resolve({ student: response.data.data });
                    },
                    function () {
                        deferred.reject();
                    });

            return deferred.promise;
        }

        function getProjects() {
            var deferred = $q.defer();

            $http(
                {
                    method: 'GET',
                    url: '/Students/GetProjects/'
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

        function getGroups() {
            var deferred = $q.defer();

            $http(
                {
                    method: 'GET',
                    url: '/Students/GetGroups'
                })
                .then(
                    function (response) {
                        deferred.resolve({ groups: response.data.data });
                    },
                    function () {
                        deferred.reject();
                    });

            return deferred.promise;
        }
    }
})();