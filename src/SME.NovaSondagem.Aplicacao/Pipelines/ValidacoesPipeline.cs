using FluentValidation;
using MediatR;
using SME.NovaSondagem.Infra.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.NovaSondagem.Aplicacao.Pipelines
{
    public class ValidacoesPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> validadores;

        public ValidacoesPipeline(IEnumerable<IValidator<TRequest>> validadores)
        {
            this.validadores = validadores ?? throw new ArgumentNullException(nameof(validadores));
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (validadores.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var erros = validadores
                    .Select(v => v.Validate(context))
                    .SelectMany(result => result.Errors)
                    .Where(f => f != null)
                    .ToList();

                if (erros.Any())
                {
                    throw new ValidacaoException(erros);
                }
            }

            return await next();
        }
    }
}