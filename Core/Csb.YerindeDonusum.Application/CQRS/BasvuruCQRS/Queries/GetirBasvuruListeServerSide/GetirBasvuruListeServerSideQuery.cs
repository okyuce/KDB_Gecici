using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.YeniYapiCQRS.Queries.GetirListeYeniYapiServerSideGroupped;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Application.Models.DataTable;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using static Csb.YerindeDonusum.Application.Extensions.FluentValidationExtension;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Queries.GetirBasvuruListeServerSide;

public class GetirBasvuruListeServerSideQuery : DataTableModel, IRequest<ResultModel<DataTableResponseModel<List<GetirBasvuruListeServerSideResponseModel>>>>
{
    public long? BasvuruId { get; set; }
    public Guid? BasvuruGuid { get; set; }
    public string? BasvuruKodu { get; set; }
    public string? Ad { get; set; }
    public string? EPosta { get; set; }
    public string? CepTelefonu { get; set; }
    public string? TuzelKisiVergiNo { get; set; }
    public string? TuzelKisiMersisNo { get; set; }
    public long? BasvuruDestekTurId { get; set; }
    public long? BasvuruTurId { get; set; }
    public List<long>? BasvuruDurumId { get; set; }
    public long? BasvuruAfadDurumId { get; set; }
    public List<long>? BasvuruDegerlendirmeDurumId { get; set; }
    public bool? IptalEdilenGelmesin { get; set; }
    public bool? BasvuruDegerlendirmeDurumOnay { get; set; }
    public string? TcKimlikNo { get; set; }
    //public long? BasvuruMinOran { get; set; }
    public bool? MaskelemeKapaliMi { get; set; } = true;
    public int? UavtIlNo { get; set; }
    public int? UavtIlceNo { get; set; }
    public int? UavtMahalleNo { get; set; }
    public string? UavtBinaAda { get; set; }
    public string? UavtBinaParsel { get; set; }
    public string? TapuAda { get; set; }
    public string? TapuParsel { get; set; }
    public string? HasarTespitAskiKodu { get; set; }
    public long? BasvuruKanalId { get; set; }
    public long? OlusturanKullaniciId { get; set; }
    public long? GuncelleyenKullaniciId { get; set; }
    public DateTime? OlusturmaTarihi { get; set; }
    public string? HasarTespitHasarDurumu { get; set; }
    public string? BasvuruIptalAciklamasi { get; set; }
    public bool? AktifMi { get; set; }

    public class GetAllAppealByIdentificationNumberHandler : IRequestHandler<GetirBasvuruListeServerSideQuery, ResultModel<DataTableResponseModel<List<GetirBasvuruListeServerSideResponseModel>>>>
    {
        private readonly IMapper _mapper;
        private readonly IBasvuruRepository _repository;
        private readonly IKullaniciRepository _kullaniciRepository;
        private readonly IKullaniciBilgi _kullaniciBilgi;

        public GetAllAppealByIdentificationNumberHandler(IKullaniciBilgi kullaniciBilgi, IMapper mapper, IBasvuruRepository appealRepository, IKullaniciRepository kullaniciRepository)
        {
            _kullaniciBilgi = kullaniciBilgi;
            _mapper = mapper;
            _repository = appealRepository;
            _kullaniciRepository = kullaniciRepository;
        }

        public async Task<ResultModel<DataTableResponseModel<List<GetirBasvuruListeServerSideResponseModel>>>> Handle(GetirBasvuruListeServerSideQuery request, CancellationToken cancellationToken)
        {
            var kullaniciBilgi = _kullaniciBilgi.GetUserInfo();

            if (kullaniciBilgi.BirimIlId > 0)
                request.UavtIlNo = kullaniciBilgi.BirimIlId;

            var query = _repository.GetWhere(x => x.SilindiMi == false && x.AktifMi == true,
                        true,
                        i => i.BasvuruDurum,
                        i => i.BasvuruTur,
                        i => i.BasvuruIptalTur,
                        i => i.BasvuruDestekTur,
                        i => i.BasvuruKanal,
                        i => i.BasvuruAfadDurum
                        //i => i.BasvuruDegerlendirmeDurum
                    )
                    .OrderByDescending(o => o.OlusturmaTarihi)
                    .AsQueryable();

            if (NotEmpty(request?.BasvuruId))
                query = query.Where(x => request.BasvuruId == x.BasvuruId);
            if (NotEmpty(request?.BasvuruGuid))
                query = query.Where(x => request.BasvuruGuid == x.BasvuruGuid);
            if (NotWhiteSpace(request?.BasvuruKodu))
                query = query.Where(x => request.BasvuruKodu == x.BasvuruKodu);
            if (NotWhiteSpace(request?.Ad))
                query = query.Where(x => string.Concat(x.Ad, " ", x.Soyad).ToLower().Contains(request.Ad.Trim().ToLower())
                                      || x.TuzelKisiAdi.ToLower().Contains(request.Ad.Trim().ToLower()));
            if (NotWhiteSpace(request?.EPosta))
                query = query.Where(x => x.Eposta.ToLower().Contains(request.EPosta.Trim().ToLower()));
            if (NotWhiteSpace(request?.CepTelefonu))
                query = query.Where(x => x.CepTelefonu.Contains(request.CepTelefonu.Trim()));
            if (NotWhiteSpace(request?.TuzelKisiVergiNo))
                query = query.Where(x => x.TuzelKisiVergiNo.ToLower().Contains(request.TuzelKisiVergiNo.Trim().ToLower()));
            if (NotWhiteSpace(request?.TuzelKisiMersisNo))
                query = query.Where(x => x.TuzelKisiMersisNo.ToLower().Contains(request.TuzelKisiMersisNo.Trim().ToLower()));
            if (NotEmpty(request?.BasvuruDestekTurId))
                query = query.Where(x => request.BasvuruDestekTurId == x.BasvuruDestekTurId);
            if (NotEmpty(request?.BasvuruTurId))
                query = query.Where(x => request.BasvuruTurId == x.BasvuruTurId);
            if (NotWhiteSpace(request?.TcKimlikNo))
                query = query.Where(x => x.TcKimlikNo.Contains(request.TcKimlikNo.Trim()));
            if (NotEmpty(request?.UavtIlNo))
                query = query.Where(x => request.UavtIlNo == x.UavtIlNo);
            if (NotEmpty(request?.UavtIlceNo))
                query = query.Where(x => request.UavtIlceNo == x.UavtIlceNo);
            if (NotEmpty(request?.UavtMahalleNo))
                query = query.Where(x => request.UavtMahalleNo == x.UavtMahalleNo);
            if (NotWhiteSpace(request?.UavtBinaAda))
                query = query.Where(x => request.UavtBinaAda.Trim() == x.UavtBinaAda.Trim());
            if (NotWhiteSpace(request?.UavtBinaParsel))
                query = query.Where(x => request.UavtBinaParsel.Trim() == x.UavtBinaParsel);
            if (NotWhiteSpace(request?.TapuAda))
                query = query.Where(x => request.TapuAda.Trim() == x.TapuAda.Trim());
            if (NotWhiteSpace(request?.TapuParsel))
                query = query.Where(x => request.TapuParsel.Trim() == x.TapuParsel);
            if (NotWhiteSpace(request?.HasarTespitAskiKodu))
                query = query.Where(x => request.HasarTespitAskiKodu.Trim() == x.HasarTespitAskiKodu);
            if (NotEmpty(request?.BasvuruKanalId))
                query = query.Where(x => request.BasvuruKanalId == x.BasvuruKanalId);
            if (request?.IptalEdilenGelmesin == true)
                query = query.Where(x => x.BasvuruDurumId == (long)BasvuruDurumEnum.BasvurunuzAlinmistir
                                      || x.BasvuruDurumId == (long)BasvuruDurumEnum.BasvurunuzDegerlendirmeAsamasindadir
                                      || x.BasvuruDurumId == (long)BasvuruDurumEnum.BasvurunuzOnaylanmistir);
            if (request?.BasvuruDurumId?.Any() == true)
                query = query.Where(x => request.BasvuruDurumId.Any(y => y == x.BasvuruDurumId));
            //if (request?.BasvuruDegerlendirmeDurumId?.Count() > 0)
            //    query = query.Where(x => request.BasvuruDegerlendirmeDurumId.Any(y => y == x.BasvuruDegerlendirmeDurumId));
            if (NotEmpty(request?.BasvuruAfadDurumId))
                query = query.Where(x => request.BasvuruAfadDurumId == x.BasvuruAfadDurumId);
            if (NotEmpty(request?.OlusturanKullaniciId))
                query = query.Where(x => request.OlusturanKullaniciId == x.OlusturanKullaniciId);
            if (NotEmpty(request?.GuncelleyenKullaniciId))
                query = query.Where(x => request.GuncelleyenKullaniciId == x.GuncelleyenKullaniciId);
            if (NotWhiteSpace(request?.HasarTespitHasarDurumu))
                query = query.Where(x => request.HasarTespitHasarDurumu.Trim() == x.HasarTespitHasarDurumu);
            //if (NotEmpty(request?.BasvuruMinOran))
            //    query = query.Where(x => request.BasvuruMinOran <= x.TapuToplamKisiHisseOrani);
            if (NotEmpty(request?.OlusturmaTarihi))
                query = query.Where(x => x.OlusturmaTarihi >= request.OlusturmaTarihi.Value && x.OlusturmaTarihi < request.OlusturmaTarihi.Value.AddDays(1));

            //if (request?.BasvuruDegerlendirmeDurumOnay == true)
            //    query = query.Where(x => x.BasvuruDurumId == (long)BasvuruDurumEnum.BasvurunuzOnaylanmistir
            //                          || x.BasvuruDegerlendirmeDurumId == (long)BasvuruDegerlendirmeDurumEnum.Onaylandi
            //                          );
            //else if (request?.BasvuruDegerlendirmeDurumOnay == false)
            //    query = query.Where(x => x.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzOnaylanmistir
            //                            && x.BasvuruDegerlendirmeDurumId != (long)BasvuruDegerlendirmeDurumEnum.Onaylandi
            //                          );

            if (request?.length > 5000)
            {
                //sadece dışarı aktarmada count alalım, dışarı aktarılmıyorsa boşuna yormayalım diye içerideki if şartına taşındı
                if (query.Count() > 5000)
                {
                    var result = new ResultModel<DataTableResponseModel<List<GetirBasvuruListeServerSideResponseModel>>>();
                    result.ErrorMessage("Tek seferde en fazla 5.000 kayıt dışarı aktarılabilir.");
                    return await Task.FromResult(result);
                }
            }

            //return await query.Paginate<GetirBasvuruListeServerSideResponseModel, Basvuru>(request, _mapper);

            var paginateData = await query.Paginate<GetirBasvuruListeServerSideResponseModel, Basvuru>(request, _mapper);

            var olusturanKullaniciIds = paginateData.Result.data.Select(p => p.OlusturanKullaniciId).Distinct().ToList();
            var guncelleyenKullaniciIds = paginateData.Result.data.Where(p => p.GuncelleyenKullaniciId.HasValue).Select(p => p.GuncelleyenKullaniciId.Value).Distinct().ToList();
            olusturanKullaniciIds.AddRange(guncelleyenKullaniciIds);

            var kullanicilar = _kullaniciRepository.GetWhere(x => olusturanKullaniciIds.Contains(x.KullaniciId)).Distinct().ToList();

            paginateData.Result.data.ForEach(x =>
            {
                x.OlusturanKullaniciAdi = kullanicilar.Where(p => p.KullaniciId == x.OlusturanKullaniciId).FirstOrDefault()?.KullaniciAdi;
                x.GuncelleyenKullaniciAdi = kullanicilar.Where(p => p.KullaniciId == x.GuncelleyenKullaniciId).FirstOrDefault()?.KullaniciAdi;
            });

            return paginateData;
        }
    }
}