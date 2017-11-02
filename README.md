# eNotasGW C# Client

**Emissão de Nota Fiscal**

```
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
```                      
