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
        vm.groups = [];

        vm.groupId = null;

        vm.save = save;
        vm.back = back;

        activate();

        function activate() {
            if (!isNaN(vm.id) && vm.id !== 0) {
                studentsService.get(vm.id)
                    .then(function(data) {
                        vm.name = data.student.Name;
                        vm.lastName = data.student.LastName;
                        vm.group = data.student.Group;
                        vm.projects = data.student.Projects;
                        vm.groups = data.student.Groups.map(function(group) {
                            return {
                                Number: group.Number,
                                GroupId: group.GroupId.toString(),
                                IsEnrolled: group.IsEnrolled
                            }
                        });

                        var enrolledGroup = vm.groups.find(function(group) {
                            return group.IsEnrolled;
                        });

                        if (enrolledGroup) {
                            vm.groupId = enrolledGroup.GroupId;
                        }
                    });
            } else {
                getProjects();
                getGroups();
            }
        }

        function save() {
            var student = {
                id: vm.id,
                name: vm.name,
                lastName: vm.lastName,
                projectIds: vm.projects.filter(function(project) {
                    return project.IsAssigned;
                }).map(function(project) {
                    return project.ProjectId
                }),
                groupId: vm.groupId
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

        function getProjects() {
            studentsService
                    .getProjects()
                    .then(function (data) {
                        vm.projects = data.projects;
                    });
        }

        function getGroups() {
            studentsService
                    .getGroups()
                    .then(function (data) {
                        vm.groups = data.groups;
                    });
        }

        function back() {
            $state.go('students');
        }
    }
})();