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
            console.log($state);
            var url = '';
            switch ($state.current.name) {
                case 'app.solicitacoes':
                    if (auth.participante)
                        url = 'solicitacoes/usuario/' + auth.id;
                    else
                        url = 'solicitacoes/pendentes'
                    break;
                case 'app.acompanha':
                    $ionicPopup.alert("Aguardando Controller");
                    return;
                case 'app.leiloei':
                    $ionicPopup.alert("Aguardando Controller");
                    return;
                case 'app.andamento':
                    $ionicPopup.alert("Aguardando Controller");
                    return;
                case 'app.destaque':
                    $ionicPopup.alert("Aguardando Controller");
                    return;
                default:
                    url = 'solicitacoes'
                    break;
            }

            if (!auth.done) {
                window.location.hash = '#/';
                return;
            }
            carregarDados(url);
        }

        function carregarDados(url) {
            if (!api.on()) {
                vm.dados = [
                    {
                        "id": 1,
                        "tempoLimiteLance": "03:00:25",
                        "loteId": 1,
                        "dataInicial": "0001-01-01T00:00:00",
                        "dataFinal": "2017-06-30T00:00:00",
                        "incrementoMinimo": 30,
                        "status": 0,
                        "usuarioId": "1ff83f28-8d42-4fb0-a976-c13344d70917",
                        "lote": {
                            "id": 1,
                            "produtos": [
                                {
                                    "id": 1,
                                    "nome": "Moto G",
                                    "descricao": "2nd Generation",
                                    "quantidade": 4,
                                    "imagem": null,
                                    "loteId": 1
                                }
                            ],
                            "valorMinimo": 30,
                            "vendedorId": "1ff83f28-8d42-4fb0-a976-c13344d70917",
                            "vendedor": null
                        },
                        "usuario": null
                    },
                    {
                        "id": 2,
                        "tempoLimiteLance": "03:00:25",
                        "loteId": 2,
                        "dataInicial": "0001-01-01T00:00:00",
                        "dataFinal": "2017-06-30T00:00:00",
                        "incrementoMinimo": 30,
                        "status": 0,
                        "usuarioId": "1ff83f28-8d42-4fb0-a976-c13344d70917",
                        "lote": {
                            "id": 2,
                            "produtos": [
                                {
                                    "id": 2,
                                    "nome": "iPonei",
                                    "descricao": "For Ass",
                                    "quantidade": 4,
                                    "imagem": null,
                                    "loteId": 2
                                }
                            ],
                            "valorMinimo": 30,
                            "vendedorId": "1ff83f28-8d42-4fb0-a976-c13344d70917",
                            "vendedor": null
                        },
                        "usuario": null
                    },
                    {
                        "id": 3,
                        "tempoLimiteLance": "03:00:25",
                        "loteId": 3,
                        "dataInicial": "0001-01-01T00:00:00",
                        "dataFinal": "0001-01-01T00:00:00",
                        "incrementoMinimo": 30,
                        "status": 0,
                        "usuarioId": "1ff83f28-8d42-4fb0-a976-c13344d70917",
                        "lote": {
                            "id": 3,
                            "produtos": [
                                {
                                    "id": 3,
                                    "nome": "test",
                                    "descricao": "For Dumbs",
                                    "quantidade": 4,
                                    "imagem": null,
                                    "loteId": 3
                                }
                            ],
                            "valorMinimo": 30,
                            "vendedorId": "1ff83f28-8d42-4fb0-a976-c13344d70917",
                            "vendedor": null
                        },
                        "usuario": null
                    },
                    {
                        "id": 4,
                        "tempoLimiteLance": "03:00:25",
                        "loteId": 4,
                        "dataInicial": "0001-01-01T00:00:00",
                        "dataFinal": "0001-01-01T00:00:00",
                        "incrementoMinimo": 30,
                        "status": 0,
                        "usuarioId": "1ff83f28-8d42-4fb0-a976-c13344d70917",
                        "lote": {
                            "id": 4,
                            "produtos": [
                                {
                                    "id": 4,
                                    "nome": "Moto G",
                                    "descricao": "2nd Generation",
                                    "quantidade": 4,
                                    "imagem": null,
                                    "loteId": 4
                                }
                            ],
                            "valorMinimo": 30,
                            "vendedorId": "1ff83f28-8d42-4fb0-a976-c13344d70917",
                            "vendedor": null
                        },
                        "usuario": null
                    }
                ];

                return;
            }

            $http({
                method: 'GET',
                url: api.url() + url,
                headers: { 'Authorization': 'Bearer ' + auth.data.tokenUsuario.tokenUsuario }
            }).success(function (data) {
                vm.dados = data;
            }).error(function (data) {
                console.log(data);
                $ionicPopup.alert({ title: 'Ops!', template: data[0].errorMessage });
            });
        }
    }
})();