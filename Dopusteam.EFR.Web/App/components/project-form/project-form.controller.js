(function() {
    angular.module('app')
        .controller('projectFormController',
        [
            'projectsService',
            '$state',
            projectFormController
        ]);

    function projectFormController(projectsService, $state) {
        var vm = this;

        vm.name = null;
        vm.id = parseInt($state.params.projectId);

        vm.save = save;
        vm.back = back;

        activate();

        function activate() {
            if (!isNaN(vm.id) && vm.id !== 0) {
                projectsService.get(vm.id)
                    .then(function(data) {
                        vm.name = data.project.Name;
                        vm.id = data.project.Id;
                    });
            }
        }

        function save() {
            var project = {
                id: vm.id,
                name: vm.name
            };

            if (vm.id && !isNaN(vm.id)) {
                projectsService
                    .update(project)
                    .then(function() {
                        $state.go('projects');
                    });
            } else {
                projectsService
                    .create(project)
                    .then(function () {
                        $state.go('projects');
                    });
            }
        }

        function back() {
            $state.go('projects');
        }
    }
})();