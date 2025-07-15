using AutoMapper;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Application.Models.DataTable;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirListeOdemeBekleyenDegerlendirmeServerSide;

public class GetirListeOdemeBekleyenDegerlendirmeServerSideQuery : DataTableModel, IRequest<ResultModel<DataTableResponseModel<List<GetirListeOdemeBekleyenDegerlendirmeServerSideResponseModel>>>>
{
    public int? UavtIlNo { get; set; }
    public int? UavtIlceNo { get; set; }
    public int? UavtMahalleNo { get; set; }
    public string? Ada { get; set; }
    public string? Parsel { get; set; }
    public string? BinaDisKapiNo { get; set; }
    public string? HasarTespitAskiKodu { get; set; }
    public long? BultenNo { get; set; }

    public class GetirListeOdemeBekleyenDegerlendirmeServerSideQueryHandler : IRequestHandler<GetirListeOdemeBekleyenDegerlendirmeServerSideQuery, ResultModel<DataTableResponseModel<List<GetirListeOdemeBekleyenDegerlendirmeServerSideResponseModel>>>>
    {
        private readonly IMapper _mapper;
        private readonly IBasvuruRepository _basvuruRepository;
        private readonly IBinaDegerlendirmeRepository _binaDegerlendirmeRepository;
        private readonly IKullaniciBilgi _kullaniciBilgi;

        public GetirListeOdemeBekleyenDegerlendirmeServerSideQueryHandler(IKullaniciBilgi kullaniciBilgi, IBinaDegerlendirmeRepository binaDegerlendirmeRepository, IMapper mapper, IBasvuruRepository basvuruRepository)
        {
            _kullaniciBilgi = kullaniciBilgi;
            _binaDegerlendirmeRepository = binaDegerlendirmeRepository;
            _mapper = mapper;
            _basvuruRepository = basvuruRepository;
        }

        public async Task<ResultModel<DataTableResponseModel<List<GetirListeOdemeBekleyenDegerlendirmeServerSideResponseModel>>>> Handle(GetirListeOdemeBekleyenDegerlendirmeServerSideQuery request, CancellationToken cancellationToken)
        {
            var kullaniciBilgi = _kullaniciBilgi.GetUserInfo();

            if (kullaniciBilgi.BirimIlId > 0)
                request.UavtIlNo = kullaniciBilgi.BirimIlId;

            bool genelMudurlukMu = kullaniciBilgi.BirimIlId > 0;

            // Genel müdürlük kullanıcıları BultenNo' su null olmayan ya da En Az 1 İzin Belgesi Olan BinaDegerlendirme' leri görebilir.
            // İl Müdürlük kullanıcıları ise (BultenNo' su null olmayanya da En Az 1 İzin Belgesi Olan)
            // ve en az 1 adet BinaYapiDenetimSeviyeTespits (Seviye) bilgisi girilmiş BinaDegerlendirme' leri görebilir.

            // Başvurusu olmayan değerlendirmeler aski koduna ve mahalleNo' ya göre alınıp BaşvuruDegerlendirmeId' leri bağlanıyor.
            var basvurusuOlmayanDegerlendirmeler = _binaDegerlendirmeRepository.GetWhere(x => x.SilindiMi == false && x.AktifMi == true
                                                        && (x.BultenNo > 0 || x.IzinBelgesiSayi > 0)
                                                        && !x.Basvurus.Any(y => y.SilindiMi == false && y.AktifMi == true 
                                                                && y.BasvuruAfadDurumId != (long)BasvuruAfadDurumEnum.Kabul
                                                                && y.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi
                                                                && y.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir)
                                        , true)
                                        .Include(x => x.Basvurus.Where(y => y.SilindiMi == false && y.AktifMi == true));
            
            if (basvurusuOlmayanDegerlendirmeler?.Any() == true)
            {
                var basvuruListe = _basvuruRepository.GetWhere(x => !x.SilindiMi && x.AktifMi == true
                                                                && x.BasvuruAfadDurumId != (long)BasvuruAfadDurumEnum.Kabul
                                                                && x.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi
                                                                && x.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir
                                                                && x.BinaDegerlendirmeId == null);

                if (basvuruListe?.Any() == true)
                {
                    var binaDegerlendirmeBosOlanBasvurular = (from binaDegerlendirme in basvurusuOlmayanDegerlendirmeler
                                                                from basvuru in basvuruListe.Where(x => x.UavtMahalleNo == binaDegerlendirme.UavtMahalleNo
                                                                            && x.HasarTespitAskiKodu == binaDegerlendirme.HasarTespitAskiKodu
                                                                            )
                                                                select new { BinaDegerlendirmeId = binaDegerlendirme.BinaDegerlendirmeId, Basvuru = basvuru }).ToList();

                    // bina değerledirme id bilgisini boş olan başvurulara ilgili başvurulara yazıyoruz
                    if (binaDegerlendirmeBosOlanBasvurular?.Any() == true)
                    {
                        foreach (var group in binaDegerlendirmeBosOlanBasvurular)
                        {
                            group.Basvuru.BinaDegerlendirmeId = group.BinaDegerlendirmeId;
                            _basvuruRepository.Update(group.Basvuru);
                        }
                        await _basvuruRepository.SaveChanges();
                    }                 
                }
            }

            // Tüm Başvurulara karşılık BasvuruImzaVerens tablosu ve BagimsizBolumNo olması gerekmektedir.
            var query = _binaDegerlendirmeRepository.GetWhere(x => x.SilindiMi == false && x.AktifMi == true
                                && (x.BultenNo > 0 || x.IzinBelgesiSayi > 0)
                                && x.Basvurus.All(y => y.SilindiMi == false && y.AktifMi == true
                                        && y.BasvuruAfadDurumId != (long)BasvuruAfadDurumEnum.Kabul
                                        && y.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi
                                        && y.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir
                                        && !string.IsNullOrWhiteSpace(y.BasvuruImzaVerens.FirstOrDefault(z => !z.SilindiMi && z.AktifMi == true).BagimsizBolumNo))
                                , true)
                    // Includedan bagimsiz bolum no kosulu kaldirildi, cunku sadece o veriler cekilirse her zaman kosul true olacak,
                    // bagimsiz bolumnosu olmayan verilerin de cekilmesi lazim
                    .Include(x => x.Basvurus.Where(y => y.SilindiMi == false && y.AktifMi == true))
                        .ThenInclude(x => x.BasvuruImzaVerens.Where(y => y.SilindiMi == false && y.AktifMi == true))
                    .Include(x => x.BinaOdemes.Where(y => y.SilindiMi == false && y.AktifMi == true))
                        .ThenInclude(x => x.BinaOdemeDurum)
                    .Include(x => x.BinaMuteahhits.Where(y => y.SilindiMi == false && y.AktifMi == true))
                    .Include(x => x.BinaYapiDenetimSeviyeTespits.Where(y=> y.SilindiMi == false && y.AktifMi == true))
                    .AsQueryable();

            // İl Müdürlüğü Rolüne sahipse en az bir seviye bilgisi girilen Başvuru Değerlendirmeleri görebilir.
            if (!genelMudurlukMu)
            {
                query = query.Where(x => x.BinaYapiDenetimSeviyeTespits.Any(y => y.SilindiMi == false && y.AktifMi == true));
            }

            if (FluentValidationExtension.NotEmpty(request.UavtIlNo))
            {
                query = query.Where(x => x.UavtIlNo == request.UavtIlNo);
            }
            if (FluentValidationExtension.NotEmpty(request.UavtIlceNo))
            {
                query = query.Where(x => x.UavtIlceNo == request.UavtIlceNo);
            }
            if (FluentValidationExtension.NotEmpty(request.UavtMahalleNo))
            {
                query = query.Where(x => x.UavtMahalleNo == request.UavtMahalleNo);
            }
            if (FluentValidationExtension.NotWhiteSpace(request.Ada))
            {
                query = query.Where(x => x.Ada == request.Ada);
            }
            if (FluentValidationExtension.NotWhiteSpace(request.Parsel))
            {
                query = query.Where(x => x.Parsel == request.Parsel);
            }
               
            if (FluentValidationExtension.NotWhiteSpace(request.BinaDisKapiNo))
            {
                query = query.Where(x => x.BinaDisKapiNo == request.BinaDisKapiNo);
            }
                      
            if (FluentValidationExtension.NotWhiteSpace(request.HasarTespitAskiKodu))
            {
                request.HasarTespitAskiKodu = HasarTespitAddon.AskiKoduToUpper(request.HasarTespitAskiKodu);
                query = query.Where(x => x.HasarTespitAskiKodu == request.HasarTespitAskiKodu);
            }
                
            if (FluentValidationExtension.NotEmpty(request.BultenNo))
                query = query.Where(x => x.BultenNo == request.BultenNo);

            return await query.Paginate<GetirListeOdemeBekleyenDegerlendirmeServerSideResponseModel, BinaDegerlendirme>(request, _mapper);
        }
    }
}