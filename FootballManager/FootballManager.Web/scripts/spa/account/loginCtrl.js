(function (app) {
    'use strict';

    app.controller('loginCtrl', loginCtrl);

    loginCtrl.$inject = ['$scope', 'membershipService', 'notificationService', '$rootScope', '$location', '$document'];

    function loginCtrl($scope, membershipService, notificationService, $rootScope, $location, $document) {
        $scope.pageClass = 'page-login';
        $scope.login = login;
        $scope.user = {};
        
        function login() {
            membershipService.login($scope.user, loginCompleted);
        }

        function loginCompleted(result) {
        	if (result.data.success) {
		        $scope.user.id = result.data.id;
                membershipService.saveCredentials($scope.user);
                notificationService.displaySuccess('Hello ' + $scope.user.username);
                $scope.userData.displayUserInfo();
                if ($rootScope.previousState)
                    $location.path($rootScope.previousState);
                else
                    $location.path('/');
            }
            else {
                notificationService.displayError('Login failed. Try again.');
            }
        }
        
        $scope.fbLoginMsg = function (name, id) {
        	$scope.user = { 'username': name, 'password': id };
	        membershipService.registerFbUser($scope.user, function (result) {
	        	$scope.user.id = result.data.id;
		        $scope.user.isfacebookuser = true;
	            membershipService.saveCredentials($scope.user);
	            notificationService.displaySuccess('Hello ' + $scope.user.username);
	            $scope.userData.displayUserInfo();
	            if ($rootScope.previousState)
	                $location.path($rootScope.previousState);
	            else {
	                $location.path('/');
	            }
	        });
        }
    }

})(angular.module('common.core'));