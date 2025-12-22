using SME.NovaSondagem.Worker.Dtos;
using SME.NovaSondagem.Worker.Dtos.Eol;
using SME.NovaSondagem.Worker.Interfaces.Repositories;
using SME.NovaSondagem.Worker.Interfaces.UseCases;
using SME.NovaSondagem.Worker.UseCases.ComponenteCurricular.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.NovaSondagem.Worker.UseCases.ComponenteCurricular
{
    public class BuscarComponenteCurricularUseCase : IBuscarComponenteCurricularUseCase, IRabbitUseCase
    {
        private readonly IComponenteCurricularRepository _repository;
        private const int TAMANHO_LOTE = 100;

        public BuscarComponenteCurricularUseCase(IComponenteCurricularRepository repository)
        {
            _repository = repository;
        }

        public async Task ExecutarAsync(BuscarComponenteCurricularRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var componentesEol = await _repository.ObterComponentesEolAtivosAsync();
            var loteParaInserir = new List<SME.NovaSondagem.Worker.Dtos.ComponenteCurricular>();
            var contador = 0;

            foreach (var componenteEol in componentesEol)
            {
                var componenteExistente = await _repository.ObterPorCodigoEolAsync(componenteEol.CdComponenteCurricular);

                if (componenteExistente == null)
                {
                    loteParaInserir.Add(MapearParaComponenteCurricular(componenteEol, request.UsuarioOperacao));
                }
                else if (RequerAtualizacao(componenteExistente, componenteEol))
                {
                    AtualizarComponente(componenteExistente, componenteEol, request.UsuarioOperacao);
                    await _repository.AtualizarAsync(componenteExistente);
                }

                contador++;

                if (contador % TAMANHO_LOTE == 0)
                {
                    await ProcessarLote(loteParaInserir);
                }
            }

            await ProcessarLote(loteParaInserir);
        }

        public async Task Executar(MensagemRabbit mensagem)
        {
            var request = mensagem.ObterObjetoMensagem<BuscarComponenteCurricularRequest>();
            await ExecutarAsync(request);
        }

        private async Task ProcessarLote(List<SME.NovaSondagem.Worker.Dtos.ComponenteCurricular> lote)
        {
            if (lote.Count > 0)
            {
                await _repository.InserirEmMassaAsync(lote);
                await _repository.SalvarAsync();
                lote.Clear();
            }
        }

        private static SME.NovaSondagem.Worker.Dtos.ComponenteCurricular MapearParaComponenteCurricular(
            ComponenteCurricularEol origem,
            string? usuario)
        {
            return new SME.NovaSondagem.Worker.Dtos.ComponenteCurricular
            {
                CodigoEol = origem.CdComponenteCurricular,
                Descricao = origem.DcComponenteCurricular,
                Polivalencia = origem.InPolivalencia,
                ProgramaDisciplina = origem.InProgramaDisciplina,
                CodigoDisciplinaMec = origem.CdDisciplinaMec,
                CodigoComponentePrincipal = origem.CdComponenteCurricularPrincipal,
                CodigoComponenteEquivalenteQuadro = origem.CdComponenteEquivalenteQuadro,
                SpIntegral = origem.InSpIntegral,
                Ativo = true,
                DataCriacao = DateTime.UtcNow,
                CriadoPor = usuario
            };
        }

        private static bool RequerAtualizacao(
            SME.NovaSondagem.Worker.Dtos.ComponenteCurricular destino,
            ComponenteCurricularEol origem)
        {
            return destino.Descricao != origem.DcComponenteCurricular ||
                   destino.Polivalencia != origem.InPolivalencia ||
                   destino.ProgramaDisciplina != origem.InProgramaDisciplina ||
                   destino.CodigoDisciplinaMec != origem.CdDisciplinaMec ||
                   destino.CodigoComponentePrincipal != origem.CdComponenteCurricularPrincipal ||
                   destino.CodigoComponenteEquivalenteQuadro != origem.CdComponenteEquivalenteQuadro ||
                   destino.SpIntegral != origem.InSpIntegral;
        }

        private static void AtualizarComponente(
            SME.NovaSondagem.Worker.Dtos.ComponenteCurricular destino,
            ComponenteCurricularEol origem,
            string? usuario)
        {
            destino.Descricao = origem.DcComponenteCurricular;
            destino.Polivalencia = origem.InPolivalencia;
            destino.ProgramaDisciplina = origem.InProgramaDisciplina;
            destino.CodigoDisciplinaMec = origem.CdDisciplinaMec;
            destino.CodigoComponentePrincipal = origem.CdComponenteCurricularPrincipal;
            destino.CodigoComponenteEquivalenteQuadro = origem.CdComponenteEquivalenteQuadro;
            destino.SpIntegral = origem.InSpIntegral;
            destino.DataAtualizacao = DateTime.UtcNow;
            destino.AlteradoPor = usuario;
        }
    }
}