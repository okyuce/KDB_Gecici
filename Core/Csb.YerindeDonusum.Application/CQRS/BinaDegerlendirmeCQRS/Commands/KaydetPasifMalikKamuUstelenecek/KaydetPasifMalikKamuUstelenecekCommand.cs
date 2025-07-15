using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.BasvuruImzaCQRS.Commands.KaydetBasvuruImzaVeren;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.KaydetPasifMalikKamuUstelenecek;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Commands.KaydetPasifMalikKamuUstelenecek;

public class KaydetPasifMalikKamuUstelenecekCommand : IRequest<ResultModel<KaydetPasifMalikKamuUstelenecekCommandResponseModel>>
{
    public long? BinaDegerlendirmeId { get; set; }
    public List<KaydetPasifMalikKamuUstelenecekCommandRequestModel> BasvuruKamuUstlenecekList { get; set; }

    public class KaydetPasifMalikKamuUstelenecekCommandHandler : IRequestHandler<KaydetPasifMalikKamuUstelenecekCommand, ResultModel<KaydetPasifMalikKamuUstelenecekCommandResponseModel>>
    {
        private readonly IMapper _mapper;
        private readonly IBasvuruKamuUstlenecekRepository _basvuruKamuUstlenecekRepository;
        private readonly IBinaDegerlendirmeRepository _binaDegerlendirmeRepository;
        private readonly IMediator _mediator;
        private readonly IKullaniciBilgi _kullaniciBilgi;

        public KaydetPasifMalikKamuUstelenecekCommandHandler(IBasvuruKamuUstlenecekRepository basvuruKamuUstlenecekRepository, IMapper mapper, IMediator mediator, IKullaniciBilgi kullaniciBilgi, IBinaDegerlendirmeRepository binaDegerlendirmeRepository)
        {
            _basvuruKamuUstlenecekRepository = basvuruKamuUstlenecekRepository;
            _mapper = mapper;
            _mediator = mediator;
            _kullaniciBilgi = kullaniciBilgi;
            _binaDegerlendirmeRepository = binaDegerlendirmeRepository;
        }

        public async Task<ResultModel<KaydetPasifMalikKamuUstelenecekCommandResponseModel>> Handle(KaydetPasifMalikKamuUstelenecekCommand request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<KaydetPasifMalikKamuUstelenecekCommandResponseModel>();
            var userInfo = _kullaniciBilgi.GetUserInfo();
            long.TryParse(userInfo.KullaniciId, out long kullaniciId);

            if (!request.BasvuruKamuUstlenecekList.Any())
            {
                result.ErrorMessage("Kamu üstlenecek olarak eklenecek pasif malik bulunamadı!");
                return await Task.FromResult(result);
            }
            var binaDegerlendirme = _binaDegerlendirmeRepository.GetWhere(x => x.BinaDegerlendirmeId == request.BinaDegerlendirmeId).FirstOrDefault();
            var kamuUstlenecekListesi = _mapper.Map<List<BasvuruKamuUstlenecek>>(request.BasvuruKamuUstlenecekList);
            kamuUstlenecekListesi = kamuUstlenecekListesi.Select(x =>
            {
                x.TapuAda = binaDegerlendirme.Ada;
                x.TapuParsel = binaDegerlendirme.Parsel;
                x.PasifMaliyeHazinesiMi = true;
                x.OlusturanIp = userInfo.IpAdresi;
                x.OlusturanKullaniciId = kullaniciId;
                x.OlusturmaTarihi = DateTime.Now;
                return x;
            }).ToList();


            _basvuruKamuUstlenecekRepository.UpdateRange(kamuUstlenecekListesi);
            await _basvuruKamuUstlenecekRepository.SaveChanges();

            foreach (var item in kamuUstlenecekListesi)
            {
                await _mediator.Send(new KaydetBasvuruImzaVerenCommand()
                {
                    BinaDegerlendirmeId = request.BinaDegerlendirmeId,
                    BasvuruDestekTurId = item.BasvuruDestekTurId,
                    BasvuruKamuUstlenecekId = item.BasvuruKamuUstlenecekId,
                    AhirliKonutMu = item.BasvuruTurId == BasvuruTurEnum.AhirliKonut ? true : false,
                    KonutMu = item.BasvuruTurId == BasvuruTurEnum.Konut ? true : false,
                    IsyeriMi = item.BasvuruTurId == BasvuruTurEnum.Ticarethane ? true : false,
                    KamuUstlenecekMi = (request.BasvuruKamuUstlenecekList.FirstOrDefault().HibeOdemeTutar == null && request.BasvuruKamuUstlenecekList.FirstOrDefault().KrediOdemeTutar == null) ? false:true,
                    HibeOdemeTutar = Convert.ToInt32(FormatExtension.TurkishLiraToNumber(request.BasvuruKamuUstlenecekList.FirstOrDefault().HibeOdemeTutar)),
                    KrediOdemeTutar = Convert.ToInt32(FormatExtension.TurkishLiraToNumber(request.BasvuruKamuUstlenecekList.FirstOrDefault().KrediOdemeTutar)),
                });
            }

            result.Result = new KaydetPasifMalikKamuUstelenecekCommandResponseModel
            {
                Mesaj = "İşleminiz Başarılı Bir Şekilde Tamamlanmıştır."
            };

            return await Task.FromResult(result);
        }
    }
}