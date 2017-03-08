(function (app) {
    'use strict';

    app.controller('stadiumCtrl', stadiumCtrl);

    stadiumCtrl.$inject = ['$scope', '$location', '$routeParams', 'apiService', 'notificationService'];

    function stadiumCtrl($scope, $location, $routeParams, apiService, notificationService) {
        $scope.pageClass = 'page-home';
        $scope.isReadOnly = true;
        $scope.stadium = null;
        $scope.selectedStadium = function (selectedStadium) {
            if (selectedStadium == undefined) return;
            $location.path('/stadium/' + selectedStadium.originalObject.Id);
        }


        function loadFailed(response) {
            notificationService.displayError(response.data);
            $location.path('/');
        }

        function stadiumLoadCompleted(result) {
            $scope.stadium = result.data;
        }

        if ($routeParams.id != undefined) {
            apiService.get('api/stadium/' + $routeParams.id, null, stadiumLoadCompleted, loadFailed);
        }
    }

})(angular.module('homeFootball'));