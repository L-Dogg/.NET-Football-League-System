(function (app) {
    'use strict';

    app.controller('playerCtrl', playerCtrl);

    playerCtrl.$inject = ['$scope', '$location', '$routeParams', 'apiService', 'notificationService'];

    function playerCtrl($scope, $location, $routeParams, apiService, notificationService) {
        $scope.pageClass = 'page-home';
        $scope.isReadOnly = true;
        $scope.player = null;
        $scope.selectedPlayer = function (selectedPlayer) {
            if (selectedPlayer == undefined) return;
            $location.path('/player/' + selectedPlayer.originalObject.Id);
        }


        function loadFailed(response) {
            notificationService.displayError(response.data);
            $location.path('/');
        }

        function playerLoadCompleted(result) {
            $scope.player = result.data;
        }

        if ($routeParams.id != undefined) {
            apiService.get('api/player/' + $routeParams.id, null, playerLoadCompleted, loadFailed);
        }
    }

})(angular.module('homeFootball'));