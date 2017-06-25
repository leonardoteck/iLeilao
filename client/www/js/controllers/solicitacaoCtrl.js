(function() {
'use strict';

    angular
        .module('starter.controllers')
        .controller('listaController', listaController);

    listaController.$inject = ['$scope', '$http', 'api', 'auth', '$ionicPopup', '$state'];
    function listaController($scope, $http, api, auth, $ionicPopup, $state) {
        var vm = this;

        vm.Dados = {
            Lote: {
                Produtos: [
                    {}
                ]
            }
        };
        vm.tipoUsuario = 0;

        vm.cadastrar = cadastrar;

        activate();

        ////////////////

        function activate() {
            console.log($state);
            switch ($state.current.name) {
                case 'app.solicitacoes':
                    
                    break;
                case 'app.acompanha':

                    break;
                case 'app.leiloei':

                    break;
                case 'app.andamento':

                    break;
                case 'app.destaque':

                    break;
                default:
                    break;
            }

            if (!auth.done) {
                window.location.hash = '#/';
                return;
            }
            carregarDados();
        }

        function carregarDados() {
            if (!api.on()) {

                return;
            }

            // $http({
            //     method: 'GET',
            //     url: api.url() + 'movimentacoes/usuario/' + auth.data.config.usuario.Id,
            //     headers: { 'Authorization': 'Bearer ' + auth.data.tokenUsuario.tokenUsuario }
            // }).success(function (data) {
            //     vm.Dados.Movimentacoes = data;
            //     filtrarMovs(vm.Dados.Movimentacoes);
            // }).error(function (data) {
            //     console.log(data);
            //     $ionicPopup.alert({
            //         title: 'Ops!',
            //         template: data[0].errorMessage
            //     });
            // });
        }

        function addProduto() {
            vm.Dados.Lote.Produtos.push({});
        }

        function salvarDados() {
            var dados = JSON.parse(JSON.stringify(vm.Dados));

            // Resolvendo o TimeSpan
            var dataAtual = new Date();
            var dataFinal = vm.Dados.DataFinal
            if (dataAtual.valueOf() > dataFinal.valueOf()) {
                $ionicPopup.alert({ title: 'Sucesso!', template: msg });
            }
            dados.TempoLimiteLance =
                (dataFinal.getDay() - dataAtual.getDay()) + ":" + 
                (dataFinal.getHours() - dataAtual.getHours()) + ":" +
                (dataFinal.getMinutes() - dataAtual.getMinutes()) + ":" +
                (dataFinal.getSeconds() - dataAtual.getSeconds());

            var request = {
                method: 'POST',
                data: dados,
                url: api.url() + 'Solicitacoes',
                headers: { 'Authorization': 'Bearer ' + auth.data.tokenUsuario.tokenUsuario }
            };
            var msg = 'Solicitação cadastrada com sucesso!';
            vm.Dados.UsuarioId = auth.id;

            if (vm.Dados.Id) {
                request.method = 'PUT';
                request.url += vm.Dados.Id;
                var msg = 'Solicitação alterada com sucesso!';
            }

            $http(request)
            .success(function (data) {
                $ionicPopup.alert({ title: 'Sucesso!', template: msg });
                console.log(data);
            })
            .error(function (data) {
                console.log(data);
                $ionicPopup.alert({ title: 'Ops!', template: data[0].errorMessage });
            });
        }
    }
})();