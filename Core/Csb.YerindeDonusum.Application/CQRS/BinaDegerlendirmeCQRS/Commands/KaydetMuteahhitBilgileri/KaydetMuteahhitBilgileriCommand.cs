using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.CQRS.KpsCQRS.Queries.GetirKisiAdSoyadTcDen;
using Csb.YerindeDonusum.Application.CQRS.KpsCQRS.Queries.GetirKisiBilgileriTcDen;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Commands.KaydetMuteahhitBilgileri;

public class KaydetMuteahhitBilgileriCommand : IRequest<ResultModel<string>>
{
    public long? BinaDegerlendirmeId { get; set; }
    public string? AdSoyadUnvan { get; set; }
    public string? Aciklama { get; set; }
    public string? Adres { get; set; }
    public string? CepTelefonu { get; set; }
    public string? EPosta { get; set; }
    public string? VergiKimlikNo { get; set; }
    public string? YetkiBelgeNo { get; set; }
    public string? Telefon { get; set; }
    public string? IbanNo { get; set; }
    public long? BinaMuteahhitTapuTurId { get; set; }
    public string? MuteahhitTc { get; set; }

    public class KaydetMuteahhitBilgileriCommandHandler : IRequestHandler<KaydetMuteahhitBilgileriCommand, ResultModel<string>>
    {
        private readonly IBinaDegerlendirmeRepository _binaDegerlendirmeRepository;
        private readonly IBinaMuteahhitRepository _binaMuteahhitRepository;
        private readonly IKullaniciBilgi _kullaniciBilgi;
        private readonly IMediator _mediator;

        public KaydetMuteahhitBilgileriCommandHandler(IBinaDegerlendirmeRepository binaDegerlendirmeRepository
            , IBinaMuteahhitRepository binaMuteahhitRepository
            , IKullaniciBilgi kullaniciBilgi
            , IMediator mediator)
        {
            _binaDegerlendirmeRepository = binaDegerlendirmeRepository;
            _binaMuteahhitRepository = binaMuteahhitRepository;
            _kullaniciBilgi = kullaniciBilgi;
            _mediator = mediator;
        }

        public async Task<ResultModel<string>> Handle(KaydetMuteahhitBilgileriCommand request, CancellationToken cancellationToken)
        {
            ResultModel<string> result = new();

            var binaDegerlendirme = await _binaDegerlendirmeRepository.GetAllQueryable()
                                                .Include(i => i.BinaMuteahhits.Where(x => !x.SilindiMi && x.AktifMi == true))
                                                .Include(i => i.BinaDegerlendirmeDosyas.Where(x => !x.SilindiMi && x.AktifMi == true))
                                                .Include(i => i.Basvurus.Where(x => !x.SilindiMi && x.AktifMi == true)).ThenInclude(x=>x.BasvuruImzaVerens.Where(x=>x.AktifMi==true && x.SilindiMi==false))                                                
                                                .Where(x => x.SilindiMi == false && x.AktifMi == true
                                                    && x.BinaDegerlendirmeId == request.BinaDegerlendirmeId)                                                
                                                
                                            .FirstOrDefaultAsync();

            if (binaDegerlendirme == null)
            {
                result.ErrorMessage("Bina Değerlendirme bilgisi bulunamadı.");
                return result;
            }


            if (!binaDegerlendirme.BinaDegerlendirmeDosyas.Any())
            {
                result.ErrorMessage("Lütfen önce 'Belge Bilgileri' alanını doldurunuz.");
                return result;
            }

            if (binaDegerlendirme.Basvurus.SelectMany(x=>x.BasvuruImzaVerens).Where(x => x.AktifMi == true && x.SilindiMi == false && x.IbanNo == request.IbanNo).Any())
            {
                result.ErrorMessage("Yapıma yönelik diğer hibe ödemesinene girilen iban bilgisi ile müteahhit iban bilgisinin aynı olması sebebiyle işleme devam edilemez");
                return result;
            }

            var kullaniciBilgi = _kullaniciBilgi.GetUserInfo();
            long.TryParse(kullaniciBilgi.KullaniciId, out long kullaniciId);
                                 
            var muteahhitBilgi = binaDegerlendirme.BinaMuteahhits.FirstOrDefault();
            muteahhitBilgi ??= new BinaMuteahhit()
            {
                AktifMi = true,
                SilindiMi = false,
                OlusturmaTarihi = DateTime.Now,
                OlusturanIp = kullaniciBilgi.IpAdresi,
                OlusturanKullaniciId = kullaniciId,
                BinaDegerlendirmeId = binaDegerlendirme.BinaDegerlendirmeId
            };

            if (request.BinaMuteahhitTapuTurId == (long)MuteahhitBilgileriTipEnum.Muteahhit)
            {
                muteahhitBilgi.Aciklama = request.Aciklama;
                muteahhitBilgi.Adres = request.Adres;
                muteahhitBilgi.Adsoyadunvan = request.AdSoyadUnvan;
                muteahhitBilgi.CepTelefonu = request.CepTelefonu;
                muteahhitBilgi.Eposta = request.EPosta;
                muteahhitBilgi.IbanNo = request.IbanNo;
                muteahhitBilgi.Telefon = request.Telefon;
                muteahhitBilgi.VergiKimlikNo = request.VergiKimlikNo;
                muteahhitBilgi.YetkiBelgeNo = request.YetkiBelgeNo;
                muteahhitBilgi.BinaMuteahhitTapuTurId = (long)request.BinaMuteahhitTapuTurId;
            }
            else
            {
                if(binaDegerlendirme.Basvurus.Any(x=> x.TapuHazineArazisiMi != true && x.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi && x.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir))
                {
                    result.ErrorMessage("Binadaki başvurular arasında hazine arazisi olmayan bir kayıt olduğundan işleme devam edilemiyor.");
                    return await Task.FromResult(result);                   
                }
                else
                {
                    muteahhitBilgi.Aciklama = null;
                    muteahhitBilgi.Adres = null;
                    muteahhitBilgi.CepTelefonu = null;
                    muteahhitBilgi.Eposta = null;
                    muteahhitBilgi.IbanNo = request.IbanNo;
                    muteahhitBilgi.Telefon = null;
                    muteahhitBilgi.VergiKimlikNo = request.MuteahhitTc;
                    muteahhitBilgi.YetkiBelgeNo = null;
                    muteahhitBilgi.BinaMuteahhitTapuTurId = (long)request.BinaMuteahhitTapuTurId;

                    #region ...: Kişi Ad Soyad Boş İse KPS Den Al :...
                    if (FluentValidationExtension.TcDogrula(request?.MuteahhitTc))
                    {
                        var kpsKisiBilgileri = await _mediator.Send(new GetirKisiAdSoyadTcDenQuery { TcKimlikNo = long.Parse(request.MuteahhitTc), MaskelemeKapaliMi = true });
                        if (!kpsKisiBilgileri.IsError)
                        {
                            if (kpsKisiBilgileri.Result.OlumTarih != null)
                            {
                                result.ErrorMessage("Başvuracak Kişi Vefat Ettiği İçin Başvuruya Devam Edemezsiniz.");
                                return await Task.FromResult(result);
                            }

                            muteahhitBilgi.Adsoyadunvan = kpsKisiBilgileri.Result.Ad + " " + kpsKisiBilgileri.Result.Soyad;
                        }
                    }
                    else
                    {
                        result.ErrorMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ, "TC Kimlik Numarası"));
                        return await Task.FromResult(result);
                    }
                    #endregion
                }
            }


            if (muteahhitBilgi.BinaMuteahhitId > 0)
            {
                muteahhitBilgi.GuncellemeTarihi = DateTime.Now;
                muteahhitBilgi.GuncelleyenIp = kullaniciBilgi.IpAdresi;
                muteahhitBilgi.GuncelleyenKullaniciId = kullaniciId;

                _binaMuteahhitRepository.Update(muteahhitBilgi);
            }
            else
                await _binaMuteahhitRepository.AddAsync(muteahhitBilgi);

            await _binaMuteahhitRepository.SaveChanges();
            if (binaDegerlendirme.BinaDegerlendirmeDurumId == (long)BinaDegerlendirmeDurumEnum.InsaatSozlesmesiImzalanmistir)
            {
                binaDegerlendirme.BinaDegerlendirmeDurumId = (long)BinaDegerlendirmeDurumEnum.MuteahhitAtamanizGerceklesmistir;
            }
            _binaDegerlendirmeRepository.Update(binaDegerlendirme);
            await _binaDegerlendirmeRepository.SaveChanges();


            result.Result = "İşleminiz Başarılı Bir Şekilde Tamamlanmıştır.";

            return await Task.FromResult(result);
        }
    }
}