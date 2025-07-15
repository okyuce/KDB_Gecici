using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Csb.YerindeDonusum.Application.CQRS.BinaOdemeCQRS.Commands.BinaOdemeDurumGuncelle;

public class BinaOdemeDurumGuncelleCommand : IRequest<ResultModel<BinaOdemeDurumGuncelleResponseModel>>
{
    public List<long>? BinaOdemeIds { get; set; }
    public long? BinaOdemeDurumId { get; set; }
    public string? ReddetmeSebebi { get; set; }

    public class BinaOdemeDurumGuncelleCommandHandler : IRequestHandler<BinaOdemeDurumGuncelleCommand, ResultModel<BinaOdemeDurumGuncelleResponseModel>>
    {
        private readonly IBinaOdemeRepository _binaOdemeRepository; 
        private readonly IAfadBasvuruTekilRepository _afadBasvuruTekilRepository;
        private readonly IKullaniciBilgi _kullaniciBilgi;

        public BinaOdemeDurumGuncelleCommandHandler(IKullaniciBilgi kullaniciBilgi, IBinaOdemeRepository binaOdemeRepository, IAfadBasvuruTekilRepository afadBasvuruTekilRepository)
        {
            _binaOdemeRepository = binaOdemeRepository;
            _kullaniciBilgi = kullaniciBilgi;
            _afadBasvuruTekilRepository = afadBasvuruTekilRepository;
        }

        public async Task<ResultModel<BinaOdemeDurumGuncelleResponseModel>> Handle(BinaOdemeDurumGuncelleCommand request, CancellationToken cancellationToken)
        {
            ResultModel<BinaOdemeDurumGuncelleResponseModel> result = new();

            long.TryParse(_kullaniciBilgi.GetUserInfo()?.KullaniciId, out long kullaniciId);
            string? ipAdresi = _kullaniciBilgi.GetUserInfo()?.IpAdresi;

            var binaOdemeList = await _binaOdemeRepository.GetWhere(x => x.SilindiMi == false && x.AktifMi == true
                                                                && request.BinaOdemeIds.Any(y => y == x.BinaOdemeId)
                                                                && (x.BinaOdemeDurumId == (int)BinaOdemeDurumEnum.Bekleniyor
                                                                        || x.BinaOdemeDurumId == (int)BinaOdemeDurumEnum.IstekAlindi
                                                                        || x.BinaOdemeDurumId == (int)BinaOdemeDurumEnum.Reddedildi
                                                                        || x.BinaOdemeDurumId == (int)BinaOdemeDurumEnum.HYSAktarildi))
                                                .Include(x => x.BinaDegerlendirme)
                                                    .ThenInclude(x => x.Basvurus.Where(y => y.SilindiMi == false && y.AktifMi == true))
                                                        .ThenInclude(x => x.BasvuruImzaVerens.Where(y => y.SilindiMi == false && y.AktifMi == true))
                                                    .ToListAsync();

            foreach (var binaOdeme in binaOdemeList)
            {
                if (binaOdeme.BinaDegerlendirme?.Basvurus?.Any(x => x.BasvuruAfadDurumId == (long)BasvuruAfadDurumEnum.Kabul) == true)
                {
                    result.ErrorMessage("AFAD durumu Kabul olan başvurular olduğundan ödeme işlemine devam edilemiyor!");
                    return await Task.FromResult(result);
                }
               
                // Durumu Odeme Bekleniyor degilse Reddedilemez.
                if (request.BinaOdemeDurumId == (int)BinaOdemeDurumEnum.Reddedildi 
                        && binaOdeme.BinaOdemeDurumId != (int)BinaOdemeDurumEnum.Bekleniyor)
                {
                    continue;
                }

                // Durumu Odeme Reddedildi degilse Ödeme Bekleniyor'a alınamaz.
                if (request.BinaOdemeDurumId == (int)BinaOdemeDurumEnum.Bekleniyor
                        && binaOdeme.BinaOdemeDurumId != (int)BinaOdemeDurumEnum.Reddedildi)
                {
                    continue;
                }

                binaOdeme.BinaOdemeDurumId = request.BinaOdemeDurumId ?? binaOdeme.BinaOdemeDurumId;
                
                if(binaOdeme.BinaOdemeDurumId == (long)BinaOdemeDurumEnum.Bekleniyor)
                {
                    binaOdeme.TalepDurumu = "Açık";
                }
                else if(binaOdeme.BinaOdemeDurumId == (long)BinaOdemeDurumEnum.MuteahhiteAktarildi)
                {
                    binaOdeme.TalepDurumu = "Kapalı";
                    binaOdeme.TalepKapatmaTarihi = DateTime.Now;
                }
                else if (binaOdeme.BinaOdemeDurumId == (long)BinaOdemeDurumEnum.Reddedildi)
                {
                    binaOdeme.TalepDurumu = "Reddedildi: " + request.ReddetmeSebebi;
                    binaOdeme.TalepKapatmaTarihi = DateTime.Now;
                }

                binaOdeme.GuncellemeTarihi = DateTime.Now;
                binaOdeme.GuncelleyenIp = ipAdresi;
                binaOdeme.GuncelleyenKullaniciId = kullaniciId;
            }

            _binaOdemeRepository.UpdateRange(binaOdemeList);
            await _binaOdemeRepository.SaveChanges();
          
            result.Result = new BinaOdemeDurumGuncelleResponseModel
            {
                Mesaj = "İşleminiz Başarılı Bir Şekilde Tamamlanmıştır.",
            };
            return await Task.FromResult(result);
        }
    }
}