(function (app) {
	'use strict';

	app.controller('passresetCtrl', passresetCtrl);

	passresetCtrl.$inject = ['$scope', 'membershipService', 'notificationService', '$rootScope', '$location'];

	function passresetCtrl($scope, membershipService, notificationService, $rootScope, $location) {
		$scope.pageClass = 'page-home';
		$scope.change = change;
		$scope.newPassword = '';
		$scope.oldPassword = '';
		
		function change() {
			membershipService.changePassword({
					oldpassword: $scope.oldPassword,
					newPassword: $scope.newPassword,
					id: membershipService.getUserId()
				},
				resetCompleted);
		}

		function resetCompleted(result) {
			if (result.data.success) {
				var _id = membershipService.getUserId();
				var login = membershipService.getUserLogin();
				membershipService.removeCredentials();
				membershipService.saveCredentials({
					username: login,
					password: $scope.newPassword,
					id: _id
				});
				notificationService.displaySuccess('Password reset successfull');
				$location.path('/');
			}
			else {
				notificationService.displayError('Password reset failed. Try again.');
			}
		}
	}

})(angular.module('common.core'));