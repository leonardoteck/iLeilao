// Ionic Starter App

// angular.module is a global place for creating, registering and retrieving Angular modules
// 'starter' is the name of this angular module example (also set in a <body> attribute in index.html)
// the 2nd parameter is an array of 'requires'
// 'starter.controllers' is found in controllers.js
angular.module('starter', ['ionic', 'starter.controllers', 'starter.services'])

.run(function($ionicPlatform) {
    $ionicPlatform.ready(function() {
        // Hide the accessory bar by default (remove this to show the accessory bar above the keyboard
        // for form inputs)
        if (window.cordova && window.cordova.plugins.Keyboard) {
            cordova.plugins.Keyboard.hideKeyboardAccessoryBar(true);
            cordova.plugins.Keyboard.disableScroll(true);

        }
        if (window.StatusBar) {
            // org.apache.cordova.statusbar required
            StatusBar.styleDefault();
        }
    });
})

.config(function($stateProvider, $urlRouterProvider) {
    $stateProvider

    .state('app', {
        url: '/app',
        abstract: true,
        templateUrl: 'templates/menu.html',
        controller: 'AppCtrl'
    })
    .state('app.solicitacoes', {
        url: '/solicitacoes',
        cache: false,
        views: {
            'menuContent': {
                templateUrl: 'templates/solicitacoes.html',
                controller: 'listaController',
                controllerAs: 'vm'
            }
        }
    })
    .state('app.solicitacoes_part', {
        url: '/solicitacoes_part',
        cache: false,
        views: {
            'menuContent': {
                templateUrl: 'templates/solicitacoes_part.html',
                controller: 'solicitacaoController',
                controllerAs: 'vm'
            }
        }
    })
    .state('app.solicitacoes_leilo', {
        url: '/solicitacoes_leilo',
        cache: false,
        views: {
            'menuContent': {
                templateUrl: 'templates/solicitacoes_leilo.html',
                controller: 'solicitacaoController',
                controllerAs: 'vm'
            }
        }
    })
    .state('app.acompanha', {
        url: '/acompanha',
        cache: false,
        views: {
            'menuContent': {
                templateUrl: 'templates/acompanha.html',
                controller: 'listaController',
                controllerAs: 'vm'
            }
        }
    })
    .state('app.principal', {
        url: '/principal',
        cache: false,
        views: {
            'menuContent': {
                templateUrl: 'templates/principal.html',
                controller: 'principalController',
                controllerAs: 'vm'
            }
        }
    })
    .state('app.visualizar', {
        url: '/visualizar',
        cache: false,
        views: {
            'menuContent': {
                templateUrl: 'templates/visualizar.html'
            }
        }
    })
    .state('app.encerrados', {
        url: '/encerrados',
        cache: false,
        views: {
            'menuContent': {
                templateUrl: 'templates/encerrados.html'
            }
        }
    })
    .state('login', {
        url: '/login',
        // views: {
        //    'menuContent': {
        templateUrl: 'templates/login.html',
        controller: 'loginController',
        controllerAs: 'vm'
            //     }
            // }
    })
    .state('app.leiloei', {
        url: '/leiloei',
        cache: false,
        views: {
            'menuContent': {
                templateUrl: 'templates/leiloei.html',
                controller: 'listaController',
                controllerAs: 'vm'
            }
        }
    })
    .state('app.andamento', {
        url: '/andamento',
        cache: false,
        views: {
            'menuContent': {
                templateUrl: 'templates/andamento.html',
                controller: 'listaController',
                controllerAs: 'vm'
            }
        }
    })
    .state('app.destaque', {
        url: '/destaque',
        cache: false,
        views: {
            'menuContent': {
                templateUrl: 'templates/destaque.html',
                controller: 'listaController',
                controllerAs: 'vm'
            }
        }
    });

    // if none of the above states are matched, use this as the fallback
    $urlRouterProvider.otherwise('/login');
});