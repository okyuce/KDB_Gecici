using Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirAlanByTakbisTasinmazIdQuery;
using Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirListeAnaTasinmaz;
using Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirTasinmazByTakbisTasinmazId;
using Csb.YerindeDonusum.Application.Models.Takbis;

namespace Csb.YerindeDonusum.Application.Interfaces.InfrastructureInterfaces;

public interface ITakbisService
{
    Task<List<IlModel>> GetirListeTakbisIlAsnyc();

    Task<List<IlModel>> GetirListeTakbisDepremIlAsnyc();

    Task<List<IlceModel>> GetirListeTakbisIlceByTakbisIlIdAsync(int takbisIlId);

    List<AnaTasinmazModel> GetirAnaTasinmaz(GetirAnaTasinmazQueryModel request);

    Task<List<TasinmazModel>> GetirBagimsizBolumAsync(GetirBagimsizBolumModel request);

    Task<List<AlanModel>> GetirListeAlanByTakbisTasinmazIdAsync(GetirAlanByTakbisTasinmazIdQueryModel request);

    Task<List<HisseModel>> GetirHisseByTakbisTasinmazIdAsync(GetirHisseTasinmazIdDenQueryModel request);

    Task<AnaTasinmazModel> GetirTasinmazByTakbisTasinmazIdAsync(GetirTasinmazByTakbisTasinmazIdQueryModel request);

    Task<List<MahalleModel>> GetirListeTakbisMahalleByTakbisIlceIdAsync(int takbisIlceId);

    Task<List<AdaModel>> GetirListeTakbisAdaByTakbisMahalleIdAsync(int takbisMahalleId);

    Task<List<ParselModel>> GetirListeTakbisParselByTakbisMahalleIdAdaNoAsync(int takbisMahalleId, string adaNo);

    Task<List<GercekKisiModel>> GetirGercekKisiAsync(string tcKimlikNo);

    Task<GercekKisiModel?> GetirGercekKisiIDDenAsync(decimal id);

    Task<TuzelKisiModel?> GetirTuzelKisiIDDenAsync(decimal id);

    Task<List<HisseModel>> GetirHisseMalikIDDenAsync(decimal malikId);

    Task<List<TasinmazModel>> GetirTasinmazHiseliMalikIDDenAsync(decimal malikId);
}