using Csb.YerindeDonusum.Application.CQRS.TebligatCQRS.Commands.TebligatGonder;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Commands.TebligatGonder;

public class TebligatGonderCommandValidator : AbstractValidator<TebligatGonderCommandModel>
{
    public TebligatGonderCommandValidator()
    {

    }
}