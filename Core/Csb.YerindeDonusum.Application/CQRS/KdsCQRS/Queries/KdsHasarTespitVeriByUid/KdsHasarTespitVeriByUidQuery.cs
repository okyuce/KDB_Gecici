using AutoMapper;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.Kds;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Entities.Kds;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace Csb.YerindeDonusum.Application.CQRS.KdsCQRS.Queries.KdsHasarTespitVeriByUid
{
    public class KdsHasarTespitVeriByUidQuery : IRequest<ResultModel<KdsHaneModel>>
    {
        public string? HasarTespitUid { get; set; }

        public class KdsBasvuruDetayByYapiNumarasiQueryHandler : IRequestHandler<KdsHasarTespitVeriByUidQuery, ResultModel<KdsHaneModel>>
        {
            private readonly IMapper _mapper;
            private readonly IKdsHaneRepository _kdsHaneRepository;
            private readonly IKdsHasartespitTespitVeriRepository _kdsHasartespitTespitVeriRepository;
            private readonly IIstisnaAskiKoduRepository _istisnaAskiKoduRepository;
            private readonly IWebHostEnvironment _webHostEnvironment;
            private readonly ICacheService _cacheService;

            public KdsBasvuruDetayByYapiNumarasiQueryHandler(IMapper mapper, IKdsHaneRepository kdsHaneRepository, IWebHostEnvironment webHostEnvironment, IKdsHasartespitTespitVeriRepository kdsHasartespitTespitVeriRepository, ICacheService cacheService, IIstisnaAskiKoduRepository istisnaAskiKoduRepository)
            {
                _mapper = mapper;
                _kdsHaneRepository = kdsHaneRepository;
                _kdsHasartespitTespitVeriRepository = kdsHasartespitTespitVeriRepository;
                _webHostEnvironment = webHostEnvironment;
                _cacheService = cacheService;
                _istisnaAskiKoduRepository = istisnaAskiKoduRepository;
            }

            public async Task<ResultModel<KdsHaneModel>> Handle(KdsHasarTespitVeriByUidQuery request, CancellationToken cancellationToken)
            {
                var result = new ResultModel<KdsHaneModel>();

                if (string.IsNullOrWhiteSpace(request.HasarTespitUid))
                {
                    result.Exception(new ArgumentNullException("Uid Bilgisi Boş Olamaz."), "Uid Bilgisi Boş Olamaz");

                    return await Task.FromResult(result);
                }

                var cacheKey = $"{_webHostEnvironment.EnvironmentName}_" + $"{nameof(KdsHasarTespitVeriByUidQuery)}_Uid_{request.HasarTespitUid.Trim()}";
                var r = await _cacheService.GetValueAsync(cacheKey);
                if (r != null)
                    return JsonConvert.DeserializeObject<ResultModel<KdsHaneModel>>(r);

                #region ...: Uid'den HasarTespit Bilgilerinin Çekilmesi :...
                var hasarTespitVeri = _kdsHasartespitTespitVeriRepository.GetWhere(x =>
                            x.Uid == request.HasarTespitUid.Trim()
                            &&
                            x.AfetId == HasarTespitAfetEnum.KahramanmarasPazarcik20230206,
                        true).Select(s => new Hane
                        {
                            Uid = s.Uid,
                            AskiKodu = s.AskiKodu,
                            HasarDurumu = s.HasarDurumu,
                            ItirazSonucu = s.ItirazSonucu,
                            IlKod = s.IlKod,
                            IlAd = s.IlAd,
                            IlceKod = s.IlceKod,
                            IlceAd = s.IlceAd,
                            MahalleKod = s.MahalleKod,
                            MahalleAd = s.MahalleAd,
                            Sokak = s.Sokak,
                            DisKapiNo = s.DisKapiNo,
                            AdaNo = s.AdaNo,
                            ParselNo = s.ParselNo,
                            Uavtkod = s.Uavtkod,
                            GuclendirmeMahkemeSonucu = s.GuclendirmeMahkemeSonucu
                        }).FirstOrDefault() ?? _kdsHaneRepository
                    .GetWhere(x =>
                            x.Uid == request.HasarTespitUid.Trim()
                            &&
                            x.AfetId == HasarTespitAfetEnum.KahramanmarasPazarcik20230206,
                        true).FirstOrDefault();
                #endregion

                if (hasarTespitVeri == null)
                    result.ErrorMessage("Kayıt Bulunamadı.");
                else
                {
                    var basvuruYapilabilecekDurumlar = new List<string> { "Orta Hasarlı", "Ağır Hasarlı", "Acil Yıktırılacak", "Yıkık" };

                    var istisnaAskiKoduListesi = _istisnaAskiKoduRepository.GetAll();
                    result.Result = new KdsHaneModel();
                    if (istisnaAskiKoduListesi.Any(x => x.AskiKodu == hasarTespitVeri.AskiKodu)) result.Result.BasvuruYapabilirMi = true;
                    else
                    {

                        if (string.IsNullOrWhiteSpace(hasarTespitVeri.ItirazSonucu) || hasarTespitVeri.ItirazSonucu?.Trim() == "Hasara İtiraz Yoktur" || hasarTespitVeri.ItirazSonucu?.Trim() == "Tespit Yapılamadı")
                        {
                            if (basvuruYapilabilecekDurumlar.Contains(hasarTespitVeri.HasarDurumu?.Trim()) || basvuruYapilabilecekDurumlar.Contains(hasarTespitVeri.GuclendirmeMahkemeSonucu?.Trim()))
                            {
                                result.Result.BasvuruYapabilirMi = true;
                            }
                            else
                            {
                                result.Result.BasvuruYapabilirMi = false;
                                result.Result.BilgilendirmeMesaji = "Yalnızca Orta Hasarlı, Ağır Hasarlı, Acil Yıktırılacak ve Yıkık Binalar İçin Başvuru Yapabilirsiniz. Askı Koduna Ait Veri Başvuru Yapmaya Uygun Değildir.";
                            }
                        }
                        else if (basvuruYapilabilecekDurumlar.Contains(hasarTespitVeri.ItirazSonucu?.Trim()) || basvuruYapilabilecekDurumlar.Contains(hasarTespitVeri.GuclendirmeMahkemeSonucu?.Trim()))
                        {
                            result.Result.BasvuruYapabilirMi = true;
                        }
                        else
                        {
                            result.Result.BasvuruYapabilirMi = false;
                            result.Result.BilgilendirmeMesaji = "Yalnızca Orta Hasarlı, Ağır Hasarlı, Acil Yıktırılacak ve Yıkık Binalar İçin Başvuru Yapabilirsiniz. Askı Koduna Ait Veri Başvuru Yapmaya Uygun Değildir.";
                        }
                    }
                    result.Result.Detay = _mapper.Map<KdsHaneDetayModel>(hasarTespitVeri);
                }

                await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(result), TimeSpan.FromMinutes(15));

                return await Task.FromResult(result);
            }
        }
    }
}