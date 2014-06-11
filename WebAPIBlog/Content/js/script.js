var webAPIBlog = angular.module('webAPIBlog', ['ngRoute', 'ui.tinymce', 'ui.bootstrap', 'dialogs.main']);

function showError(status, data, dialogs) {
    err = JSON.stringify(data, null, 4);
    if (data.Message != undefined) {
        err = "<b>" + data.Message + "</b><br/><br/>";
        if (data.ModelState != undefined) {
            for (var i in data.ModelState) {
                err += data.ModelState[i] + "<br/>";
            }
        }
        if (data.Error != undefined) {
            err += data.Error + "<br/>";
        }
    }
    dialogs.error("Error " + status, err);
}

webAPIBlog.config(function($routeProvider) {
	$routeProvider
		.when('/', {
			templateUrl: 'pages/posts.html',
			controller: 'postsController'
	//	})
	//	.when('/account', {
	//		templateUrl: 'pages/account.html',
	//		controller: 'accountController'
		});
});

webAPIBlog.service('userService', function ($http) {
	var service = this;

	this.__defineSetter__("User", function (value) {
		if (value == null)
			onUserChanged({ UserName: 'Anonymous' }, false);
		else
			onUserChanged(value, true);
	});

	var userChanged = [];
	var onUserChanged = function (newUser, signed) {
		angular.forEach(userChanged, function (callback) {
			callback(newUser, signed);
		});
	};

	this.__defineSetter__("UserChanged", function (value) {
		if (typeof (value) == "function")
			userChanged.push(value);
	});

	this.signin = function (signinModel, modalInstance, dialogs) {
		$http.post('account/signin', signinModel).success(function(data, status, headers, config) {
		    service.User = signinModel;
		    modalInstance.close();
		}).error(function(data, status, headers, config) {
		    showError(status, data, dialogs);
		});
	};

	this.register = function(registerModel, modalInstance, dialogs) {
	    $http.post('account/register', registerModel).success(function (data) {
	        dialogs.notify("Information", "Registration successful. You can login now.");
		    modalInstance.close();
		}).error(function(data, status) {
		    showError(status, data, dialogs);
		});
	};

	this.signout = function(dialogs) {
		$http.get('account/signout').success(function(data, status, headers, config) {
			service.User = null;
		}).error(function(data, status, headers, config) {
		    showError(status, data, dialogs);
		});
	};

	$http.get('account/').success(function (data) {
		service.User = data;
	}).error(function (data, status) {
		service.User = null;
	});
});

webAPIBlog.service('postService', function($http) {
	var service = this;
	var posts = [];

	var postsChangedCallbacks = [];
	var onPostsChanged = function (updatedPosts) {
		angular.forEach(postsChangedCallbacks, function (callback) {
			callback(updatedPosts);
		});
	};

	this.__defineSetter__("postsChangedCallback", function (value) {
		if (typeof (value) == "function")
			postsChangedCallbacks.push(value);
	});

	this.__defineSetter__("posts", function (value) {
		posts.unshift(value);
		onPostsChanged(posts);
	});
	this.__defineGetter__("posts", function() {
		return posts;
	})

	this.showPostForm = false;

	$http.get('post?offset=0&count=100', { cache: true }).success(function (data, status, headers, config) {
		posts = data;
		onPostsChanged(posts);
	}).error(function (data, status, headers, config) {
	    alert("Error (" + status + ")!\n\n" + JSON.stringify(data, null, 4));
	});
});

webAPIBlog.controller('mainController', function ($scope, $window, $modal, userService, postService) {
	userService.UserChanged = function (newUser, signed) {
		$scope.User = newUser;
		$scope.signed = signed;
	};

	$scope.signinModal = function() {
		$modal.open({
			templateUrl: 'pages/signinModal.html',
			controller: accountContoller
		});
	};

	$scope.registerModal = function () {
		$modal.open({
			templateUrl: 'pages/registerModal.html',
			controller: accountContoller
		});
	};

	$scope.signout = function (dialogs) {
		userService.signout(dialogs);
	};

	$scope.postService = postService;
});

var accountContoller = function ($scope, $http, $window, $modalInstance, userService, dialogs) {
	$scope.User = userService.User;
	userService.UserChanged = function(newUser) {
		$scope.User = newUser;
	};

	$scope.RegisterModel = {
		UserName: null,
		Password: null,
		Email: null,
	};

	$scope.SignInModel = {
		UserName: null,
		Password: null,
		IsPersistent: true,
	};

	$scope.register = function () {
	    userService.register($scope.RegisterModel, $modalInstance, dialogs);
		//$modalInstance.close();
	};

	$scope.signin = function() {
		userService.signin($scope.SignInModel, $modalInstance, dialogs);
		//$modalInstance.close();
	};

	$scope.signout = function() {
		userService.signout(dialogs);
	};

	$scope.close = function() {
		$modalInstance.close();
	};
};

webAPIBlog.controller('postsController', function ($scope, $sce, postService) {
	$scope.posts = postService.posts;

	postService.postsChangedCallback = function (posts) {
		$scope.posts = posts;
	};
	
	$scope.renderHtml = function(htmlCode) {
		return $sce.trustAsHtml(htmlCode);
	};
});

webAPIBlog.controller('postController', function($scope, $http, postService, dialogs) {
	$scope.tinymceOptions = {
		plugins: "image, autoresize, code",
		autoresize_min_height: 300
	};

	$scope.post =
	{
		title: null,
		content: null,
	};

	$scope.submit = function() {
		$http.post('post', $scope.post).success(function(data, status, headers, config) {
			postService.posts = data;
			postService.showPostForm = false;
		}).error(function(data, status, headers, config) {
		    showError(status, data, dialogs);
		});
	};
});