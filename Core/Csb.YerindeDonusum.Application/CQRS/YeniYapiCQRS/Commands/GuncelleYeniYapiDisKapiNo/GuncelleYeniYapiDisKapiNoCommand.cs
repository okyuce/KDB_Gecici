using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Addons.FileAddons;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Csb.YerindeDonusum.Application.CQRS.YeniYapiCQRS.Commands.GuncelleYeniYapiDisKapiNo;

public class GuncelleYeniYapiDisKapiNoCommand : IRequest<ResultModel<GuncelleYeniYapiDisKapiNoCommandResponseModel>>
{
    public long? BinaDegerlendirmeId { get; set; }
    public List<long>? BasvuruIds { get; set; }
    public List<long>? BasvuruKamuUstlenecekIds { get; set; }

    public class GuncelleYeniYapiDisKapiNoCommandHandler : IRequestHandler<GuncelleYeniYapiDisKapiNoCommand, ResultModel<GuncelleYeniYapiDisKapiNoCommandResponseModel>>
    {
        private readonly IBasvuruRepository _basvuruRepository;
        private readonly IBasvuruKamuUstlenecekRepository _basvuruKamuUstlenecekRepository;
        private readonly IBinaDegerlendirmeRepository _binaDegerlendirmeRepository;
        private readonly IConfiguration _configuration;
        private readonly IKullaniciBilgi _kullaniciBilgi;

        public GuncelleYeniYapiDisKapiNoCommandHandler(IBasvuruRepository basvuruRepository
            , IBinaDegerlendirmeRepository binaDegerlendirmeRepository
            , IKullaniciBilgi kullaniciBilgi
            , IBasvuruKamuUstlenecekRepository basvuruKamuUstlenecekRepository
            , IConfiguration configuration)
        {
            _basvuruKamuUstlenecekRepository = basvuruKamuUstlenecekRepository;
            _configuration = configuration;
            _binaDegerlendirmeRepository = binaDegerlendirmeRepository;
            _binaDegerlendirmeRepository = binaDegerlendirmeRepository;
            _kullaniciBilgi = kullaniciBilgi;
            _basvuruRepository = basvuruRepository;
        }

        public async Task<ResultModel<GuncelleYeniYapiDisKapiNoCommandResponseModel>> Handle(GuncelleYeniYapiDisKapiNoCommand request, CancellationToken cancellationToken)
        {
            ResultModel<GuncelleYeniYapiDisKapiNoCommandResponseModel> result = new();

            var kullaniciBilgi = _kullaniciBilgi.GetUserInfo();
            long.TryParse(kullaniciBilgi.KullaniciId, out long kullaniciId);

            var binaDegerlendirme = await _binaDegerlendirmeRepository
                                            .GetWhere(x => !x.SilindiMi && x.AktifMi == true
                                                && x.BinaDegerlendirmeDurumId != (long)BinaDegerlendirmeDurumEnum.Reddedildi
                                                && x.BinaDegerlendirmeId == request.BinaDegerlendirmeId
                                            ).FirstOrDefaultAsync();

            if (binaDegerlendirme == null)
            {
                result.ErrorMessage("Bina Değerlendirme bilgisi bulunamadı.");
                return result;
            }
            if (binaDegerlendirme.BinaDegerlendirmeDurumId == (long)BinaDegerlendirmeDurumEnum.YapiRuhsatinizOnaylanmistir
                     || binaDegerlendirme.BinaDegerlendirmeDurumId == (long)BinaDegerlendirmeDurumEnum.YapiIlerlemeSeviyesiYuzde20
                     || binaDegerlendirme.BinaDegerlendirmeDurumId == (long)BinaDegerlendirmeDurumEnum.YapiIlerlemeSeviyesiYuzde60
                     || binaDegerlendirme.BinaDegerlendirmeDurumId == (long)BinaDegerlendirmeDurumEnum.YapiTamamlanmistir)
            {
                result.ErrorMessage("Yapı ruhsatınız onaylandığı için işleme devam edilemiyor.");
                return result;
            }

            var basvuruList = _basvuruRepository.GetWhere(x => x.SilindiMi == false && x.AktifMi == true
                                                        //&& x.BasvuruAfadDurumId != (long)BasvuruAfadDurumEnum.Kabul
                                                        && x.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi
                                                        && x.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir
                                                        && request.BasvuruIds.Any(y => y == x.BasvuruId)
                                                        , true
                                                        , i=> i.BasvuruImzaVerens.Where(y=> y.SilindiMi == false && y.AktifMi == true)
                                                    ).ToList();
            if (basvuruList?.Any() == true)
            {
                foreach (var basvuru in basvuruList)
                {
                    basvuru.BinaDegerlendirmeId = binaDegerlendirme.BinaDegerlendirmeId;
                }

                _basvuruRepository.UpdateRange(basvuruList);
                await _basvuruRepository.SaveChanges();
            }

            var basvuruKamuUstlenecekList = _basvuruKamuUstlenecekRepository.GetWhere(x => x.SilindiMi == false && x.AktifMi == true
                                                        //&& x.BasvuruAfadDurumId != (long)BasvuruAfadDurumEnum.Kabul
                                                        && x.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi
                                                        && x.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir
                                                        && request.BasvuruKamuUstlenecekIds.Any(y => y == x.BasvuruKamuUstlenecekId)
                                                        , true
                                                        , i => i.BasvuruImzaVerens.Where(y => y.SilindiMi == false && y.AktifMi == true)
                                                    ).ToList();

            if (basvuruKamuUstlenecekList?.Any() == true)
            {
                foreach (var basvuruKamuUstlenecek in basvuruKamuUstlenecekList)
                {
                    basvuruKamuUstlenecek.BinaDegerlendirmeId = binaDegerlendirme.BinaDegerlendirmeId;
                }

                _basvuruKamuUstlenecekRepository.UpdateRange(basvuruKamuUstlenecekList);
                await _basvuruKamuUstlenecekRepository.SaveChanges();
            }   

            result.Result = new GuncelleYeniYapiDisKapiNoCommandResponseModel
            {
                Mesaj = "İşleminiz Başarılı Bir Şekilde Tamamlanmıştır."
            };

            return await Task.FromResult(result);
        }
    }
}