using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.X86;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Commands.AktarKamuUstlenecek;

public class AktarKamuUstlenecekCommand : IRequest<ResultModel<string>>
{
    public long? BasvuruId { get; set; }

    public class AktarKamuUstlenecekCommandHandler : IRequestHandler<AktarKamuUstlenecekCommand, ResultModel<string>>
    {
        private readonly IBasvuruRepository _basvuruRepository;
        private readonly IBasvuruKamuUstlenecekRepository _basvuruKamuUstlenecekRepository;
        private readonly IMediator _mediator;
        private readonly IKullaniciBilgi _kullaniciBilgi;

        public AktarKamuUstlenecekCommandHandler(IBasvuruRepository basvuruRepository
            , IMediator mediator
            , IBasvuruKamuUstlenecekRepository basvuruKamuUstlenecekRepository
            , IKullaniciBilgi kullaniciBilgi)
        {
            _basvuruKamuUstlenecekRepository = basvuruKamuUstlenecekRepository;
            _basvuruRepository = basvuruRepository;
            _mediator = mediator;
            _kullaniciBilgi = kullaniciBilgi;
        }

        public async Task<ResultModel<string>> Handle(AktarKamuUstlenecekCommand request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<string>();

            var basvuru = await _basvuruRepository.GetWhere(x => !x.SilindiMi && x.AktifMi == true
                                                    && x.BasvuruId == request.BasvuruId).FirstOrDefaultAsync();

            if (basvuru == null)
            {
                result.ErrorMessage("Başvuru bulunamadı.");
                return await Task.FromResult(result);
            }

            var basvuruKamuUstlenecek = await _basvuruKamuUstlenecekRepository.GetWhere(x=> !x.SilindiMi && x.AktifMi == true
                                                                    && x.UavtMahalleNo == basvuru.UavtMahalleNo
                                                                    && x.TcKimlikNo == basvuru.TcKimlikNo
                                                                    && x.TapuTasinmazId == basvuru.TapuTasinmazId
                                                                ).FirstOrDefaultAsync();

            if(basvuruKamuUstlenecek != null)
            {
                result.ErrorMessage("Bu T.C. Kimlik Numarasına ve Tapu Taşınmazına ait bir Kamu Üstlenecek bilgisi zaten mevcut.");
                return await Task.FromResult(result);
            }

            basvuruKamuUstlenecek = new BasvuruKamuUstlenecek
            {
                BasvuruAfadDurumId = (long)BasvuruAfadDurumEnum.BasvuruYok,
                BasvuruDurumId = (long)BasvuruDurumEnum.BasvurunuzDegerlendirmeAsamasindadir,
                BasvuruDestekTurId = BasvuruDestekTurEnum.HibeVeKredi,
                BasvuruTurId = basvuru.BasvuruTurId,
                TuzelKisiTipId = basvuru.TuzelKisiTipId,
                AktifMi = true,
                SilindiMi = false,
                Ad = basvuru.Ad,
                Soyad = basvuru.Soyad,
                TcKimlikNo = basvuru.TcKimlikNo,
                TuzelKisiVergiNo = basvuru.TuzelKisiVergiNo,
                TapuTasinmazId = basvuru.TapuTasinmazId,
                TapuAnaTasinmazId = basvuru.TapuAnaTasinmazId,
                TapuAda = basvuru.TapuAda,
                TapuParsel = basvuru.TapuParsel,
                TapuArsaPay = basvuru.TapuArsaPay,
                TapuArsaPayda = basvuru.TapuArsaPayda,
                TapuMahalleId = basvuru.TapuMahalleId,
                TapuMahalleAdi = basvuru.TapuMahalleAdi,
                TapuKat = basvuru.TapuKat,
                TapuIlceAdi = basvuru.TapuIlceAdi,
                TapuIlceId = basvuru.TapuIlceId,
                TapuIstirakNo = basvuru.TapuIstirakNo,
                TapuBlok = basvuru.TapuBlok,
                TapuTasinmazTipi = basvuru.TapuTasinmazTipi,
                TapuBagimsizBolumNo = basvuru.TapuBagimsizBolumNo,
                TapuNitelik = basvuru.TapuNitelik,
                TapuIlAdi = basvuru.TapuIlAdi,
                TapuIlId = basvuru.TapuIlId,
                UavtMahalleAdi = basvuru.UavtMahalleAdi,
                UavtIlAdi = basvuru.UavtIlAdi,
                UavtIlNo = basvuru.UavtIlNo,
                UavtIlKodu = basvuru.UavtIlKodu,
                UavtIlceAdi = basvuru.UavtIlceAdi,
                UavtIlceNo = basvuru.UavtIlceNo,
                UavtIlceKodu = basvuru.UavtIlceKodu,
                UavtMahalleNo = basvuru.UavtMahalleNo,
                UavtMahalleKodu = basvuru.UavtMahalleKodu,
                OlusturmaTarihi = DateTime.Now,
                OlusturanKullaniciId = 1,
            };
      
            await _basvuruKamuUstlenecekRepository.AddAsync(basvuruKamuUstlenecek);
            await _basvuruKamuUstlenecekRepository.SaveChanges();

            if (basvuru.SonuclandirmaAciklamasi?.Contains("kamu_ustlenecek_tablosuna_aktarildi") != true)
            {
                basvuru.SonuclandirmaAciklamasi = (basvuru.SonuclandirmaAciklamasi ?? "") + "kamu_ustlenecek_tablosuna_aktarildi";
                await _basvuruRepository.SaveChanges();
            }

            result.Result = "İşlem başarıyla tamamlandı.";

            return await Task.FromResult(result);
        }
    }
}