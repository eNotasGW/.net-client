using eNotasGW.Client.Lib.Data;
using eNotasGW.Client.Lib.Exceptions;
using eNotasGW.Client.Lib.Exceptions.Base;
using eNotasGW.Client.Lib.Models.Configuracao;
using eNotasGW.Client.Lib.Models.Prefeitura;
using eNotasGW.Client.Lib.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace eNotasGW.Client.Lib.Services
{
    public class PrefeituraService : IDisposable
    {
        private HttpClient _client;
        private ConfiguracaoApi _config;

        public PrefeituraService(ConfiguracaoApi config = null)
        {
            _config = config ?? Config.RetornarConfig();

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Authorization", "Basic " + _config.ApiKey);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_config.DefaultContentType));
        }

        /// <summary>
        /// Consulta as características de uma determinada prefeitura.
        /// </summary>
        /// <param name="codigoIBGECidade">Código do ibge da cidade cuja a prefeitura será consultada.</param>
        /// <returns></returns>
        public async Task<CaracteristicaPrefeitura> ConsultaCaracteristicasAsync(int codigoIBGECidade)
        {
            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, string.Format("{0}/estados/cidades/{1}/provedor", string.Concat(_config.BaseEndPoint, _config.Versao), codigoIBGECidade.ToString())))
                {
                    using (var response = await _client.SendAsync(request))
                    {
                        var resultContent = await response.Content.ReadAsStringAsync();
                        resultContent = resultContent.Replace(@"\""", "'");

                        if (response.IsSuccessStatusCode)
                        {
                            var model = JsonConvert.DeserializeObject<CaracteristicaPrefeitura>(resultContent);

                            return model;
                        }
                        else
                        {
                            var messageException = new StringBuilder(((int)response.StatusCode) + " - " + response.ReasonPhrase);

                            var dataResponse = JsonConvert.DeserializeObject<GWLibErro[]>(resultContent);
                            if (dataResponse != null)
                            {
                                foreach (var message in dataResponse)
                                {
                                    messageException.AppendLine(message.Codigo + " - " + message.Mensagem);
                                }
                            }

                            if (response.StatusCode == HttpStatusCode.BadRequest)
                            {
                                throw new GWLibValidationException(messageException.ToString(), dataResponse);
                            }
                            else if (response.StatusCode == HttpStatusCode.Forbidden)
                            {
                                throw new GWLibAuthorizationException(messageException.ToString(), dataResponse);
                            }
                            else if (response.StatusCode == HttpStatusCode.Unauthorized)
                            {
                                throw new GWLibAuthenticationException(messageException.ToString(), dataResponse);
                            }
                            else
                            {
                                throw new GWLibGeneralException(messageException.ToString(), dataResponse);
                            }
                        }
                    }
                }
            }
            catch (GWLibException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw (new GWLibGeneralException(GWLibMessages.GWLibClientInternalError, ex));
            }
        }

        /// <summary>
        /// Consulta a relação de serviços municipais da prefeitura.
        /// </summary>
        /// <param name="uf">Sigla do estado. Exemplo: SP</param>
        /// <param name="nomeCidade">Nome da cidade. Exemplo: São Paulo</param>
        /// <param name="pageNumber">Número da página, sendo 0 o número da primeira página.</param>
        /// <param name="pageSize">Total de itens a serem retornados por página.</param>
        /// <param name="descricaoServico">Filtro para realização da pesquisa. Possíveis campos: descricao. Exemplo: contains(descricao, 'Consultoria')</param>
        /// <returns></returns>
        public async Task<DataServicoMunicipalLista> ConsultaServicoMunicipalAsync(string uf, string nomeCidade, int pageNumber, int pageSize, string descricaoServico = "")
        {
            try
            {
                var parameters = new StringBuilder(string.Format("/estados/{0}/cidades/{1}/servicos?pageNumber={2}&pageSize={3}", uf, nomeCidade, pageNumber.ToString(), pageSize.ToString()));

                if (!string.IsNullOrEmpty(descricaoServico))
                    parameters.Append("&filter=contains(descricao,'" + descricaoServico + "')");

                using (var request = new HttpRequestMessage(HttpMethod.Get, string.Format("{0}{1}", string.Concat(_config.BaseEndPoint, _config.Versao), parameters.ToString())))
                {
                    using (var response = await _client.SendAsync(request))
                    {
                        var resultContent = await response.Content.ReadAsStringAsync();
                        resultContent = resultContent.Replace(@"\""", "'");

                        if (response.IsSuccessStatusCode)
                        {
                            var model = JsonConvert.DeserializeObject<DataServicoMunicipalLista>(resultContent);

                            return model;
                        }
                        else
                        {
                            var messageException = new StringBuilder(((int)response.StatusCode) + " - " + response.ReasonPhrase);

                            var dataResponse = JsonConvert.DeserializeObject<GWLibErro[]>(resultContent);
                            if (dataResponse != null)
                            {
                                foreach (var message in dataResponse)
                                {
                                    messageException.AppendLine(message.Codigo + " - " + message.Mensagem);
                                }
                            }

                            if (response.StatusCode == HttpStatusCode.BadRequest)
                            {
                                throw new GWLibValidationException(messageException.ToString(), dataResponse);
                            }
                            else if (response.StatusCode == HttpStatusCode.Forbidden)
                            {
                                throw new GWLibAuthorizationException(messageException.ToString(), dataResponse);
                            }
                            else if (response.StatusCode == HttpStatusCode.Unauthorized)
                            {
                                throw new GWLibAuthenticationException(messageException.ToString(), dataResponse);
                            }
                            else
                            {
                                throw new GWLibGeneralException(messageException.ToString(), dataResponse);
                            }
                        }
                    }
                }
            }
            catch (GWLibException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw (new GWLibGeneralException(GWLibMessages.GWLibClientInternalError, ex));
            }
        }

        /// <summary>
        /// Consulta a relação de cidades que já possuem serviço municipal unificado.
        /// </summary>
        /// <param name="pageNumber">Número da página, sendo 0 o número da primeira página.</param>
        /// <param name="pageSize">Total de itens a serem retornados por página.</param>
        /// <returns></returns>
        public async Task<DataServicoMunicipalUnificadoLista> ConsultaCidadesServicoMunicipalUnificadoAsync(int pageNumber, int pageSize)
        {
            try
            {
                var parameters = string.Format("/servicos/cidades?pageNumber={0}&pageSize={1}", pageNumber.ToString(), pageSize.ToString());

                using (var request = new HttpRequestMessage(HttpMethod.Get, string.Format("{0}{1}", string.Concat(_config.BaseEndPoint, _config.Versao), parameters)))
                {
                    using (var response = await _client.SendAsync(request))
                    {
                        var resultContent = await response.Content.ReadAsStringAsync();
                        resultContent = resultContent.Replace(@"\""", "'");

                        if (response.IsSuccessStatusCode)
                        {
                            var model = JsonConvert.DeserializeObject<DataServicoMunicipalUnificadoLista>(resultContent);

                            return model;
                        }
                        else
                        {
                            var messageException = new StringBuilder(((int)response.StatusCode) + " - " + response.ReasonPhrase);

                            var dataResponse = JsonConvert.DeserializeObject<GWLibErro[]>(resultContent);
                            if (dataResponse != null)
                            {
                                foreach (var message in dataResponse)
                                {
                                    messageException.AppendLine(message.Codigo + " - " + message.Mensagem);
                                }
                            }

                            if (response.StatusCode == HttpStatusCode.BadRequest)
                            {
                                throw new GWLibValidationException(messageException.ToString(), dataResponse);
                            }
                            else if (response.StatusCode == HttpStatusCode.Forbidden)
                            {
                                throw new GWLibAuthorizationException(messageException.ToString(), dataResponse);
                            }
                            else if (response.StatusCode == HttpStatusCode.Unauthorized)
                            {
                                throw new GWLibAuthenticationException(messageException.ToString(), dataResponse);
                            }
                            else
                            {
                                throw new GWLibGeneralException(messageException.ToString(), dataResponse);
                            }
                        }
                    }
                }
            }
            catch (GWLibException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw (new GWLibGeneralException(GWLibMessages.GWLibClientInternalError, ex));
            }
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
