using AutoMapper;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Csb.YerindeDonusum.Application.CQRS.KullaniciCQRS;

public class GetirKullaniciIdIleQuery : IRequest<ResultModel<KullaniciDto>>
{
	public long? KullaniciId { get; set; }

	public class GetirIdIleQueryHandler : IRequestHandler<GetirKullaniciIdIleQuery, ResultModel<KullaniciDto>>
	{
		private readonly IMapper _mapper;
		private readonly IKullaniciRepository _repository;

		public GetirIdIleQueryHandler(IMapper mapper, IKullaniciRepository repository)
		{
			_mapper = mapper;
			_repository = repository;
		}

		public async Task<ResultModel<KullaniciDto>> Handle(GetirKullaniciIdIleQuery request, CancellationToken cancellationToken)
		{
			var result = new ResultModel<KullaniciDto>();

			var kullanici = await _repository.GetWhere(x =>
				x.SistemKullanicisiMi != true //sistem kullanıcıları dışındakileri listeye getir
                &&
				x.KullaniciId == request.KullaniciId
				&&
				!x.SilindiMi,
					false,
					x => x.KullaniciRols.Where(y => !y.SilindiMi && y.AktifMi == true)
				).FirstOrDefaultAsync();

			if (kullanici != null)
				result.Result = _mapper.Map<KullaniciDto>(kullanici);
			else
				result.ErrorMessage("Kullanıcı bulunamadı!");

			return await Task.FromResult(result);
		}
	}
}