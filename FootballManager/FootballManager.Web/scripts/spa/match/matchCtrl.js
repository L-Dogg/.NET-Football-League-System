(function (app) {
    'use strict';

    app.controller('matchCtrl', matchCtrl);

    matchCtrl.$inject = ['$scope', '$location', '$routeParams', '$sce', '$document', 'apiService', 'notificationService', 'membershipService'];

    function matchCtrl($scope, $location, $routeParams, $sce, $document, apiService, notificationService, membershipService) {
        $scope.pageClass = 'page-home';
        $scope.isReadOnly = true;
        $scope.match = null;
        $scope.newComment = {};
        $scope.comments = [];
        $scope.PictureUrl = '';

        $scope.redirectToPlayer = function (id) {
            $location.path('/player/' + id);
        }
        
        $scope.redirectToTeam = function (id) {
        	$location.path('/team/' + id);
        }
        
        $scope.redirectToReferee = function (id) {
        	$location.path('/referees/' + id);
        }

        $scope.redirectToStadium = function (id) {
        	$location.path('/stadium/' + id);
        }

        $scope.getIconUrl = function(goalType) {
	        if (goalType === 'Own')
	        	return '../../../Content/images/own-goal.png';
	        return '../../../Content/images/goal.png';
        }

        $scope.saveComment = function () {
            var login = membershipService.getUserLogin();
            var id;
            if (membershipService.isFacebookUser()) {
                id = membershipService.getUserFbId();
            }
            else {
                id = membershipService.getUserId();
            }
            if (checkIfUserCommented(id)) {
                notificationService.displayError('You already rated this match');
                return;
            }
            var newComment = {};
            newComment.Text = $scope.newComment.Text;
            newComment.Username = login;
            newComment.Date = new Date();
            newComment.MatchId = $scope.match.Id;
            if (membershipService.isFacebookUser()) {
                newComment.FacebookId = id;
            }
            else {
                newComment.UserId = id;
            }
            $scope.popoverIsVisible = false;
            newComment.Rating = $scope.newComment.Rating;
            apiService.post('api/match/saveComment', newComment, addCommentToList, loadFailed);
        }

        function checkIfUserCommented (id){
            var i = 0, len = $scope.match.Comments.length;
            if (membershipService.isFacebookUser()) {
                for (; i < len; i++) {
                    if ($scope.match.Comments[i].FacebookId === id) {
                        return true;
                    }
                }
            }
            else {
                for (; i < len; i++) {
                    if ($scope.match.Comments[i].UserId === id) {
                        return true;
                    }
                }
            }
            return false;
        }

        function addCommentToList (result){
            $scope.match.Comments.push(result.data);
            $scope.newComment = { Rating: 0 };
        }

        $scope.url = $sce.trustAsResourceUrl(
			'https://www.google.com/maps/embed/v1/directions?key=AIzaSyC9L4Kz7hJLc8_G_08zMt1uj4R9McQE6iA&origin=Sulejow+Poland&destination=Warsaw+Poland');
        var iFrame = $document.find("mapFrame");
        iFrame.attr("src", iFrame.attr("src"));
	    var destinationAddressString = '';

        $scope.changeIt = function() {
            var originAddressString = $scope.originAddress.split(' ').join('+');
            if (!originAddressString) return;

            $scope.url = $sce.trustAsResourceUrl(
                'https://www.google.com/maps/embed/v1/directions?key=AIzaSyC9L4Kz7hJLc8_G_08zMt1uj4R9McQE6iA&origin=' + originAddressString +
                '&destination=' + destinationAddressString);
            iFrame.attr("src", iFrame.attr("src"));
        }

        $scope.setUrl = function (footballer) {
	        $scope.PictureUrl = footballer.Team + '/' + footballer.PictureUrl;
        };
		
        function playerLoadCompleted(result) {
        	$scope.player = result.data;
        };
		
        function loadFailed(response) {
            notificationService.displayError(response.data);
            $location.path('/');
        }

        function matchLoadCompleted(result) {
        	$scope.match = result.data;
        	destinationAddressString = $scope.match.Stadium.Address.Street.split(' ').join('+') +
                '+' +
                $scope.match.Stadium.Address.Number +
                '+' +
                $scope.match.Stadium.Address.City;
        	$scope.url = $sce.trustAsResourceUrl(
                'https://www.google.com/maps/embed/v1/directions?key=AIzaSyC9L4Kz7hJLc8_G_08zMt1uj4R9McQE6iA&origin=Warsaw+Poland' +
                '&destination=' + destinationAddressString);
        	iFrame.attr("src", iFrame.attr("src"));
        }

        if ($routeParams.id != undefined) {
            apiService.get('api/match/' + $routeParams.id, null, matchLoadCompleted, loadFailed);
        }

    }

})(angular.module('homeFootball'));