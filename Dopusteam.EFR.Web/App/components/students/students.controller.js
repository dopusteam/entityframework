(function() {
    angular.module('app').controller('studentsController', ['studentsService', controller]);

    function controller(studentsService) {
        var vm = this;

        vm.students = [];

        vm.newStudent = {
            name: null,
            lastName: null
        }

        vm.showProjects = false;
        vm.showGroup = false;

        vm.sortField = 1;
        vm.sortOrder = 1;
        vm.limit = 2;

        vm.addStudent = addStudent;
        vm.getStudents = getStudents;
        vm.removeStudent = removeStudent;

        activate();

        function activate() {
            getStudents();
        }

        function addStudent() {
            studentsService
                .create(vm.newStudent)
                .then(function() {
                    getStudents();
                    vm.newStudent = {
                        name: null,
                        lastName: null
                    }
                });
        }

        function removeStudent(studentId) {
            studentsService
                .remove(studentId)
                .then(function () {
                    getStudents();
                });
        }

        function getStudents() {
            studentsService
                .getAll(vm.sortField, vm.sortOrder, vm.limit, vm.showGroup, vm.showProjects)
                .then(function (data) {
                    vm.students = data.students;
                });
        }
    }
})();