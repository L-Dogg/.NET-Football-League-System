(function (app) {
    'use strict';

    app.controller('teamCtrl', teamCtrl);

    teamCtrl.$inject = ['$scope', '$location', '$routeParams', 'apiService', 'notificationService'];

    function teamCtrl($scope, $location, $routeParams, apiService, notificationService) {
        $scope.pageClass = 'page-home';
        $scope.isReadOnly = true;
        $scope.team = null;
        $scope.selectedTeam = function (selectedTeam) {
            if (selectedTeam == undefined) return;
            $location.path('/team/' + selectedTeam.originalObject.Id);
        }

        $scope.redirectToPlayer = function (player) {
            $location.path('/player/' + player.Id);
        }

        $scope.redirectToMatch = function (match) {
            $location.path('/match/' + match.Id);
        }

        function loadFailed(response) {
            notificationService.displayError(response.data);
            $location.path('/');
        }

        function teamLoadCompleted(result) {
            $scope.team = result.data;
        }

        if($routeParams.id != undefined)
        {
            apiService.get('api/team/' + $routeParams.id, null, teamLoadCompleted, loadFailed);
        }
    }

})(angular.module('homeFootball'));