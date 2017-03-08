using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace FootballManager.Web.App_Start
{
	public class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
				"~/scripts/Vendors/modernizr.js"));

			bundles.Add(new ScriptBundle("~/bundles/vendors").Include(
				"~/scripts/Vendors/jquery.js",
				"~/scripts/Vendors/bootstrap.js",
				"~/scripts/Vendors/toastr.js",
				"~/scripts/Vendors/jquery.raty.js",
				"~/scripts/Vendors/respond.src.js",
				"~/scripts/Vendors/angular.js",
				"~/scripts/Vendors/angular-route.js",
				"~/scripts/Vendors/angular-cookies.js",
				"~/scripts/Vendors/angular-validator.js",
				"~/scripts/Vendors/angular-base64.js",
				"~/scripts/Vendors/angular-file-upload.js",
				"~/scripts/Vendors/angucomplete-alt.min.js",
				"~/scripts/Vendors/ui-bootstrap-tpls-0.13.1.js",
				"~/scripts/Vendors/underscore.js",
				"~/scripts/Vendors/raphael.js",
				"~/scripts/Vendors/morris.js",
				"~/scripts/Vendors/jquery.fancybox.js",
				"~/scripts/Vendors/jquery.fancybox-media.js",
				"~/scripts/Vendors/loading-bar.js",
				"~/scripts/Vendors/ui-bootstrap-custom-2.4.0.js",
				"~/scripts/Vendors/ui-bootstrap-custom-2.4.0.min.js",
				"~/scripts/Vendors/ui-bootstrap-custom-tpls-2.4.0.js",
				"~/scripts/Vendors/ui-bootstrap-custom-tpls-2.4.0.min.js"
				));

			bundles.Add(new ScriptBundle("~/bundles/spa").Include(
				"~/scripts/spa/modules/common.core.js",
				"~/scripts/spa/modules/common.ui.js",
				"~/scripts/spa/app.js",
				"~/scripts/spa/services/apiService.js",
				"~/scripts/spa/services/notificationService.js",
				"~/scripts/spa/services/membershipService.js",
				"~/scripts/spa/services/fileUploadService.js",
				"~/scripts/spa/layout/topBar.directive.js",
				"~/scripts/spa/layout/sideBar.directive.js",
				"~/scripts/spa/layout/customPager.directive.js",
				"~/scripts/spa/directives/rating.directive.js",
				"~/scripts/spa/directives/availableMovie.directive.js",
				"~/scripts/spa/account/loginCtrl.js",
				"~/scripts/spa/account/registerCtrl.js",
				"~/scripts/spa/account/passresetCtrl.js",
				"~/scripts/spa/home/rootCtrl.js",
				"~/scripts/spa/home/indexCtrl.js",
				"~/scripts/spa/customers/customersCtrl.js",
				"~/scripts/spa/customers/customersRegCtrl.js",
				"~/scripts/spa/customers/customerEditCtrl.js",
				"~/scripts/spa/movies/moviesCtrl.js",
				"~/scripts/spa/movies/movieAddCtrl.js",
				"~/scripts/spa/movies/movieDetailsCtrl.js",
				"~/scripts/spa/movies/movieEditCtrl.js",
				"~/scripts/spa/controllers/rentalCtrl.js",
				"~/scripts/spa/rental/rentMovieCtrl.js",
				"~/scripts/spa/rental/rentStatsCtrl.js",
				"~/scripts/spa/season/seasonCtrl.js",
				"~/scripts/spa/team/teamCtrl.js",
				"~/scripts/spa/referees/refereesCtrl.js",
				"~/scripts/spa/coaches/coachesCtrl.js",
				"~/scripts/spa/player/playerCtrl.js",
				"~/scripts/spa/stadium/stadiumCtrl.js",
				"~/scripts/spa/match/matchCtrl.js",
				"~/scripts/spa/directives/componentRating.directive.js",
				"~/scripts/spa/notification/notificationCtrl.js"
				));

			bundles.Add(new StyleBundle("~/Content/css").Include(
				"~/content/css/site.css",
				"~/content/css/bootstrap.css",
				"~/content/css/bootstrap-theme.css",
				 "~/content/css/font-awesome.css",
				"~/content/css/morris.css",
				"~/content/css/toastr.css",
				"~/content/css/jquery.fancybox.css",
				"~/content/css/bootstrap-select.css",
				"~/content/css/loading-bar.css"));

			BundleTable.EnableOptimizations = false;
		}
	}
}