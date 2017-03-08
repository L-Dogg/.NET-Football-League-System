(function () {
    'use strict';

    angular.module('homeFootball', ['common.core', 'common.ui'])
        .config(config)
        .run();     //TODO nie wiem co to, zobaczymy czy zadziala

    config.$inject = ['$routeProvider'];
    function config($routeProvider) {
        $routeProvider
            .when("/", {
                templateUrl: "scripts/spa/home/index.html",
                controller: "indexCtrl"
            })
            .when("/season", {
                templateUrl: "scripts/spa/season/index.html",
                controller: "seasonCtrl"
            })
            .when("/season/:id", {
                templateUrl: "scripts/spa/season/index.html",
                controller: "seasonCtrl"
            })
            .when("/team", {
                templateUrl: "scripts/spa/team/index.html",
                controller: "teamCtrl"
            })
            .when("/team/:id", {
                templateUrl: "scripts/spa/team/index.html",
                controller: "teamCtrl"
            })
            .when("/referees", {
                templateUrl: "scripts/spa/referees/index.html",
                controller: "refereesCtrl"
            })
            .when("/referees/:id", {
                templateUrl: "scripts/spa/referees/index.html",
                controller: "refereesCtrl"
            })
            .when("/coaches", {
                templateUrl: "scripts/spa/coaches/index.html",
                controller: "coachesCtrl"
            })
            .when("/player", {
                templateUrl: "scripts/spa/player/index.html",
                controller: "playerCtrl"
            })
            .when("/player/:id", {
                templateUrl: "scripts/spa/player/index.html",
                controller: "playerCtrl"
            })
             .when("/coaches/:id", {
                 templateUrl: "scripts/spa/coaches/index.html",
                 controller: "coachesCtrl"
             })
            .when("/login", {
                templateUrl: "scripts/spa/account/login.html",
                controller: "loginCtrl"
            })
            .when("/register", {
                templateUrl: "scripts/spa/account/register.html",
                controller: "registerCtrl"
            })
			.when("/passreset", {
				templateUrl: "scripts/spa/account/passreset.html",
				controller: "passresetCtrl"
			})
             .when("/stadium", {
                 templateUrl: "scripts/spa/stadium/index.html",
                 controller: "stadiumCtrl"
             })
            .when("/stadium/:id", {
                templateUrl: "scripts/spa/stadium/index.html",
                controller: "stadiumCtrl"
            })
            .when("/match/:id", {
                templateUrl: "scripts/spa/match/index.html",
                controller: "matchCtrl"
            })
            .when("/notification", {
            	templateUrl: "scripts/spa/notification/index.html",
            	controller: "notificationCtrl"
            })
            //.when("/movies", {
            //    templateUrl: "scripts/spa/movies/movies.html",
            //    controller: "moviesCtrl"
            //})
            //.when("/movies/add", {
            //    templateUrl: "scripts/spa/movies/add.html",
            //    controller: "movieAddCtrl"
            //})
            //.when("/movies/:id", {
            //    templateUrl: "scripts/spa/movies/details.html",
            //    controller: "movieDetailsCtrl"
            //})
            //.when("/movies/edit/:id", {
            //    templateUrl: "scripts/spa/movies/edit.html",
            //    controller: "movieEditCtrl"
            //})
            //.when("/rental", {
            //    templateUrl: "scripts/spa/rental/rental.html",
            //    controller: "rentStatsCtrl"
    //})
            .otherwise({ redirectTo: "/" });
    }
    
    run.$inject = ['$rootScope', '$location', '$cookieStore', '$http'];

    function run($rootScope, $location, $cookieStore, $http) {
        // handle page refreshes
        $rootScope.repository = $cookieStore.get('repository') || {};
        if ($rootScope.repository.loggedUser) {
            $http.defaults.headers.common['Authorization'] = $rootScope.repository.loggedUser.authdata;
        }

        $(document).ready(function () {
            $(".fancybox").fancybox({
                openEffect: 'none',
                closeEffect: 'none'
            });

            $('.fancybox-media').fancybox({
                openEffect: 'none',
                closeEffect: 'none',
                helpers: {
                    media: {}
                }
            });

            $('[data-toggle=offcanvas]').click(function () {
                $('.row-offcanvas').toggleClass('active');
            });
        });
    }

})();