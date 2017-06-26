(function() {
'use strict';

    angular
        .module('starter.controllers')
        .controller('solicitacaoController', solicitacaoController);

    solicitacaoController.$inject = ['$scope', '$http', 'api', 'auth', '$ionicPopup', '$state', '$timeout'];
    function solicitacaoController($scope, $http, api, auth, $ionicPopup, $state, $timeout) {
        var vm = this;

        vm.dados = {
            lote: {
                produtos: [
                    {}
                ]
            }
        };

        vm.salvarDados = salvarDados;
        vm.base64 = base64;
        vm.aceitar = aceitar;
        vm.recusar = recusar;

        activate();

        ////////////////

        function activate() {
            if (!auth.done) {
                window.location.hash = '#/';
                return;
            }
            carregarDados();
        }

        function carregarDados() {
            if (!api.on()) {
                vm.dados = {
                    "nome": "Moto G2 - Caiu do caminhão",
                    "diasDuracao": 20,
                    "incrementoMinimo": 30,
                    "status": 0,
                    "usuarioId": "1ff83f28-8d42-4fb0-a976-c13344d70917",
                    "lote": {
                        "produtos": [
                            {
                                "nome": "Moto G",
                                "descricao": "2nd Generation",
                                "quantidade": 4,
                                "imagem": "http://www.comprasparaguai.com.br/media/fotos/modelos/celular_motorola_moto_g_xt_1068_dual_chip_8gb_26740_550x550.png",
                            }
                        ],
                        "valorMinimo": 30,
                        "vendedorId": "1ff83f28-8d42-4fb0-a976-c13344d70917",
                        "vendedor": null
                    },
                    "usuario": null
                }
                formataDados(vm.dados);
                return;
            }

            if ($state.params.id)
                $http({
                    method: 'GET',
                    url: api.url() + 'solicitacoes/' + $state.params.id,
                    headers: { 'Authorization': 'Bearer ' + auth.token }
                }).success(function (data) {
                    vm.dados = data;
                    formataDados(data);
                }).error(function (data) {
                    console.log(data);
                    $ionicPopup.alert({
                        title: 'Ops!',
                        template: data[0].errorMessage
                    });
                });
        }

        function formataDados(data) {
            if (data.lote.produtos.length == 0)
                data.lote.produtos.push({});
        }

        function salvarDados() {
            var dados = JSON.parse(JSON.stringify(vm.dados));

            // Formatando alguns dados para a API entender
                dados.UsuarioId = auth.id;
                dados.status = 0

                for (var i = 0; i < dados.lote.produtos.length; i++) {
                    delete dados.lote.produtos[i].imagem;
                    if (!dados.lote.produtos[i].nome) {
                        dados.lote.produtos.splice(i, 1);
                        i--;
                    }
                }

            var request = {
                method: 'POST',
                data: dados,
                url: api.url() + 'Solicitacoes',
                headers: { 'Authorization': 'Bearer ' + auth.token }
            };
            var msg = 'Solicitação cadastrada com sucesso!';

            if (vm.dados.id) {
                request.method = 'PUT';
                request.url += '/' + vm.dados.id;
                var msg = 'Solicitação alterada com sucesso!';
            }

            $http(request)
            .success(function (data) {
                console.log(data);
                $ionicPopup.alert({ title: 'Sucesso!', template: msg });
            })
            .error(function (data) {
                console.log(data);
                $ionicPopup.alert({ title: 'Ops!', template: data[0].errorMessage });
            });
        }

        function aceitar() {
            $http({
                method: 'POST',
                url: api.url() + 'solicitacoes/aprovar/' + vm.dados.id,
                headers: { 'Authorization': 'Bearer ' + auth.token }
            })
            .success(function (data) {
                console.log(data);
                $ionicPopup.alert({ title: 'Sucesso!', template: 'Solicitação aprovada com sucesso.' });
            })
            .error(function (data) {
                console.log(data);
                $ionicPopup.alert({ title: 'Ops!', template: data[0].errorMessage });
            });
        }

        function recusar() {
            $http({
                method: 'POST',
                url: api.url() + 'solicitacoes/reprovar/' + vm.dados.id,
                headers: { 'Authorization': 'Bearer ' + auth.token }
            })
            .success(function (data) {
                console.log(data);
                $ionicPopup.alert({ title: 'Sucesso!', template: 'Solicitação aprovada com sucesso.' });
            })
            .error(function (data) {
                console.log(data);
                $ionicPopup.alert({ title: 'Ops!', template: data[0].errorMessage });
            });
        }

        function base64(elemento) {
            if (!elemento.files || elemento.files.length < 1) return;
            var file = elemento.files[0];
            if (file.size > 512000) {
                elemento.files.splice(0, 1);
                $ionicPopup.alert({ title: 'Atenção!', template: 'Selecione uma imagem com menos de 500KB!' });
                return;
            }

            var reader = new FileReader();
            reader.onloadend = function() {
                $scope.$eval(elemento.name + '=' + '"' + reader.result + '"')
                $timeout(function () {});
            }
            reader.readAsDataURL(file);
            console.dir(file);
        }
    }
})();