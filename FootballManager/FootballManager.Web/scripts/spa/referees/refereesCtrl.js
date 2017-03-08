(function (app) {
    'use strict';

    app.controller('refereesCtrl', refereesCtrl);

    refereesCtrl.$inject = ['$scope', '$location', '$routeParams', 'apiService', 'notificationService'];

    function refereesCtrl($scope, $location, $routeParams, apiService, notificationService) {
        $scope.pageClass = 'page-home';

        $scope.referees = [];
        $scope.selectedReferee = {};
        $scope.referee = null;
        $scope.getRefereeData = getRefereeData;
        
        function loadFailed(response) {
            notificationService.displayError(response.data);
            $location.path('/');
        }
        
        function getRefereeData(selected) {
            if (selected == undefined)
                return;
            $location.path('/referees/' + selected.originalObject.Id);
        }

        function refereeDataLoadCompleted(result) {
            $scope.referee = result.data;
        }

        if ($routeParams.id != undefined) {
            apiService.get('api/referees/' + $routeParams.id, null, refereeDataLoadCompleted, loadFailed);
        }

        $scope.redirectToMatch = function (match) {
        	$location.path('/match/' + match.Id);
        }
    }

})(angular.module('homeFootball'));