using eNotasGW.Client.Lib.Data;
using eNotasGW.Client.Lib.Exceptions;
using eNotasGW.Client.Lib.Models;
using eNotasGW.Client.Lib.Models.Configuracao;
using eNotasGW.Client.Lib.Models.Empresa;
using eNotasGW.Client.Lib.Models.NFe;
using eNotasGW.Client.Lib.Models.Prefeitura;
using eNotasGW.Client.Lib.Services;
using eNotasGW.Client.Lib.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eNotasGWTeste
{
    class Program
    {
        static string API_KEY = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
        static string EMPRESA_ID = "00000000-1111-2222-3333-999999999999";
        static string NFE_ID = "xxxxxxxx-aaaa-cccc-eeee-xxxxxxxxxxxx";

        static void Main(string[] args)
        {
            Console.WriteLine("eNotasGW");
            Console.WriteLine("Informe sua Api Key:");

            var apiKey = Console.ReadLine();

            if (string.IsNullOrEmpty(apiKey))
            {
                Console.WriteLine("Chave da api não informada, não é possível continuar!\n\nPressione qualquer tecla pra continuar...");
                Console.ReadKey();
                return;
            }

            if (!string.IsNullOrEmpty(apiKey))
            {
                Config.Configure(apiKey);
            }

            EmpresaService empService = new EmpresaService();
            NFeService nfeService = new NFeService();
            PrefeituraService prefService = new PrefeituraService();

            Console.WriteLine("");
            Console.WriteLine("Selecione uma opção:");
            Console.WriteLine("");
            Console.WriteLine("[0] - Consultar empresas");
            Console.WriteLine("");
            Console.WriteLine("[1] - Consultar empresa pelo Id");
            Console.WriteLine("");
            Console.WriteLine("[2] - Inserir/alterar empresa");
            Console.WriteLine("");
            Console.WriteLine("[3] - Emitir nota fiscal");
            Console.WriteLine("");
            Console.WriteLine("[4] - Cancelar nota fiscal");
            Console.WriteLine("");
            Console.WriteLine("[5] - Vincula um logotipo a empresa");
            Console.WriteLine("");
            Console.WriteLine("[6] - Configurar Api Key (somente a chave da Api)");
            Console.WriteLine("");
            Console.WriteLine("[7] - Configurar Api (dados padrão: Api Key, Versao, BaseEndPoint, DefaultContentType)");
            Console.WriteLine("");
            Console.WriteLine("[8] - Download do XML da nota fiscal");
            Console.WriteLine("");
            Console.WriteLine("[9] - Consultar nota fiscal");
            Console.WriteLine("");
            Console.WriteLine("[10] - Download PDF da nota fiscal");
            Console.WriteLine("");
            Console.WriteLine("[11] - Consulta serviços municipais");
            Console.WriteLine("");
            Console.WriteLine("[12] - Consulta serviços municipais unificados");
            Console.WriteLine();

            var opcao = Console.ReadLine();

            Console.WriteLine("");

            switch (opcao)
            {
                case "0":
                    Console.WriteLine("Consultando empresas...\n");

                    try
                    {
                        var dataEmpresas = empService.ConsultarEmpresasAsync(1, 5, "nome_fantasia", "", "nome_fantasia", "ASC").Result;

                        if (dataEmpresas != null)
                        {
                            Console.WriteLine("Empresas encontradas: " + dataEmpresas.totalRecords.ToString() + "\n\nPressione alguma tecla pra continuar...");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.InnerException.Message);
                    }

                    break;

                case "1":
                    Console.WriteLine("Consultando empresa...\n");

                    try
                    {
                        var dataEmpresa = empService.ConsultarEmpresaAsync(Guid.Parse(EMPRESA_ID)).Result;

                        if (dataEmpresa != null)
                        {
                            Console.WriteLine("Empresa Id: " + dataEmpresa.id + "\n\nPressione alguma tecla pra continuar...");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.InnerException.Message);
                    }

                    break;

                case "2":

                    Console.WriteLine("Incluindo/Alterando empresa...\n");

                    try
                    {
                        var empresa = new Empresa()
                        {
                            id = Guid.Parse(EMPRESA_ID), //informar apenas se quiser atualizar uma empresa existente, se for omitido cria uma nova empresa.
                            cnpj = "99999999999999",     //sem formatação
                            inscricaoMunicipal = "999999999",
                            inscricaoEstadual = null,
                            razaoSocial = "Razao Social",
                            nomeFantasia = "Nome Fantasia",
                            optanteSimplesNacional = true,
                            email = "email@email.com.br",
                            telefoneComercial = "99999999",
                            endereco = new eNotasGW.Client.Lib.Models.Empresa.Endereco()
                            {
                                pais = "Brasil",
                                uf = "MG",
                                codigoIbgeUf = 31,
                                cidade = "Belo Horizonte",
                                codigoIbgeCidade = 3106200,
                                logradouro = "R Teste",
                                numero = "999",
                                complemento = null,
                                bairro = "Bairro Teste",
                                cep = "85100000"
                            },
                            incentivadorCultural = false,
                            regimeEspecialTributacao = "6", //A lista de valores possíveis deve ser obtida pela api de caraterísticas da prefeitura
                            codigoServicoMunicipal = "010700188", //código do serviço municipal padrão para emissão de NFS-e
                            descricaoServico = "SUPORTE TECNICO EM INFORMATICA, INCLUSIVE INSTALACAO, CONFIGURACAO E MANUTENCAO DE PROGRAMAS DE COMPUTACAO E BANCOS DE DADOS", //Descrição do serviço municipal padrão para emissão de NFS-e (utilizado apenas na impressão da NFS-e)
                            aliquotaIss = 2,
                            ConfiguracoesNFSeProducao = new ConfiguracoesNFSeProducao()
                            {
                                sequencialNFe = 1,
                                serieNFe = "1",
                                sequencialLoteNFe = 1,
                                tokenAcessoProvedor = null,
                                usuarioAcessoProvedor = null
                            },
                            ConfiguracoesNFSeHomologacao = new ConfiguracoesNFSeHomologacao()
                            {
                                sequencialNFe = 1,
                                serieNFe = "1",
                                sequencialLoteNFe = 1,
                                tokenAcessoProvedor = null,
                                usuarioAcessoProvedor = null
                            },
                        };

                        //-----------------------------
                        //CARACTERÍSTICAS DA PREFEITURA
                        //-----------------------------
                        var caracteristicaPrefeitura = prefService.ConsultaCaracteristicasAsync(empresa.endereco.codigoIbgeCidade).Result;

                        if (caracteristicaPrefeitura.usaCNAE)
                            empresa.cnae = "123456";

                        if (caracteristicaPrefeitura.usaItemListaServico)
                            empresa.itemListaServicoLC116 = "1.07";

                        var configProd = empresa.ConfiguracoesNFSeProducao;
                        var configHomologa = empresa.ConfiguracoesNFSeHomologacao;

                        if (caracteristicaPrefeitura.tipoAutenticacao == (int)CaracteristicaPrefeitura.TipoAutenticacao.UsuarioESenha)
                        {
                            empresa.ConfiguracoesNFSeProducao.usuarioAcessoProvedor = "Usuario";
                            empresa.ConfiguracoesNFSeProducao.senhaAcessoProvedor = "Senha";

                            //opcional, preencher apenas se for emitir em ambiente de homologação
                            empresa.ConfiguracoesNFSeHomologacao.usuarioAcessoProvedor = "Usuario";
                            empresa.ConfiguracoesNFSeHomologacao.senhaAcessoProvedor = "Senha";
                        }
                        else if (caracteristicaPrefeitura.tipoAutenticacao == (int)CaracteristicaPrefeitura.TipoAutenticacao.Token)
                        {
                            empresa.ConfiguracoesNFSeProducao.tokenAcessoProvedor = "token";
                            empresa.ConfiguracoesNFSeHomologacao.tokenAcessoProvedor = "token";
                        }

                        //-----------------------------
                        //INCLUSÃO/ALTERAÇÃO DA EMPRESA
                        //-----------------------------
                        var dataEmpresaId = empService.InserirAtualizarEmpresaAsync(empresa).Result;

                        if (dataEmpresaId != null)
                        {
                            //-----------------------------------------
                            //ATUALIZAÇÃO/UPLOAD DO CERTIFICADO DIGITAL
                            //-----------------------------------------
                            if (caracteristicaPrefeitura.assinaturaDigital == (int)CaracteristicaPrefeitura.TipoAssinaturaDigital.Obrigatorio ||
                                caracteristicaPrefeitura.tipoAutenticacao == (int)CaracteristicaPrefeitura.TipoAutenticacao.Certificado)
                            {
                                var pathCertificado = @"C:\ENOTAS\MeuCertificado.pfx";

                                if (!System.IO.File.Exists(pathCertificado))
                                {
                                    Console.WriteLine("Certificado digital não encontrado!\nVerifique se o caminho do arquivo foi informado corretamente!\n\nPressione alguma tecla pra continuar...");
                                    Console.ReadKey();
                                    return;
                                }

                                var byCertificado = File.ReadAllBytes(pathCertificado);
                                var uploadCertificado = empService.UploadCertificado(Guid.Parse(dataEmpresaId.empresaId), byCertificado, "123").Result;

                                if (uploadCertificado)
                                {
                                    Console.WriteLine("Empresa incluída/alterada: " + dataEmpresaId.empresaId + "\n\nCertificado atualizado com sucesso!\n\nPressione alguma tecla pra continuar...");
                                    Console.ReadKey();
                                }
                            }

                            Console.WriteLine("Empresa incluída/alterada: " + dataEmpresaId.empresaId + "\n\nPressione alguma tecla pra continuar...");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.InnerException.Message);
                    }

                    break;

                case "3":

                    try
                    {
                        var nfe = new NFe()
                        {
                            idExterno = null, //opcional - "id de mapeamento com seu sistema"
                            ambienteEmissao = NFe.AmbienteEmissao.Homologacao,
                            cliente = new Cliente()
                            {
                                nome = "Cliente teste",
                                email = "cliente@email.com.br",
                                cpfCnpj = "11111111111",
                                endereco = new eNotasGW.Client.Lib.Models.NFe.Endereco()
                                {
                                    logradouro = "Rua 01",
                                    numero = "112",
                                    complemento = "AP 402",
                                    bairro = "Savassi",
                                    cep = "85100000",
                                    uf = "MG",
                                    cidade = "Belo Horizonte"
                                }
                            },
                            servico = new eNotasGW.Client.Lib.Models.NFe.Servico()
                            {
                                descricao = "Discriminação do Serviço prestado",
                                valorPis = 0,
                                valorCofins = 0,
                                valorCsll = 0,
                                valorInss = 0,
                                valorIr = 0
                            },
                            valorTotal = 10
                        };

                        var dataNfeId = nfeService.EmitirNFAsync(Guid.Parse(EMPRESA_ID), nfe).Result;

                        if (dataNfeId != null)
                        {
                            Console.WriteLine("NFS-e incluída: " + dataNfeId.nfeId + "\n\nPressione alguma tecla pra continuar...");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.InnerException.Message);
                    }

                    break;

                case "4":

                    try
                    {
                        var dataNfeId = nfeService.CancelarNFAsync(Guid.Parse(EMPRESA_ID), Guid.Parse(NFE_ID)).Result;

                        if (dataNfeId != null)
                        {
                            Console.WriteLine("NFS-e cancelada com suceso: " + dataNfeId.nfeId + "\n\nPressione alguma tecla pra continuar...");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.InnerException.Message);
                    }

                    break;

                case "5":

                    try
                    {
                        var pathLogo = @"C:\ENOTAS\logo.png";

                        if (!System.IO.File.Exists(pathLogo))
                        {
                            Console.WriteLine("Arquivo não encontrado!\nVerifique se o caminho do arquivo foi informado corretamente!\n\nPressione alguma tecla pra continuar...");
                            Console.ReadKey();
                            return;
                        }

                        var byLogo = File.ReadAllBytes(pathLogo);
                        var dataLogo = empService.UploadLogo(Guid.Parse(EMPRESA_ID), byLogo).Result;

                        if (dataLogo)
                        {
                            Console.WriteLine("Logotipo vinculada a empresa com sucesso!\n\nPressione alguma tecla pra continuar...");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.InnerException.Message);
                    }

                    break;

                case "6":

                    try
                    {
                        Config.Configure(API_KEY);

                        Console.WriteLine("Chave da Api configurada com sucesso!\n\nPressione alguma tecla pra continuar...");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.InnerException.Message);
                    }

                    break;

                case "7":

                    try
                    {
                        var config = new ConfiguracaoApi()
                        {
                            ApiKey = API_KEY,
                            Versao = "1",
                            BaseEndPoint = "https://api.enotasgw.com.br/v",
                            DefaultContentType = "application/json"
                        };

                        Config.Configure(config);

                        Console.WriteLine("Configuração efetuada com sucesso!\n\nPressione alguma tecla pra continuar...");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.InnerException.Message);
                    }

                    break;

                case "8":

                    try
                    {
                        var xml = nfeService.DownloadXMLAsync(Guid.Parse(EMPRESA_ID), Guid.Parse(NFE_ID)).Result;

                        if (!string.IsNullOrEmpty(xml))
                        {
                            Console.WriteLine("XML: \n" + xml + "\n\nPressione qualquer tecla pra continuar...");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.InnerException.Message);
                    }

                    break;

                case "9":

                    try
                    {
                        var dataNFe = nfeService.ConsultarNFAsync(Guid.Parse(EMPRESA_ID), Guid.Parse(NFE_ID)).Result;

                        if (dataNFe != null)
                        {
                            Console.WriteLine("NF Id: \n" + dataNFe.id + "\n\nPressione qualquer tecla pra continuar...");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.InnerException.Message);
                    }

                    break;

                case "10":

                    try
                    {
                        var pathPDF = @"C:\ENOTAS\";

                        if (!System.IO.Directory.Exists(pathPDF))
                        {
                            Console.WriteLine("Diretório não encontrado: " + pathPDF + "\n\nPressione alguma tecla pra continuar...");
                            Console.ReadKey();
                            return;
                        }

                        pathPDF = Path.Combine(@"C:\ENOTAS\", string.Concat(NFE_ID, ".pdf"));
                        var dataPDF = nfeService.DownloadPDFAsync(Guid.Parse(EMPRESA_ID), Guid.Parse(NFE_ID)).Result;

                        if (dataPDF != null)
                        {
                            File.WriteAllBytes(pathPDF, dataPDF);
                            Console.WriteLine("PDF salvo em: \n" + pathPDF + "\n\nPressione qualquer tecla pra continuar...");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.InnerException.Message);
                    }

                    break;

                case "11":

                    try
                    {
                        var dataServicos = prefService.ConsultaServicoMunicipalAsync("sp", "são paulo", 1, 5, "cons").Result;

                        if (dataServicos != null)
                        {
                            Console.WriteLine("Total de registros: " + dataServicos.totalRecords.ToString() + "\n\nPressione qualquer tecla pra continuar...");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.InnerException.Message);
                    }

                    break;

                case "12":

                    try
                    {
                        var dataServicos = prefService.ConsultaServicoMunicipalUnificadoAsync(1, 5).Result;

                        if (dataServicos != null)
                        {
                            Console.WriteLine("Total de registros: " + dataServicos.totalRecords.ToString() + "\n\nPressione qualquer tecla pra continuar...");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.InnerException.Message);
                    }

                    break;
            }

            Console.ReadKey();
        }
    }
}
