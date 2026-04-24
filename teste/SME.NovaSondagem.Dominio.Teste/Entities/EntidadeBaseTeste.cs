using SME.NovaSondagem.Dominio.Entities;
using Xunit;
using Assert = Xunit.Assert;

namespace SME.NovaSondagem.Dominio.Teste.Entities;

public class EntidadeBaseTeste
{
    [Fact]
    public void Deve_permitir_atribuicao_do_id()
    {
        var entidade = new EntidadeFake { Id = 123 };

        Assert.Equal(123, entidade.Id);
    }

    private sealed class EntidadeFake : EntidadeBase
    {
    }
}
