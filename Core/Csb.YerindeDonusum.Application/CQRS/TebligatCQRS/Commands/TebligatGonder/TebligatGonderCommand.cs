using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.EDevlet;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Csb.YerindeDonusum.Application.CQRS.TebligatCQRS.Commands.TebligatGonder;

public class TebligatGonderCommand : IRequest<ResultModel<string>>
{
    public EDevletTebligatTipiEnum TebligatTipi { get; set; }
    public List<TebligatGonderCommandModel>? TebligatYapilacaklar { get; set; }

    public class TebligatGonderCommandHandler : IRequestHandler<TebligatGonderCommand, ResultModel<string>>
    {
        private readonly IEDevletTebligatService _eDevletTebligatService;
        private readonly ITebligatGonderimRepository _tebligatGonderim;
        private readonly ITebligatGonderimDetayRepository _tebligatGonderimDetay;
        private readonly IKullaniciBilgi _kullaniciBilgi;

        public TebligatGonderCommandHandler(IEDevletTebligatService eDevletTebligatService, ITebligatGonderimRepository tebligatGonderim, ITebligatGonderimDetayRepository tebligatGonderimDetay, IKullaniciBilgi kullaniciBilgi)
        {
            _eDevletTebligatService = eDevletTebligatService;
            _tebligatGonderim = tebligatGonderim;
            _kullaniciBilgi = kullaniciBilgi;
            _tebligatGonderimDetay = tebligatGonderimDetay;
        }

        public async Task<ResultModel<string>> Handle(TebligatGonderCommand request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<string>();

            if (request.TebligatYapilacaklar.Count() > 1000)
            {
                result.ErrorMessage("Tebligat Göderim Sınırı 1000 Kişi İle Sınırlıdır.Bu Sebeple Tebligat Gönderilememektedir.");
                return await Task.FromResult(result);
            }

            var tebligatdetay = _tebligatGonderimDetay.GetWhere(x => request.TebligatYapilacaklar.Select(s => s.TcKimlikNoRaw.ToString()).Contains(x.TcKimlikNo)
                                                                    && request.TebligatYapilacaklar.Select(s => s.TapuIlAdi.ToString()).Contains(x.TapuIlAdi)
                                                                    && request.TebligatYapilacaklar.Select(s => s.TapuIlceAdi.ToString()).Contains(x.TapuIlceAdi)
                                                                    && request.TebligatYapilacaklar.Select(s => s.TapuMahalleAdi.ToString()).Contains(x.TapuMahalleAdi)
                                                                    && request.TebligatYapilacaklar.Select(s => s.AskiKodu.ToString()).Contains(x.HasarTespitAskiKodu)
                                                                    && request.TebligatTipi.GetHashCode() == x.TebligatTipiId.Value
                                                                , true).ToList();
            
            var tebligatIds = tebligatdetay.Select(x => x.TebligatGonderimId).Distinct().ToList();
            
            var tebligat = _tebligatGonderim.GetWhere(x => tebligatIds.Contains(x.TebligatGonderimId) && x.GonderimBasariliMi == true).ToList();

            if (tebligatdetay.Count() > 0 && tebligat.Count() > 0)
            {
                result.ErrorMessage("Aynı Tebligat Türü Daha Önce Gönderilmiştir.");
                return await Task.FromResult(result);
            }

            if (!request.TebligatYapilacaklar.Select(s => s.TuzelKisiVergiNo).Contains(null) || request.TebligatYapilacaklar.Select(s => s.TcKimlikNoRaw).Contains(null))
            {
                result.ErrorMessage("Tüzel Kişiye Tebligat Gönderilememektedir.");
                return await Task.FromResult(result);
            }

            TebligatGonderim tebligatGonderim = new TebligatGonderim() { TebligatGonderimDetays = new HashSet<TebligatGonderimDetay>() };
            tebligatGonderim.GonderimTarihi = DateTime.Now;
            tebligatGonderim.GonderenKullaniciId = Convert.ToInt64(_kullaniciBilgi.GetUserInfo().KullaniciId);

            foreach (var yapilacak in request.TebligatYapilacaklar)
            {

                TebligatGonderimDetay tebligatGonderimDetay = new TebligatGonderimDetay();
                tebligatGonderimDetay.HasarTespitHasarDurumu = yapilacak.HasarTespitHasarDurumu;
                tebligatGonderimDetay.HasarTespitAskiKodu = yapilacak.AskiKodu;
                tebligatGonderimDetay.TapuAda = yapilacak.TapuAda == null ? "-" : yapilacak.TapuAda;
                tebligatGonderimDetay.TapuParsel = yapilacak.TapuParsel;
                tebligatGonderimDetay.TapuTasinmazId = yapilacak.TapuTasinmazId == null ? 0 : Convert.ToInt32(yapilacak.TapuTasinmazId);
                tebligatGonderimDetay.TapuIlAdi = yapilacak.TapuIlAdi;
                tebligatGonderimDetay.TapuIlceAdi = yapilacak.TapuIlceAdi;
                tebligatGonderimDetay.TapuMahalleAdi = yapilacak.TapuMahalleAdi;
                tebligatGonderimDetay.TebligTarihi = yapilacak.TebligTarihi;
                tebligatGonderimDetay.TebligatTipiId = (int)request.TebligatTipi;
                tebligatGonderimDetay.TuzelKisiVergiNo = yapilacak.TuzelKisiVergiNo;
                tebligatGonderimDetay.TcKimlikNo = yapilacak.TcKimlikNoRaw?.ToString();
                tebligatGonderim.TebligatGonderimDetays.Add(tebligatGonderimDetay);
            }

            _tebligatGonderim.AddAsync(tebligatGonderim);
            await _tebligatGonderim.SaveChanges();

            request.TebligatYapilacaklar = request.TebligatYapilacaklar.Select(x => { x.GonderimId = tebligatGonderim.TebligatGonderimId.ToString(); return x; }).ToList();

            var servisResult = await _eDevletTebligatService.TebligatGonder(request);

            if (servisResult.SonucKodu == "0000")
            {
                tebligatGonderim.GonderimBasariliMi = true;
                tebligatGonderim.EdevletTakipId = servisResult.TakipIdList != null || servisResult.TakipIdList.Count > 0 ? servisResult.TakipIdList[0] : null;
                result.Result = "Tebligat Başarıyla Gönderildi";
            }
            else
            {
                tebligatGonderim.GonderimBasariliMi = false;
                tebligatGonderim.GonderimAciklama = servisResult.SonucKodu + " " + servisResult.SonucAciklamasi;
                _tebligatGonderim.Update(tebligatGonderim);
                await _tebligatGonderim.SaveChanges();
                result.ErrorMessage(servisResult.SonucAciklamasi);
                return await Task.FromResult(result);
            }

            tebligatGonderim.GonderimAciklama = servisResult.SonucKodu + " " + servisResult.SonucAciklamasi;

            _tebligatGonderim.Update(tebligatGonderim);
            await _tebligatGonderim.SaveChanges();

            return result;
        }
    }
}