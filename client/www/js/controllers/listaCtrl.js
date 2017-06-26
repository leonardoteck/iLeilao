(function() {
'use strict';

    angular
        .module('starter.controllers')
        .controller('listaController', listaController);

    listaController.$inject = ['$scope', '$http', 'api', 'auth', '$ionicPopup', '$state'];
    function listaController($scope, $http, api, auth, $ionicPopup, $state) {
        var vm = this;

        vm.dados = {};

        activate();

        ////////////////

        function activate() {
            if (!auth.done) {
                window.location.hash = '#/';
                return;
            }

            var url = '';
            switch ($state.current.name) {
                case 'app.solicitacoes':
                    if (auth.participante)
                        url = 'solicitacoes/usuario/' + auth.id;
                    else
                        url = 'solicitacoes/pendentes'
                    break;
                case 'app.acompanha':
                    url = 'leiloes/usuario/participando/' + auth.id;
                    break;
                case 'app.leiloei':
                    url = 'leiloes/usuario/' + auth.id;
                    break;
                case 'app.andamento':
                    url = 'leiloes'
                    break;
                case 'app.destaque':
                    url = 'leiloes/populares'
                    break;
                default:
                    url = 'solicitacoes'
                    break;
            }
            carregarDados(url);
        }

        function carregarDados(url) {
            if (!api.on()) {
                
                return;
            }

            $http({
                method: 'GET',
                url: api.url() + url,
                headers: { 'Authorization': 'Bearer ' + auth.token }
            }).success(function (data) {
                vm.dados = data;
            }).error(function (data) {
                console.log(data);
                $ionicPopup.alert({ title: 'Ops!', template: data[0].errorMessage });
            });
        }
    }
})();