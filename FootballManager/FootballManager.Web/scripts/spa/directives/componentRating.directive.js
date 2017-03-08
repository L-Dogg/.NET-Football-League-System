(function (app) {
    'use strict';

    app.directive('displayComponentRating', displayComponentRating);

    function displayComponentRating() {
        return {
            restrict: 'A',
            link: function ($scope, $element, $attrs) {
                $element.raty({
                    score: $attrs.displayComponentRating,
                    halfShow: false,
                    readOnly: true,
                    noRatedMsg: "Not rated yet!",
                    starHalf: "../../../Content/images/raty/star-half.png",
                    starOff: "../../../Content/images/raty/star-off.png",
                    starOn: "../../../Content/images/raty/star-on.png",
                    hints: ["Poor", "Average", "Good", "Very Good", "Excellent"],
                    click: function (score, event) {
                        //Set the model value
                    }
                });
            }
        }
    }

})(angular.module('common.ui'));

(function (app) {
    'use strict';

    app.directive('editComponentRating', editComponentRating);

    function editComponentRating() {
        return {
            restrict: 'A',
            link: function ($scope, $element, $attrs) {
                $element.raty({
                    score: $attrs.editComponentRating,
                    halfShow: false,
                    readOnly: false,
                    noRatedMsg: "Not rated yet!",
                    starHalf: "../Content/images/raty/star-half.png",
                    starOff: "../Content/images/raty/star-off.png",
                    starOn: "../Content/images/raty/star-on.png",
                    hints: ["Poor", "Average", "Good", "Very Good", "Excellent"],
                    click: function (score, event) {
                        //Set the model value
                        $scope.newComment.Rating = score;
                    }
                });
            }
        }
    }

})(angular.module('common.ui'));