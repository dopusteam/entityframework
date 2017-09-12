(function() {
    angular.module('app').controller('projectsController', [
        'projectsService',
        projectsController]);

    function projectsController(projectsService) {
        var vm = this;

        vm.projects = [];

        vm.createProject = createProject;
        vm.removeProject = removeProject;

        activate();

        function activate() {
            getProjects();
        }

        function createProject() {
            
        }

        function removeProject(projectId) {
            projectsService
                .remove(projectId)
                .then(getProjects);
        }

        function getProjects() {
            projectsService
                .getAll()
                .then(function(data) {
                    vm.projects = data.projects;
                });
        }
    }
})();