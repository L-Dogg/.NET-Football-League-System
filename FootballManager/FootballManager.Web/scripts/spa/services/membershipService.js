(function (app) {
    'use strict';

    app.factory('membershipService', membershipService);

    membershipService.$inject = ['apiService', 'notificationService', '$http', '$base64', '$cookieStore', '$rootScope'];

    function membershipService(apiService, notificationService, $http, $base64, $cookieStore, $rootScope) {

        var service = {
            login: login,
            register: register,
			registerFbUser: registerFbUser,
            saveCredentials: saveCredentials,
            removeCredentials: removeCredentials,
            isUserLoggedIn: isUserLoggedIn,
            getUserLogin: getUserLogin,
            getUserId: getUserId,
            getUserFbId: getUserFbId,
            changePassword: changePassword,
            canChangePassword: canChangePassword,
            isFacebookUser: isFacebookUser
        }

        function login(user, completed) {
            apiService.post('/api/account/authenticate', user,
            completed,
            loginFailed);
        }

        function register(user, completed) {
            apiService.post('/api/account/register', user,
            completed,
            registrationFailed);
        }

        function changePassword(user, completed) {
        	apiService.post('/api/account/change', user,
            completed,
            registrationFailed);
        }

        function registerFbUser(user, completed) {
			var fbUsr = {username : user.username, password: user.password, isfacebookuser: true}
			apiService.post('/api/account/register', fbUsr,
			   completed,
			   function(response) {
			   	apiService.post('/api/account/authenticate', user,
				 completed,
				 loginFailed);
			   });

	    }

        function saveCredentials(user) {
            var membershipData = $base64.encode(user.username + ':' + user.password);
	        $rootScope.repository = {
		        loggedUser: {
			        id: user.id,
                    username: user.username,
                    authdata: membershipData,
					isfacebookuser: user.isfacebookuser
                }
            };
	        if (user.isfacebookuser) {
	            $rootScope.repository.loggedUser.facebookId = user.password;
	        }
            $http.defaults.headers.common['Authorization'] = 'Basic ' + membershipData;
            $cookieStore.put('repository', $rootScope.repository);
        }

        function removeCredentials() {
            $rootScope.repository = {};
            $cookieStore.remove('repository');
            $http.defaults.headers.common.Authorization = '';
        };

        function loginFailed(response) {
            notificationService.displayError(response.data);
        }

        function registrationFailed(response) {

            notificationService.displayError('Registration failed. Try again.');
        }

        function isUserLoggedIn() {
            return $rootScope.repository != undefined && $rootScope.repository.loggedUser != null;
        }

        function getUserLogin() {
            if (isUserLoggedIn()) {
                return $rootScope.repository.loggedUser.username;
            }
            return 'Anonymous';
        }

        function getUserId() {
        	if (isUserLoggedIn()) {
        		return $rootScope.repository.loggedUser.id;
        	}
        	return -1;
        }

        function getUserFbId() {
            if (isUserLoggedIn()) {
                return $rootScope.repository.loggedUser.facebookId;
            }
            return -1;
        }

        function canChangePassword() {
	        if (isUserLoggedIn() && $rootScope.repository.loggedUser.isfacebookuser !== true)
		        return true;
        	return false;
        }

        function isFacebookUser() {
            if (isUserLoggedIn() && $rootScope.repository.loggedUser.isfacebookuser === true)
                return true;
            return false;
        }

       return service;
    }



})(angular.module('common.core'));