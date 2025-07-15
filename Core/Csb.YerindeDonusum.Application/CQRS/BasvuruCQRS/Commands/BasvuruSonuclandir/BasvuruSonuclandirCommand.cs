using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using System.Reflection;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Commands.BasvuruSonuclandir;

public class BasvuruSonuclandirCommand : IRequest<ResultModel<string>>
{
    public long? BasvuruId { get; set; }
    //public string? BasvuruGuid { get; set; }
    public long? BasvuruKamuUstlenecekId { get; set; }
    //public string? BasvuruKamuUstlenecekGuid { get; set; }
    //public string? TcKimlikNo { get; set; }
    //public long? BasvuruDurumId { get; set; }
    //public string? SonuclandirmaAciklamasi { get; set; }
    public long? BasvuruIptalTurId { get; set; }

    public class BasvuruSonuclandirCommandHandler : IRequestHandler<BasvuruSonuclandirCommand, ResultModel<string>>
    {
        private readonly IBasvuruRepository _basvuruRepository;
        private readonly IBasvuruKamuUstlenecekRepository _basvuruKamuUstlenecekRepository;
        private readonly IKullaniciBilgi _kullaniciBilgi;

        public BasvuruSonuclandirCommandHandler(IBasvuruRepository basvuruRepository, IBasvuruKamuUstlenecekRepository basvuruKamuUstlenecekRepository, IKullaniciBilgi kullaniciBilgi)
        {
            _basvuruKamuUstlenecekRepository = basvuruKamuUstlenecekRepository;
            _basvuruRepository = basvuruRepository;
            _kullaniciBilgi = kullaniciBilgi;
        }

        public async Task<ResultModel<string>> Handle(BasvuruSonuclandirCommand request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<string>();

            var userInfo = _kullaniciBilgi.GetUserInfo();
            long.TryParse(userInfo.KullaniciId, out long kullaniciId);
            if (request.BasvuruIptalTurId == null || request.BasvuruIptalTurId<1)
            {
                result.ErrorMessage("Başvuru İptal Türü Seçiniz.");
                return await Task.FromResult(result);
            }
            Basvuru? basvuru = null;
            BasvuruKamuUstlenecek? basvuruKamuUstlenecek = null;
            if(request?.BasvuruId > 0)
            {
                basvuru = _basvuruRepository.GetWhere(x => x.AktifMi == true && x.SilindiMi == false 
                                                    && x.BasvuruId == request.BasvuruId
                                                    , false
                                                    , i => i.BasvuruDurum
                                                     , i => i.BinaDegerlendirme
                                            ).FirstOrDefault();

                if (basvuru == null)
                {
                    result.ErrorMessage("Başvuru bulunamadı.");
                    return await Task.FromResult(result);
                }
                if (basvuru.BinaDegerlendirme != null  && basvuru.AktifMi == true && basvuru.SilindiMi == false && basvuru.BinaDegerlendirme.AktifMi==true && basvuru.BinaDegerlendirme.SilindiMi==false && (basvuru.BinaDegerlendirme.BultenNo != null || basvuru.BinaDegerlendirme.IzinBelgesiTarih != null))
                {
                    result.Exception(new InvalidFilterCriteriaException($"{basvuru.BasvuruGuid} - Başvurunun Bina Değerlendirme Durumu: '{BinaDegerlendirmeDurumEnum.YapiRuhsatinizOnaylanmistir.GetDisplayName()}' Olduğu İçin İptal Edilemez."), $"Başvurunun Bina Değerlendirme Durumu: '{BinaDegerlendirmeDurumEnum.YapiRuhsatinizOnaylanmistir.GetDisplayName()}' Olduğu İçin İptal Edilemez.");

                    return await Task.FromResult(result);
                }
                basvuru.GuncelleyenKullaniciId = kullaniciId;
                basvuru.GuncellemeTarihi = DateTime.Now;
                basvuru.GuncelleyenIp = userInfo.IpAdresi;

                basvuru.BasvuruDurumId = (int)BasvuruDurumEnum.BasvuruIptalEdildi; // request?.BasvuruDurumId ?? (int)BasvuruDurumEnum.BasvuruIptalEdildi;
                basvuru.BasvuruIptalTurId = request?.BasvuruIptalTurId;

                await _basvuruRepository.SaveChanges(cancellationToken);
            }
            else
            {
                basvuruKamuUstlenecek = _basvuruKamuUstlenecekRepository.GetWhere(x => x.AktifMi == true && x.SilindiMi == false 
                                                    && x.BasvuruKamuUstlenecekId == request.BasvuruKamuUstlenecekId
                                                    , false
                                                    , i => i.BasvuruDurum
                                            ).FirstOrDefault();

                if (basvuruKamuUstlenecek == null)
                {
                    result.ErrorMessage("Başvuru bulunamadı.");
                    return await Task.FromResult(result);
                }

                basvuruKamuUstlenecek.AktifMi = false;
                basvuruKamuUstlenecek.SilindiMi = true;
                basvuruKamuUstlenecek.GuncelleyenKullaniciId = kullaniciId;
                basvuruKamuUstlenecek.GuncellemeTarihi = DateTime.Now;
                basvuruKamuUstlenecek.GuncelleyenIp = userInfo.IpAdresi;

                basvuruKamuUstlenecek.BasvuruDurumId = (int)BasvuruDurumEnum.BasvuruIptalEdildi; // request?.BasvuruDurumId ?? (int)BasvuruDurumEnum.BasvuruIptalEdildi;
                //basvuruKamuUstlenecek.SonuclandirmaAciklamasi = request?.SonuclandirmaAciklamasi?.Trim();
                basvuruKamuUstlenecek.BasvuruIptalTurId = request.BasvuruIptalTurId;
                await _basvuruKamuUstlenecekRepository.SaveChanges(cancellationToken);
            }

            result.Result = "Başvuru başarıyla iptal edildi.";
            return await Task.FromResult(result);
        }
    }
}