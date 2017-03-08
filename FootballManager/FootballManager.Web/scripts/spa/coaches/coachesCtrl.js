(function (app) {
    'use strict';

    app.controller('coachesCtrl', coachesCtrl);

    coachesCtrl.$inject = ['$scope', '$location', '$routeParams', 'apiService', 'notificationService'];

    function coachesCtrl($scope, $location, $routeParams, apiService, notificationService) {
        $scope.pageClass = 'page-home';

        $scope.coaches = [];
        $scope.selectedCoach = {};
        $scope.coach = null;
        $scope.getCoachData = getCoachData;

        function loadFailed(response) {
            notificationService.displayError(response.data);
        }

        function getCoachData(selected) {
            if (selected == undefined)
                return;
            $location.path('/coaches/' + selected.originalObject.Id);
        }

        function coachDataLoadCompleted(result) {
            $scope.coach = result.data;
        }

        if ($routeParams.id != undefined) {
            apiService.get('api/coaches/' + $routeParams.id, null, coachDataLoadCompleted, loadFailed);
        }
    }

})(angular.module('homeFootball'));