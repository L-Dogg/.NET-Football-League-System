(function (app) {
    'use strict';

    app.controller('seasonCtrl', seasonCtrl);

    seasonCtrl.$inject = ['$scope', '$routeParams', '$location', 'apiService', 'notificationService'];

    function seasonCtrl($scope, $routeParams, $location, apiService, notificationService) {
        $scope.pageClass = 'page-home';
        $scope.isReadOnly = true;

        $scope.season = null;
        $scope.loadSeasonData = loadSeasonData;
        $scope.selectionChanged = selectionChanged;

        $scope.selectedSeason = function (selectedSeason) {
            if (selectedSeason == undefined) return;
            $location.path('/season/' + selectedSeason.originalObject.Id);
        }

        $scope.redirectToMatch = function (match) {
            $location.path('/match/' + match.Id);
        }

        function loadFailed(response) {
            notificationService.displayError(response.data);
            $location.path('/');
        }

        function loadSeasonData()
        {
            apiService.get('api/season/' + $scope.selectedSeason,null, seasonLoadCompleted, loadFailed);
        }

        function selectionChanged(){
            $location.path('/season/' + $scope.selectedSeason);
        }

        function seasonLoadCompleted(result) {
            $scope.season = result.data;
        }

        if ($routeParams.id != undefined) {
            apiService.get('api/season/' + $routeParams.id, null, seasonLoadCompleted, loadFailed);
        }

    }

})(angular.module('homeFootball'));