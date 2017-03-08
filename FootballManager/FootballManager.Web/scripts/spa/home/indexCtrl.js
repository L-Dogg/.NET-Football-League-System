(function (app) {
    'use strict';

    app.controller('indexCtrl', indexCtrl);

    indexCtrl.$inject = ['$scope', 'apiService', 'notificationService', '$location'];

    function indexCtrl($scope, apiService, notificationService, $location) {
        $scope.pageClass = 'page-home';
        $scope.loadingMatches = true;
        $scope.loadingPreviousMatches = true;
       // $scope.loadingGenres = true;
        $scope.isReadOnly = true;

        $scope.nextRoundMatches = [];
        $scope.previousRoundMatches = [];
        $scope.loadData = loadData;

        $scope.redirectToMatch = function (match) {
            $location.path('/match/' + match.Id);
        }

        function loadData() {
            apiService.get('/api/matches/next', null,
                        matchesLoadCompleted,
                        matchesLoadFailed);

            apiService.get('/api/matches/previous', null,
                        previousMatchesLoadCompleted,
                        matchesLoadFailed);
        }

        function matchesLoadCompleted(result) {
            $scope.nextRoundMatches = result.data;
            $scope.loadingMatches = false;
        }

        function previousMatchesLoadCompleted(result) {
            $scope.previousRoundMatches = result.data;
            $scope.loadingPreviousMatches = false;
        }


        function matchesLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        loadData();
    }

})(angular.module('homeFootball'));