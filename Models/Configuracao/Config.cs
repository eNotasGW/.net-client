using eNotasGW.Client.Lib.Exceptions;
using eNotasGW.Client.Lib.Models.Configuracao;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace eNotasGW.Client.Lib.Models.Configuracao
{
    public static class Config
    {
        /// <summary>
        /// Configura a Api Key da empresa.
        /// </summary>
        /// <param name="apiKey">Chave da Api.</param>
        public static void Configure(string apiKey)
        {
            try
            {
                var pathConfig = Path.Combine(Environment.CurrentDirectory, "config.json");
                var strJson = string.Empty;
                var config = new ConfiguracaoApi();

                if (!File.Exists(pathConfig))
                {
                    strJson = Utils.RetornarEmbeddedResourceJson();
                    config = JsonConvert.DeserializeObject<ConfiguracaoApi>(strJson);
                }
                else
                {
                    config = JsonConvert.DeserializeObject<ConfiguracaoApi>(System.IO.File.ReadAllText(pathConfig));
                }

                if (config != null)
                {
                    config.ApiKey = apiKey;

                    strJson = JsonConvert.SerializeObject(config, Formatting.Indented);

                    System.IO.File.WriteAllText(pathConfig, strJson);
                }
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message));
            }
        }

        /// <summary>
        /// Cria/Atualiza o arquivo de configuração pra Api.
        /// </summary>
        /// <param name="config">Objeto contendo os dados padrão para a Api. (Api Key, Versão, BaseEndPoint, DefaultContentType)</param>
        public static void Configure(ConfiguracaoApi config)
        {
            try
            {
                if(string.IsNullOrEmpty(config.ApiKey))
                    throw new Exception("Api Key não pode ser valor nulo ou em branco!");

                if (string.IsNullOrEmpty(config.BaseEndPoint))
                    throw new Exception("BaseEndPoint não pode ser valor nulo ou em branco!");

                if (string.IsNullOrEmpty(config.Versao))
                    throw new Exception("Versão não pode ser valor nulo ou em branco!");

                var pathConfig = Path.Combine(Environment.CurrentDirectory, "config.json");
                var strJson = JsonConvert.SerializeObject(config, Formatting.Indented);

                File.WriteAllText(pathConfig, strJson);
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message));
            }
        }

        /// <summary>
        /// Retorna dados das configurações da Api.
        /// </summary>
        /// <returns></returns>
        public static ConfiguracaoApi RetornarConfig()
        {
            try
            {
                var pathConfig = Path.Combine(Environment.CurrentDirectory, "config.json");

                return JsonConvert.DeserializeObject<ConfiguracaoApi>(System.IO.File.ReadAllText(pathConfig));
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message));
            }
        }
    }
}
