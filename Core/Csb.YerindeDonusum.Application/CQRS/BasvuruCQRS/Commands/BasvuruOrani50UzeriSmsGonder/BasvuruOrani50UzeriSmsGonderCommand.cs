using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.Kds;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Entities;
using CSB.Core.Utilities.Messaging;
using MediatR;
using Serilog;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Commands.BasvuruOrani50UzeriSmsGonder;

public class BasvuruOrani50UzeriSmsGonderCommand : IRequest<ResultModel<string>>
{
    public class BasvuruOrani50UzeriSmsGonderCommandHandler : IRequestHandler<BasvuruOrani50UzeriSmsGonderCommand, ResultModel<string>>
    {
        private readonly ISmsLogRepository _smsLogRepository;
        private readonly IKdsYerindedonusumBinabazliOranRepository _kdsYerindedonusumBinabazliOranRepository;
        private readonly IKdsBasvuruRepository _kdsBasvuruRepository;
        private readonly ISMSService _smsService;

        public BasvuruOrani50UzeriSmsGonderCommandHandler(ISmsLogRepository smsLogRepository, IKdsYerindedonusumBinabazliOranRepository kdsYerindedonusumBinabazliOranRepository, IKdsBasvuruRepository kdsBasvuruRepository, ISMSService smsService)
        {
            _smsLogRepository = smsLogRepository;
            _kdsYerindedonusumBinabazliOranRepository = kdsYerindedonusumBinabazliOranRepository;
            _kdsBasvuruRepository = kdsBasvuruRepository;
            _smsService = smsService;
        }

        public async Task<ResultModel<string>> Handle(BasvuruOrani50UzeriSmsGonderCommand request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<string>();

            var telefonListe = (
                    from oran in _kdsYerindedonusumBinabazliOranRepository.GetAllQueryable().AsQueryable()
                    join basvuru in _kdsBasvuruRepository.GetAllQueryable().AsQueryable() on new { oran.UavtMahalleNo, oran.HasarTespitAskiKodu } equals new { basvuru.UavtMahalleNo, basvuru.HasarTespitAskiKodu }
                    where
                        oran.Oran > 50
                        &&
                        basvuru.BasvuruDurumId == (long)BasvuruDurumEnum.BasvurunuzAlinmistir
                        &&
                        basvuru.AktifMi == true
                        &&
                        basvuru.SilindiMi == false
                    select
                        basvuru.CepTelefonu
                ).Distinct().ToList();

            foreach (var telefon in telefonListe)
            {
                if (!_smsLogRepository.GetWhere(x => x.Telefon == telefon && x.GonderildiMi, false).Any())
                {
                    try
                    {
                        var smsMessage = new SMSMessage
                        {
                            To = telefon,
                            Content = $"Yerinde Dönüşüm Projesi kapsamında; müracaatınız alınmış olup, bulunduğunuz parselde salt çoğunluk sağlanmıştır. Proje ve ekleri ile birlikte Yerinde Dönüşüm Ofislerimize veya Çevre, Şehircilik ve İklim Değişikliği İl Müdürlüklerine başvurabilirsiniz."
                        };

                        var smsResult = await _smsService.SendAsync(smsMessage);

                        //bulkinsert yapılmadı çünkü devuser her zaman aktif değil
                        await _smsLogRepository.AddAsync(new SmsLog
                        {
                            Telefon = telefon,
                            Icerik = smsMessage.Content,
                            ApiSmsId = smsResult.ApiSmsId,
                            GonderildiMi = smsResult.IsSuccess,
                            ApiMesaj = smsResult.Message,
                            OlusturmaTarihi = DateTime.Now
                        });
                    }
                    catch (Exception exception)
                    {
                        Log.Error("{Exception}", exception);
                    }
                }
            }

            await _smsLogRepository.SaveChanges(cancellationToken);

            return await Task.FromResult(result);
        }
    }
}