using FluentValidation.Results;
using SME.NovaSondagem.Infra.Exceptions;
using System.Net;
using Xunit;
using Assert = Xunit.Assert;

namespace SME.NovaSondagem.Infra.Teste.Exceptions;

public class ExceptionsTeste
{
    [Fact]
    public void Deve_criar_negocio_exception_com_status_http()
    {
        var ex = new NegocioException("regra de negocio", HttpStatusCode.BadRequest);

        Assert.Equal("regra de negocio", ex.Message);
        Assert.Equal((int)HttpStatusCode.BadRequest, ex.StatusCode);
    }

    [Fact]
    public void Deve_criar_erro_exception_com_status_padrao()
    {
        var ex = new ErroException("erro interno");

        Assert.Equal("erro interno", ex.Message);
        Assert.Equal(500, ex.StatusCode);
    }

    [Fact]
    public void Deve_retornar_lista_de_mensagens_na_validacao_exception()
    {
        var erros = new List<ValidationFailure>
        {
            new("CampoA", "CampoA obrigatorio"),
            new("CampoB", "CampoB invalido")
        };
        var ex = new ValidacaoException(erros);

        var mensagens = ex.Mensagens();

        Assert.NotNull(mensagens);
        Assert.Equal(2, mensagens.Count);
        Assert.Contains("CampoA obrigatorio", mensagens);
        Assert.Contains("CampoB invalido", mensagens);
    }
}
