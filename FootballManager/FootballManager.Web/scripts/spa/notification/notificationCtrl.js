(function (app) {
    'use strict';

    app.controller('notificationCtrl', notificationCtrl);

    notificationCtrl.$inject = ['$scope', '$location', '$routeParams', 'apiService', 'notificationService', 'membershipService'];

    function notificationCtrl($scope, $location, $routeParams, apiService, notificationService, membershipService) {
        if (!membershipService.isFacebookUser()) {
            $location.path('/');
        }
        $scope.pageClass = 'page-home';
        $scope.isReadOnly = true;
        $scope.team = null;
        $scope.notifications = [];
        $scope.selectedTeam = function (selectedTeam) {
            if (selectedTeam == undefined) return;
            var obj = { FacebookId: membershipService.getUserFbId(), TeamId: selectedTeam.originalObject.Id, Team: selectedTeam.originalObject };
            apiService.post('api/notification/addNotification', obj, addCompleted, loadFailed);
        }

        $scope.removeNotification = function (notification) {
            apiService.post('api/notification/removeNotification', notification, removeCompleted, loadFailed);
        }

        function loadFailed(response) {
            notificationService.displayError(response.data);
        }

        function addCompleted(response) {
            $scope.notifications.push(response.data);
        }

        function removeCompleted(response) {
            var length = $scope.notifications.length, i = 0;
            for (i; i < length; i++) {
                if ($scope.notifications[i].Id === response.data.Id) {
                    $scope.notifications.splice(i, 1);
                    return;
                }
            }
        }

        function notificationLoadCompleted(result) {
            $scope.notifications = result.data;
        }

        apiService.get('api/notification/list/' + membershipService.getUserFbId(), null, notificationLoadCompleted, loadFailed);
    }

})(angular.module('homeFootball'));