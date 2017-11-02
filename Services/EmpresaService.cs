using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eNotasGW.Client.Lib.Models;
using eNotasGW.Client.Lib.Data;
using Newtonsoft.Json;
using System.Net.Http;
using eNotasGW.Client.Lib.Exceptions;
using System.Web.Http;
using System.Net;
using eNotasGW.Client.Lib.Models.Empresa;
using eNotasGW.Client.Lib.Models.Prefeitura;
using System.Net.Http.Headers;
using System.IO;
using eNotasGW.Client.Lib.Models.Configuracao;
using eNotasGW.Client.Lib.Exceptions.Base;
using eNotasGW.Client.Lib.Resources;

namespace eNotasGW.Client.Lib.Services
{
    public class EmpresaService : IDisposable
    {
        private HttpClient _client;
        private ConfiguracaoApi _config;

        public EmpresaService(ConfiguracaoApi config = null)
        {
            _config = config ?? eNotasGW.Client.Lib.Models.Configuracao.Config.RetornarConfig();

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Authorization", "Basic " + _config.ApiKey);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_config.DefaultContentType));
        }

        /// <summary>
        /// Insere ou atualiza uma empresa (CNPJ Emissor).
        /// </summary>
        /// <param name="empresa">Objeto Empresa.</param>
        /// <returns></returns>
        public async Task<DataEmpresaId> InserirAtualizarEmpresaAsync(Empresa empresa)
        {
            var strJson = JsonConvert.SerializeObject(empresa);
            var strContent = new StringContent(strJson, Encoding.UTF8, _config.DefaultContentType);

            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, string.Format("{0}/empresas", string.Concat(_config.BaseEndPoint, _config.Versao))))
                {
                    request.Content = strContent;

                    using (var response = await _client.SendAsync(request))
                    {
                        var resultContent = await response.Content.ReadAsStringAsync();
                        resultContent = resultContent.Replace(@"\""", "'");

                        if (response.IsSuccessStatusCode)
                        {
                            var model = JsonConvert.DeserializeObject<DataEmpresaId>(resultContent);

                            return model;
                        }
                        else
                        {
                            var messageException = ((int)response.StatusCode) + " - " + response.ReasonPhrase;
                            var dataResponse = JsonConvert.DeserializeObject<GWLibErro[]>(resultContent);

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
        /// Recupera uma lista de empresas por um critério de pesquisa.
        /// </summary>
        /// <param name="pageNumber">Número da página.</param>
        /// <param name="pageSize">Total de itens a serem retornados por página.</param>
        /// <param name="searchBy">Campo no qual será aplicado o termo de pesquisa. (nome_fantasia, razao_social)</param>
        /// <param name="searchTerm">Termo de pesquisa.</param>
        /// <param name="sortBy">Campo no qual será aplicado a ordenação. (nome_fantasia, razao_social, cidade, data_criacao, data_ultima_modificacao)</param>
        /// <param name="sortDirection">Direção na qual será feita a ordenação. (ASC, DESC)</param>
        /// <returns></returns>
        public async Task<DataEmpresaLista> ConsultarEmpresasAsync(int pageNumber, int pageSize, string searchBy = "", string searchTerm = "", string sortBy = "", string sortDirection = "")
        {
            try
            {
                var parameters = new StringBuilder(string.Format("/empresas?pageNumber={0}&pageSize={1}", pageNumber.ToString(), pageSize.ToString()));

                if (!string.IsNullOrEmpty(searchBy))
                    parameters.Append("&searchBy=" + searchBy);

                if (!string.IsNullOrEmpty(searchTerm))
                    parameters.Append("&searchTerm=" + searchTerm);

                if (!string.IsNullOrEmpty(sortBy))
                    parameters.Append("&sortBy=" + sortBy);

                if (!string.IsNullOrEmpty(sortDirection))
                    parameters.Append("&sortDirection=" + sortDirection);

                using (var request = new HttpRequestMessage(HttpMethod.Get, string.Format("{0}{1}", string.Concat(_config.BaseEndPoint, _config.Versao), parameters.ToString())))
                {
                    using (var response = await _client.SendAsync(request))
                    {
                        var resultContent = await response.Content.ReadAsStringAsync();
                        resultContent = resultContent.Replace(@"\""", "'");

                        if (response.IsSuccessStatusCode)
                        {
                            var model = JsonConvert.DeserializeObject<DataEmpresaLista>(resultContent);

                            return model;
                        }
                        else
                        {
                            var messageException = ((int)response.StatusCode) + " - " + response.ReasonPhrase;
                            var dataResponse = JsonConvert.DeserializeObject<GWLibErro[]>(resultContent);

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
        /// Recupera informações de uma Empresa (CNPJ Emissor).
        /// </summary>
        /// <param name="empresaId">Identificador único da empresa.</param>
        /// <returns></returns>
        public async Task<DataEmpresa> ConsultarEmpresaAsync(Guid empresaId)
        {
            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, string.Format("{0}/empresas/{1}", string.Concat(_config.BaseEndPoint, _config.Versao), empresaId)))
                {
                    using (var response = await _client.SendAsync(request))
                    {
                        var resultContent = await response.Content.ReadAsStringAsync();
                        resultContent = resultContent.Replace(@"\""", "'");

                        if (response.IsSuccessStatusCode)
                        {
                            var model = JsonConvert.DeserializeObject<DataEmpresa>(resultContent);

                            return model;
                        }
                        else
                        {
                            var messageException = ((int)response.StatusCode) + " - " + response.ReasonPhrase;
                            var dataResponse = JsonConvert.DeserializeObject<GWLibErro[]>(resultContent);
                           
                            if (response.IsSuccessStatusCode)
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
        /// Vincula um certificado digital a empresa.
        /// </summary>
        /// <param name="empresaId">Identificador único da empresa.</param>
        /// <param name="byCertificado">Array de bytes do certificado digital, .PFX ou .P12</param>
        /// <param name="senhaCertificado">Senha do certificado digital.</param>
        /// <returns></returns>
        public async Task<bool> UploadCertificado(Guid empresaId, byte[] byCertificado, string senhaCertificado)
        {
            try
            {
                var fileName = Guid.NewGuid().ToString() + ".pfx";
                var streamContent = new ByteArrayContent(byCertificado);
                streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { FileName = fileName, Name = "certificado" };

                var formData = new MultipartFormDataContent();
                formData.Add(new StringContent(senhaCertificado, Encoding.UTF8, _config.DefaultContentType), "senha");
                formData.Add(streamContent, "arquivo", fileName);

                using (var request = new HttpRequestMessage(HttpMethod.Post, string.Format("{0}/empresas/{1}/certificadoDigital", string.Concat(_config.BaseEndPoint, _config.Versao), empresaId)))
                {
                    request.Content = formData;

                    using (var response = await _client.SendAsync(request))
                    {
                        var resultContent = await response.Content.ReadAsStringAsync();
                        resultContent = resultContent.Replace(@"\""", "'");

                        if (response.IsSuccessStatusCode)
                        {
                            return true;
                        }
                        else
                        {
                            var messageException = ((int)response.StatusCode) + " - " + response.ReasonPhrase;
                            var dataResponse = JsonConvert.DeserializeObject<GWLibErro[]>(resultContent);

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
        /// Vincula um logotipo a empresa. O arquivo da imagem deve ser enviado usando codificação multipart/form-data. Formatos suportados: JPG, PNG e GIF
        /// </summary>
        /// <param name="empresaId">Identificador único da empresa.</param>
        /// <param name="logotipo">Array de bytes da imagem/logo da empresa.</param>
        /// <returns></returns>
        public async Task<bool> UploadLogo(Guid empresaId, byte[] logotipo)
        {
            try
            {
                var fileName = empresaId + ".png";
                var streamContent = new ByteArrayContent(logotipo);
                streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { FileName = fileName, Name = "logotipo" };

                var formData = new MultipartFormDataContent();
                formData.Add(streamContent, "logotipo", fileName);

                using (var request = new HttpRequestMessage(HttpMethod.Post, string.Format("{0}/empresas/{1}/logo", string.Concat(_config.BaseEndPoint, _config.Versao), empresaId)))
                {
                    request.Content = formData;

                    using (var response = await _client.SendAsync(request))
                    {
                        var resultContent = await response.Content.ReadAsStringAsync();
                        resultContent = resultContent.Replace(@"\""", "'");

                        if (response.IsSuccessStatusCode)
                        {
                            return true;
                        }
                        else
                        {
                            var messageException = ((int)response.StatusCode) + " - " + response.ReasonPhrase;
                            var dataResponse = JsonConvert.DeserializeObject<GWLibErro[]>(resultContent);
                            
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
