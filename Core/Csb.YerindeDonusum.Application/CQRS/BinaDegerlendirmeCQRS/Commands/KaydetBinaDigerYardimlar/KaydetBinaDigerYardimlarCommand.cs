using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirListeMalikler;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Commands.KaydetBinaDigerYardimlar;

public class KaydetBinaDigerYardimlarCommand : IRequest<ResultModel<string>>
{
    public long? BinaAdinaYardimTipi { get; set; }
    public DateTime? YardimTarihi { get; set; }
    public int? YardimTutari { get; set; }
    public long? BinaDegerlendirmeId { get; set; }
    public int? UavtMahalleNo { get; set; }

    public class KaydetBinaDigerYardimlarCommandHandler : IRequestHandler<KaydetBinaDigerYardimlarCommand, ResultModel<string>>
    {
        private readonly IBinaDegerlendirmeRepository _binaDegerlendirmeRepository;
        private readonly IBinaAdinaYapilanYardimRepository _binaAdinaYapilanYardimRepository;
        private readonly IKullaniciBilgi _kullaniciBilgi;
        private readonly IMediator _mediator;

        public KaydetBinaDigerYardimlarCommandHandler(IBinaDegerlendirmeRepository binaDegerlendirmeRepository
            , IBinaAdinaYapilanYardimRepository binaAdinaYapilanYardimRepository
            , IMediator mediator
            , IKullaniciBilgi kullaniciBilgi)
        {
            _mediator = mediator;
            _binaDegerlendirmeRepository = binaDegerlendirmeRepository;
            _binaAdinaYapilanYardimRepository = binaAdinaYapilanYardimRepository;
            _kullaniciBilgi = kullaniciBilgi;
        }

        public async Task<ResultModel<string>> Handle(KaydetBinaDigerYardimlarCommand request, CancellationToken cancellationToken)
        {
            ResultModel<string> result = new();

            var binaDegerlendirme = await _binaDegerlendirmeRepository.GetWhere(x => !x.SilindiMi && x.AktifMi == true
                                                && x.BinaDegerlendirmeId == request.BinaDegerlendirmeId,
                                        asNoTracking: true,
                                        i => i.BinaAdinaYapilanYardims.Where(x => x.SilindiMi == false && x.AktifMi == true
                                                    && x.BinaAdinaYapilanYardimTipiId == request.BinaAdinaYardimTipi)
                                        ).FirstOrDefaultAsync();

            if (binaDegerlendirme == null)
            {
                result.ErrorMessage("Bina Değerlendirme bilgisi bulunamadı.");
                return result;
            }

            // Bütün Malikler İmza Vermemişse İşleme Devam Edilemez.
            var butunMaliklerImzaVerdiMiResult = await DomainServices.ButunMaliklerImzaVerdiMi(_mediator, binaDegerlendirme);
            if (butunMaliklerImzaVerdiMiResult.IsError)
            {
                result.ErrorMessage(butunMaliklerImzaVerdiMiResult.ErrorMessageContent);
                return result;
            }

            var binaAdinaYapilanYardim = binaDegerlendirme.BinaAdinaYapilanYardims.FirstOrDefault();

            var kullaniciBilgi = _kullaniciBilgi.GetUserInfo();
            long.TryParse(kullaniciBilgi.KullaniciId, out long kullaniciId);

            binaAdinaYapilanYardim ??= new BinaAdinaYapilanYardim()
            {
                AktifMi = true,
                SilindiMi = false,
                OlusturmaTarihi = DateTime.Now,
                OlusturanIp = kullaniciBilgi.IpAdresi,
                OlusturanKullaniciId = kullaniciId,
                BinaDegerlendirmeId = binaDegerlendirme.BinaDegerlendirmeId
            };

            // TODO: Fluent Validation'a Taşınacaktır...
            if (request.BinaAdinaYardimTipi.HasValue == false)
            {
                result.ErrorMessage("Yardım tipi bilgisi boş geçilemez. Lütfen gerekli alanları dolduralım.");
                return result;
            }
            else
            {
                binaAdinaYapilanYardim.BinaAdinaYapilanYardimTipiId = request.BinaAdinaYardimTipi!.Value;
            }

            if (request.YardimTarihi == null)
            {
                result.ErrorMessage("Yardım tarihi bilgisi boş geçilemez. Lütfen gerekli alanları dolduralım.");
                return result;
            }
            else
            {
                binaAdinaYapilanYardim.Tarih = request.YardimTarihi.Value;
            }

            if (request.YardimTutari == null)
            {
                result.ErrorMessage("Yardım tutarı bilgisi boş geçilemez. Lütfen gerekli alanları dolduralım.");
                return result;
            }
            else
            {
                binaAdinaYapilanYardim.Tutar = request.YardimTutari.Value;
            }

            // Add Or Update
            if (binaAdinaYapilanYardim.BinaAdinaYapilanYardimId > 0)
            {
                binaAdinaYapilanYardim.GuncelleyenKullaniciId = kullaniciId;
                binaAdinaYapilanYardim.GuncellemeTarihi = DateTime.Now;
                binaAdinaYapilanYardim.GuncelleyenIp = kullaniciBilgi.IpAdresi;

                _binaAdinaYapilanYardimRepository.Update(binaAdinaYapilanYardim);
            }
            else
            {
                await _binaAdinaYapilanYardimRepository.AddAsync(binaAdinaYapilanYardim);
            }

            await _binaAdinaYapilanYardimRepository.SaveChanges();

            result.Result = "İşleminiz Başarılı Bir Şekilde Tamamlanmıştır.";

            return await Task.FromResult(result);
        }

    }
}
