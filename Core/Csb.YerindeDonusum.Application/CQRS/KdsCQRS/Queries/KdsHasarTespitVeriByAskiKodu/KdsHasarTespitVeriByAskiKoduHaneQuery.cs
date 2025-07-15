using AutoMapper;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.Kds;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Entities.Kds;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Csb.YerindeDonusum.Application.CQRS.KdsCQRS.Queries.KdsHasarTespitVeriByAskiKodu
{
    public class KdsHasarTespitVeriByAskiKoduHaneQuery : IRequest<ResultModel<KdsHaneModel>>
    {
        public string? AskiKodu { get; set; }

        public class KdsHasarTespitVeriByAskiKoduHaneQueryHandler : IRequestHandler<KdsHasarTespitVeriByAskiKoduHaneQuery, ResultModel<KdsHaneModel>>
        {
            private readonly IMapper _mapper;
            private readonly IKdsHaneRepository _kdsHaneRepository;
            private readonly IIstisnaAskiKoduRepository _istisnaAskiKoduRepository;
            private readonly IWebHostEnvironment _webHostEnvironment;
            private readonly ICacheService _cacheService;

            public KdsHasarTespitVeriByAskiKoduHaneQueryHandler(IMapper mapper, IKdsHaneRepository kdsHaneRepository, IWebHostEnvironment webHostEnvironment, ICacheService cacheService, IIstisnaAskiKoduRepository istisnaAskiKoduRepository)
            {
                _mapper = mapper;
                _kdsHaneRepository = kdsHaneRepository;
                _webHostEnvironment = webHostEnvironment;
                _cacheService = cacheService;
                _istisnaAskiKoduRepository = istisnaAskiKoduRepository;
            }

            public async Task<ResultModel<KdsHaneModel>> Handle(KdsHasarTespitVeriByAskiKoduHaneQuery request, CancellationToken cancellationToken)
            {
                var result = new ResultModel<KdsHaneModel>();

                request.AskiKodu = HasarTespitAddon.AskiKoduToUpper(request.AskiKodu);

                var cacheKey = $"{_webHostEnvironment.EnvironmentName}_" + $"{nameof(KdsHasarTespitVeriByAskiKoduHaneQuery)}_AskiKodu_{request.AskiKodu}";
                var r = await _cacheService.GetValueAsync(cacheKey);
                if (r != null)
                    return JsonConvert.DeserializeObject<ResultModel<KdsHaneModel>>(r);

                #region ...: Askı Kodundan HasarTespit Bilgilerinin Çekilmesi :...
                Hane? hasarTespitVeri = null;

                var queryHasarTespitVeriListe = _kdsHaneRepository
                       .GetWhere(x => x.AfetId == HasarTespitAfetEnum.KahramanmarasPazarcik20230206 && x.AskiKodu != null, true)
                        .OrderBy(p => p.Uid);

                if (request.AskiKodu.Trim().Contains("-"))
                {
                    if (request.AskiKodu.Trim().EndsWith("-1"))
                    {
                        #region ...: Askı Kodu -1 ile bitiyor ise mükerrer askı kodu olan kaydın ilki dönülecek :...
                        var gercekAskiKodu = request.AskiKodu.Trim().Replace("-1", string.Empty).Trim();

                        if (gercekAskiKodu.IsNullOrEmpty())
                        {
                            result.ErrorMessage("Hatalı Askı Kodu girildi.");

                            await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(result), TimeSpan.FromMinutes(60));

                            return await Task.FromResult(result);
                        }

                        var hasarTespitVeriListeSonuc = queryHasarTespitVeriListe.Where(x => x.AskiKodu.Contains(gercekAskiKodu)).ToList();

                        if (hasarTespitVeriListeSonuc.Any())
                        {
                            hasarTespitVeri = hasarTespitVeriListeSonuc.FirstOrDefault();
                        }
                        else
                        {
                            result.ErrorMessage($"Sistemde {gercekAskiKodu} ile kayıtlı bir askı kodu bulunamadı.");

                            await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(result), TimeSpan.FromMinutes(60));

                            return await Task.FromResult(result);
                        }
                        #endregion
                    }
                    else if (request.AskiKodu.Trim().EndsWith("-2"))
                    {
                        #region ...: Askı Kodu -2 ile bitiyor ise mükerrer askı kodu olan kaydın ikincisi dönülecek :...
                        var gercekAskiKodu = request.AskiKodu.Trim().Replace("-2", "").Trim();
                        if (gercekAskiKodu.IsNullOrEmpty())
                        {
                            result.ErrorMessage("Hatalı Askı Kodu Girildi.");

                            await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(result), TimeSpan.FromMinutes(10));

                            return await Task.FromResult(result);
                        }

                        var hasarTespitVeriListeSonuc = queryHasarTespitVeriListe.Where(x => x.AskiKodu.Contains(gercekAskiKodu)).ToList();

                        if (hasarTespitVeriListeSonuc.Count > 1)
                        {
                            hasarTespitVeri = hasarTespitVeriListeSonuc.Skip(1).FirstOrDefault();
                        }
                        else if (hasarTespitVeriListeSonuc.Count == 1)
                        {
                            result.ErrorMessage($"Sistemde bulunan {gercekAskiKodu} askı kodu mükerrer değil.");

                            await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(result), TimeSpan.FromMinutes(60));

                            return await Task.FromResult(result);
                        }
                        else
                        {
                            result.ErrorMessage($"Sistemde {gercekAskiKodu} ile kayıtlı bir askı kodu bulunamadı.");

                            await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(result), TimeSpan.FromMinutes(10));

                            return await Task.FromResult(result);
                        }
                        #endregion
                    }
                }
                else
                {
                    #region ...: Askı Kodundan Hasar Tespit Bilgisi dönülecek. Askı Kodu Mükerrer ise -1 veya -2 ile sorgulanması istenecek:...

                    var hasarTespitVeriListeSonuc = queryHasarTespitVeriListe.Where(x => x.AskiKodu.Contains(request.AskiKodu.Trim())).ToList();

                    if (hasarTespitVeriListeSonuc.Count > 1)
                    {
                        List<string> listeIndeksliAskiKodu = new List<string>();

                        for (int i = 0; i < hasarTespitVeriListeSonuc.Count; i++)
                        {
                            listeIndeksliAskiKodu.Add($"{hasarTespitVeriListeSonuc[i].AskiKodu}-{i + 1}");//ABCDE-1,ABCDE-2
                        }

                        var askiKoduMukerrerMesaji = $"Askı Kodunuza Karşılık 1 den fazla tespit olduğundan {string.Join(" veya ", listeIndeksliAskiKodu)} askı kodları ile denemelisiniz.";
                        result.ErrorMessage(askiKoduMukerrerMesaji);

                        await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(result), TimeSpan.FromMinutes(60));

                        return result;
                    }

                    hasarTespitVeri = hasarTespitVeriListeSonuc.FirstOrDefault();
                    #endregion
                }
                #endregion

                if (hasarTespitVeri == null)
                    result.ErrorMessage("Kayıt Bulunamadı.");
                else
                {
                    //virgüllü askı kodları için eklendi. contains içeriyor fakat askı kodunu tam olarak karşılaması gerektiği için ek kontrol yapıldı
                    //askı kodu içerisinde "-1" ve "-2" karakterleri varsa temizleniyor ve kontrol ediliyor
                    if (!hasarTespitVeri.AskiKodu.Split(',').Any(s => s.Trim() == request.AskiKodu.Replace("-1", "").Replace("-2", "").Trim()))
                    {
                        result.ErrorMessage($"Sistemde {request.AskiKodu} ile kayıtlı bir askı kodu bulunamadı.");

                        await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(result), TimeSpan.FromMinutes(60));

                        return await Task.FromResult(result);
                    }

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

                await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(result), TimeSpan.FromMinutes(60));

                return await Task.FromResult(result);
            }
        }
    }
}