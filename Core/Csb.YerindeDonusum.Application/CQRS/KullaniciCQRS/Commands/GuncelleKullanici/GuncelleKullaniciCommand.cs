using AutoMapper;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Addons.StringAddons;
using Csb.YerindeDonusum.Domain.Cryptography;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Csb.YerindeDonusum.Application.CQRS.KullaniciCQRS.Commands
{
    public class GuncelleKullaniciCommand : IRequest<ResultModel<GuncelleKullaniciCommandResponseModel>>
    {
        public long? KullaniciId { get; set; }
        public string? KullaniciAdi { get; set; }
        public string? Sifre { get; set; }
        public long? KullaniciHesapTipId { get; set; }
        public long? BirimId { get; set; }
        public bool? AktifMi { get; set; }
        public long? TcKimlikNo { get; set; }
        public string? Eposta { get; set; }
        public string? CepTelefonu { get; set; }
        public string? Ad { get; set; }
        public string? Soyad { get; set; }
        public List<long>? SecilenRolIdList { get; set; }

        public class GuncelleKullaniciCommandHandler : IRequestHandler<GuncelleKullaniciCommand, ResultModel<GuncelleKullaniciCommandResponseModel>>
        {
            private readonly IMapper _mapper;
            private readonly IKullaniciRepository _kullaniciRepository;
            private readonly IWebHostEnvironment _webHostEnvironment;
            private readonly IKullaniciBilgi _kullaniciBilgi;
            private readonly ICacheService _cacheService;

            public GuncelleKullaniciCommandHandler(IMapper mapper
                , IKullaniciRepository kullaniciRepository,
                IWebHostEnvironment webHostEnvironment,
                ICacheService cacheService,
                IKullaniciBilgi kullaniciBilgi)
            {
                _cacheService = cacheService;
                _webHostEnvironment = webHostEnvironment;
                _mapper = mapper;
                _kullaniciRepository = kullaniciRepository;
                _kullaniciBilgi = kullaniciBilgi;
            }

            public async Task<ResultModel<GuncelleKullaniciCommandResponseModel>> Handle(GuncelleKullaniciCommand request, CancellationToken cancellationToken)
            {
                var result = new ResultModel<GuncelleKullaniciCommandResponseModel>();

                var kullaniciMevcutMu = _kullaniciRepository.GetWhereEnumerable(x =>
                    x.SilindiMi == false
                    &&
                    x.KullaniciId != request.KullaniciId
                    &&
                    (
                        StringAddons.NormalizeText(x.KullaniciAdi).Equals(StringAddons.NormalizeText(request.KullaniciAdi))
                        ||
                        x.TcKimlikNo == request.TcKimlikNo
                    )
                ).Any();

                if (kullaniciMevcutMu)
                {
                    result.ErrorMessage("Bu TC kimlik numarasına ya da kullanıcı adına ait kullanıcı zaten mevcut.");
                    return await Task.FromResult(result);
                }

                var kullanici = await _kullaniciRepository.GetWhere(x =>
                    x.SistemKullanicisiMi != true //sistem kullanıcıları dışındakiler güncellenebilir
					&&
					x.KullaniciId == request.KullaniciId
					&&
					!x.SilindiMi,
						false,
                        x => x.KullaniciRols.Where(x => x.AktifMi == true && x.SilindiMi == false)
                    ).FirstOrDefaultAsync();

                if (kullanici == null)
                {
                    result.ErrorMessage("Kullanıcı veritabanında bulunamadı.");
                    return await Task.FromResult(result);
                }

                kullanici.AktifMi = request.AktifMi;
                kullanici.BirimId = request.BirimId;
                kullanici.Eposta = request.Eposta;
                kullanici.CepTelefonu = StringAddon.ToClearPhone(request.CepTelefonu);
                kullanici.KullaniciAdi = request.KullaniciAdi;
                kullanici.KullaniciHesapTipId = (long)request.KullaniciHesapTipId;
                kullanici.TcKimlikNo = request.TcKimlikNo;

                long.TryParse(_kullaniciBilgi.GetUserInfo().KullaniciId, out long kullaniciId);
                var ipAdresi = _kullaniciBilgi.GetUserInfo().IpAdresi;

                kullanici.GuncelleyenKullaniciId = kullaniciId;
                kullanici.GuncelleyenIp = ipAdresi;
                kullanici.GuncellemeTarihi = DateTime.Now;

                kullanici.KullaniciRols.ToList();

                request.SecilenRolIdList ??= new List<long>();

                #region ...: Silinecek Yetkileri Sil :...
                var mevcutYetkiIdListe = kullanici.KullaniciRols.Select(s => s.RolId).ToList();
                foreach (var silinenYetkiId in mevcutYetkiIdListe.Except(request.SecilenRolIdList))
                {
                    foreach (var silinenYetki in kullanici.KullaniciRols.Where(x => x.RolId == silinenYetkiId))
                    {
                        silinenYetki.SilindiMi = true;
                        silinenYetki.AktifMi = false;
                        silinenYetki.GuncelleyenKullaniciId = kullaniciId;
                        silinenYetki.GuncellemeTarihi = DateTime.Now;
                        silinenYetki.GuncelleyenIp = ipAdresi;
                    }
                }
                #endregion

                #region ...: Yeni Yetkileri Ekle :...

                var yeniler = request.SecilenRolIdList.Except(mevcutYetkiIdListe).Distinct();

                foreach (var eklenenRolId in request.SecilenRolIdList.Except(mevcutYetkiIdListe).Distinct())
                {
                    kullanici.KullaniciRols.Add(new KullaniciRol
                    {
                        RolId = eklenenRolId,
                        KullaniciId = kullanici.KullaniciId,
                        OlusturanKullaniciId = kullaniciId,
                        OlusturanIp = ipAdresi,
                        AktifMi = true,
                        SilindiMi = false
                    });
                }
                #endregion

                await _kullaniciRepository.SaveChanges(cancellationToken);

                var cacheKey = $"{_webHostEnvironment.EnvironmentName}_" + $"{nameof(GetirKullaniciListeQuery)}";
                await _cacheService.Clear(cacheKey);

                result.Result = new GuncelleKullaniciCommandResponseModel
                {
                    Mesaj = "İşleminiz Başarılı Bir Şekilde Tamamlanmıştır.",
                };

                return await Task.FromResult(result);
            }
        }
    }
}