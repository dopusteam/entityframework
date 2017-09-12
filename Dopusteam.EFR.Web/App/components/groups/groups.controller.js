(function() {
    angular.module('app').controller('groupsController', [
        'groupsService',
        groupsController]);

    function groupsController(groupsService) {
        var vm = this;

        vm.groups = [];

        vm.createGroup = createGroup;
        vm.removeGroup = removeGroup;

        activate();

        function activate() {
            getGroups();
        }

        function createGroup() {
            
        }

        function removeGroup(groupId) {
            groupsService
                .remove(groupId)
                .then(getGroups);
        }

        function getGroups() {
            groupsService
                .getAll()
                .then(function(data) {
                    vm.groups = data.groups;
                });
        }
    }
})();