﻿using eNotasGW.Client.Lib.Data;
using eNotasGW.Client.Lib.Exceptions;
using eNotasGW.Client.Lib.Models;
using eNotasGW.Client.Lib.Models.NFe;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using eNotasGW.Client.Lib.Helpers;
using System.Net.Http.Headers;
using eNotasGW.Client.Lib.Models.Configuracao;
using eNotasGW.Client.Lib.Exceptions.Base;
using eNotasGW.Client.Lib.Resources;
using System.Net;

namespace eNotasGW.Client.Lib.Services
{
    public class NFeService : IDisposable
    {
        private HttpClient _client;
        private ConfiguracaoApi _config;

        public NFeService(ConfiguracaoApi config = null)
        {
            _config = config ?? Config.RetornarConfig();

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Authorization", "Basic " + _config.ApiKey);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_config.DefaultContentType));
        }

        /// <summary>
        /// Emite uma Nota Fiscal.
        /// </summary>
        /// <param name="empresaId">Identificador único da empresa.</param>
        /// <param name="nfe">Objeto Nota fiscal.</param>
        /// <returns></returns>
        public async Task<DataNFeId> EmitirNFAsync(Guid empresaId, NFe nfe)
        {
            try
            {
                var strJson = JsonConvert.SerializeObject(nfe);
                var strContent = new StringContent(strJson, Encoding.UTF8, _config.DefaultContentType);

                using (var request = new HttpRequestMessage(HttpMethod.Post, string.Format("{0}/empresas/{1}/nfes", string.Concat(_config.BaseEndPoint, _config.Versao), empresaId)))
                {
                    request.Content = strContent;

                    using (var response = await _client.SendAsync(request))
                    {
                        var resultContent = await response.Content.ReadAsStringAsync();
                        resultContent = resultContent.Replace(@"\""", "'");

                        if (response.IsSuccessStatusCode)
                        {
                            var model = JsonConvert.DeserializeObject<DataNFeId>(resultContent);

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
        /// Cancela uma Nota Fiscal identificada pelo seu identificador único (paramêtro {nfeId})
        /// </summary>
        /// <param name="empresaId">Identificador único da empresa.</param>
        /// <param name="nfeId">Identificador único da Nota Fiscal que deseja cancelar.</param>
        /// <returns></returns>
        public async Task<DataNFeId> CancelarNFAsync(Guid empresaId, Guid nfeId)
        {
            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Delete, string.Format("{0}/empresas/{1}/nfes/{2}", string.Concat(_config.BaseEndPoint, _config.Versao), empresaId, nfeId)))
                {
                    using (var response = await _client.SendAsync(request))
                    {
                        var resultContent = await response.Content.ReadAsStringAsync();
                        resultContent = resultContent.Replace(@"\""", "'");

                        if (response.IsSuccessStatusCode)
                        {
                            var model = JsonConvert.DeserializeObject<DataNFeId>(resultContent);

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
        /// Recupera informações da Nota Fiscal pelo seu Identificado único (paramêtro {nfeId}). O retorno inclui os dados da Nota Fiscal requisitada.
        /// </summary>
        /// <param name="empresaId">Identificador único da Empresa.</param>
        /// <param name="nfeId">Identificador único da Nota Fiscal que deseja requisitar.</param>
        /// <returns></returns>
        public async Task<DataNFe> ConsultarNFAsync(Guid empresaId, Guid nfeId)
        {
            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, string.Format("{0}/empresas/{1}/nfes/{2}", string.Concat(_config.BaseEndPoint, _config.Versao), empresaId, nfeId)))
                {
                    using (var response = await _client.SendAsync(request))
                    {
                        var resultContent = await response.Content.ReadAsStringAsync();
                        resultContent = resultContent.Replace(@"\""", "'");

                        if (response.IsSuccessStatusCode)
                        {
                            return JsonConvert.DeserializeObject<DataNFe>(resultContent);
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
        /// Download do XML da Nota Fiscal. O retorno inclui uma string com o conteúdo XML da Nota Fiscal requisitada.
        /// </summary>
        /// <param name="empresaId">Identificador único da Empresa.</param>
        /// <param name="nfeId">Identificador único da Nota Fiscal. Exemplo: dac99999-cdcd-9999-99c9-9999e9999999</param>
        /// <returns></returns>
        public async Task<string> DownloadXMLAsync(Guid empresaId, Guid nfeId)
        {
            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, string.Format("{0}/empresas/{1}/nfes/{2}/xml", string.Concat(_config.BaseEndPoint, _config.Versao), empresaId, nfeId)))
                {
                    using (var response = await _client.SendAsync(request))
                    {
                        var resultContent = await response.Content.ReadAsStringAsync();
                        resultContent = resultContent.Replace(@"\""", "'");

                        if (response.IsSuccessStatusCode)
                        {
                            return resultContent;
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
        /// Download do PDF de impressão da Nota Fiscal identificada pelo seu Identificador único (paramêtro {nfeId}). O retorno inclui os bytes do PDF da Nota Fiscal requisitada.
        /// </summary>
        /// <param name="empresaId">Identificador único da Empresa.</param>
        /// <param name="nfeId">Identificador único da Nota Fiscal que deseja requisitar.</param>
        /// <returns></returns>
        public async Task<byte[]> DownloadPDFAsync(Guid empresaId, Guid nfeId)
        {
            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, string.Format("{0}/empresas/{1}/nfes/{2}/pdf", string.Concat(_config.BaseEndPoint, _config.Versao), empresaId, nfeId)))
                {
                    using (var response = await _client.SendAsync(request))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var byPDF = await response.Content.ReadAsByteArrayAsync();
                            return byPDF;
                        }
                        else
                        {
                            var resultContent = await response.Content.ReadAsStringAsync();
                            resultContent = resultContent.Replace(@"\""", "'");

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
        /// Recupera uma lista de Notas Fiscais por um critério de pesquisa
        /// </summary>
        /// <param name="empresaId">Identificador único da Empresa</param>
        /// <param name="pageNumber">Número da página, sendo 0 (zero) o número da primeira página</param>
        /// <param name="pageSize">Total de itens a serem retornados por página</param>
        /// <param name="filter">Filtro para realização da pesquisa. Possíveis campos: Status. Exemplos de filtro: status eq 'Autorizada'</param>
        /// <param name="sortBy">Campo no qual será aplicado a ordenação</param>
        /// <param name="sortDirection">Direção na qual será feita a ordenação</param>
        /// <returns></returns>
        public async Task<DataNFeLista> ConsultarNFsAsync(Guid empresaId, int pageNumber, int pageSize, string filter, string sortBy = "", string sortDirection = "")
        {
            try
            {
                var parameters = new StringBuilder(string.Format("{0}/empresas/{1}/nfes?pageNumber={2}&pageSize={3}&filter={4}", string.Concat(_config.BaseEndPoint, _config.Versao), empresaId, pageNumber.ToString(), pageSize.ToString(), filter));

                if (!string.IsNullOrEmpty(sortBy))
                    parameters.Append("&sortBy=" + sortBy);

                if (!string.IsNullOrEmpty(sortDirection))
                    parameters.Append("&sortDirection=" + sortDirection);

                using (var request = new HttpRequestMessage(HttpMethod.Get, parameters.ToString()))
                {
                    using (var response = await _client.SendAsync(request))
                    {
                        var resultContent = await response.Content.ReadAsStringAsync();
                        resultContent = resultContent.Replace(@"\""", "'");

                        if (response.IsSuccessStatusCode)
                        {
                            return JsonConvert.DeserializeObject<DataNFeLista>(resultContent);
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
        /// Recupera informações da Nota Fiscal pelo seu Identificador externo (paramêtro {idExterno})
        /// </summary>
        /// <param name="empresaId">Identificador único da Empresa</param>
        /// <param name="idExterno">Identificador único externo da Nota Fiscal que deseja requisitar</param>
        /// <returns></returns>
        public async Task<DataNFe> ConsultarNFporIdExternoAsync(Guid empresaId, string idExterno)
        {
            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, string.Format("{0}/empresas/{1}/nfes/porIdExterno/{2}", string.Concat(_config.BaseEndPoint, _config.Versao), empresaId, idExterno)))
                {
                    using (var response = await _client.SendAsync(request))
                    {
                        var resultContent = await response.Content.ReadAsStringAsync();
                        resultContent = resultContent.Replace(@"\""", "'");

                        if (response.IsSuccessStatusCode)
                        {
                            return JsonConvert.DeserializeObject<DataNFe>(resultContent);
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
        /// Download do XML da Nota Fiscal identificada pelo seu Identificador externo (paramêtro {idExterno}).
        /// </summary>
        /// <param name="empresaId">Identificador único da Empresa</param>
        /// <param name="idExterno">Identificador único externo da Nota Fiscal que deseja requisitar.</param>
        /// <returns></returns>
        public async Task<string> DownloadXMLporIdExternoAsync(Guid empresaId, string idExterno)
        {
            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, string.Format("{0}/empresas/{1}/nfes/porIdExterno/{2}/xml", string.Concat(_config.BaseEndPoint, _config.Versao), empresaId, idExterno)))
                {
                    using (var response = await _client.SendAsync(request))
                    {
                        var resultContent = await response.Content.ReadAsStringAsync();
                        resultContent = resultContent.Replace(@"\""", "'");

                        if (response.IsSuccessStatusCode)
                        {
                            return resultContent;
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
        /// Download do PDF de impressão da Nota Fiscal identificada pelo seu identificador externo (paramêtro {idExterno}).
        /// </summary>
        /// <param name="empresaId">Identificador único da Empresa</param>
        /// <param name="idExterno">Identificador único externo da Nota Fiscal que deseja requisitar.</param>
        /// <returns></returns>
        public async Task<byte[]> DownloadPDFporIdExternoAsync(Guid empresaId, string idExterno)
        {
            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, string.Format("{0}/empresas/{1}/nfes/porIdExterno/{2}/pdf", string.Concat(_config.BaseEndPoint, _config.Versao), empresaId, idExterno)))
                {
                    using (var response = await _client.SendAsync(request))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var bytesPDF = await response.Content.ReadAsByteArrayAsync();
                            return bytesPDF;
                        }
                        else
                        {
                            var resultContent = await response.Content.ReadAsStringAsync();
                            resultContent = resultContent.Replace(@"\""", "'");

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
        /// Cancela uma Nota Fiscal identificada pelo seu ID Externo
        /// </summary>
        /// <param name="empresaId">Identificador único da Empresa</param>
        /// <param name="idExterno">Identificador único da Nota Fiscal que deseja cancelar</param>
        /// <returns></returns>
        public async Task<DataNFeId> CancelarNFporIdExternoAsync(Guid empresaId, string idExterno)
        {
            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Delete, string.Format("{0}/empresas/{1}/nfes/porIdExterno/{2}", string.Concat(_config.BaseEndPoint, _config.Versao), empresaId, idExterno)))
                {
                    using (var response = await _client.SendAsync(request))
                    {
                        var resultContent = await response.Content.ReadAsStringAsync();
                        resultContent = resultContent.Replace(@"\""", "'");

                        if (response.IsSuccessStatusCode)
                        {
                            var model = JsonConvert.DeserializeObject<DataNFeId>(resultContent);

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
