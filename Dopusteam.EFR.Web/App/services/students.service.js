(function() {
    angular.module('app').service('studentsService', ['$http', '$q',service]);

    function service($http, $q) {
        var me = {
            getAll: getAll,
            create: create,
            remove: remove
        }

        return me;

        function getAll(sortField, sortOrder, limit) {
            var deferred = $q.defer();

            $http(
                {
                    method: 'GET',
                    url: '/Students/All?limit=' + limit + '&sortField=' + sortField + '&sortOrder=' + sortOrder
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
    }
})();