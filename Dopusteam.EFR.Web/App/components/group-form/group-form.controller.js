(function() {
    angular.module('app')
        .controller('groupFormController',
        [
            'groupsService',
            '$state',
            groupFormController
        ]);

    function groupFormController(groupsService, $state) {
        var vm = this;

        vm.number = null;
        vm.id = parseInt($state.params.groupId);

        vm.save = save;
        vm.back = back;

        activate();

        function activate() {
            if (!isNaN(vm.id) && vm.id !== 0) {
                groupsService.get(vm.id)
                    .then(function(data) {
                        vm.number = data.group.Number;
                        vm.id = data.group.Id;
                    });
            }
        }

        function save() {
            var group = {
                id: vm.id,
                number: vm.number
            };

            if (vm.id && !isNaN(vm.id)) {
                groupsService
                    .update(group)
                    .then(function() {
                        $state.go('groups');
                    });
            } else {
                groupsService
                    .create(group)
                    .then(function () {
                        $state.go('groups');
                    });
            }
        }

        function back() {
            $state.go('groups');
        }
    }
})();