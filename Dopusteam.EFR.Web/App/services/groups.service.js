(function() {
    angular.module('app').service('groupsService',
            ['$http', '$q', groupsService]);

    function groupsService($http, $q) {
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
                    url: '/Groups/All'
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

        function create(group) {
            var deferred = $q.defer();

            $http(
                {
                    method: 'POST',
                    url: '/Groups/Create',
                    data: group
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

        function update(group) {
            var deferred = $q.defer();

            $http(
                {
                    method: 'POST',
                    url: '/Groups/Update',
                    data: group
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

        function remove(groupId) {
            var deferred = $q.defer();

            $http(
                {
                    method: 'POST',
                    url: '/Groups/Delete',
                    data: { groupId: groupId }
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

        function get(groupId) {
            var deferred = $q.defer();

            $http(
                {
                    method: 'GET',
                    url: '/Groups/Get/?id=' + groupId
                })
                .then(
                    function (response) {
                        deferred.resolve({ group: response.data.data });
                    },
                    function () {
                        deferred.reject();
                    });

            return deferred.promise;
        }
    }
})();