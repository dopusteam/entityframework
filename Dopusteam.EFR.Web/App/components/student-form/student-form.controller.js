(function() {
    angular.module('app')
        .controller('studentFormController',
        [
            'studentsService',
            '$state',
            studentFormController
        ]);

    function studentFormController(studentsService, $state) {
        var vm = this;

        vm.name = null;
        vm.lastName = null;
        vm.id = parseInt($state.params.studentId);
        vm.projects = [];
        vm.group = null;

        vm.save = save;
        vm.back = back

        activate();

        function activate() {
            if (!isNaN(vm.id) && vm.id !== 0) {
                studentsService.get(vm.id)
                    .then(function (data) {
                        vm.name = data.student.Name;
                        vm.lastName = data.student.LastName;
                        vm.group = data.student.Group;
                        vm.projects = data.student.Projects;
                    });
            }
        }

        function save() {
            var student = {
                id: vm.id,
                name: vm.name,
                lastName: vm.lastName
            };

            if (vm.id && !isNaN(vm.id)) {
                studentsService
                    .update(student)
                    .then(function() {
                        $state.go('students');
                    });
            } else {
                studentsService
                    .create(student)
                    .then(function () {
                        $state.go('students');
                    });
            }
        }

        function back() {
            $state.go('students');
        }
    }
})();