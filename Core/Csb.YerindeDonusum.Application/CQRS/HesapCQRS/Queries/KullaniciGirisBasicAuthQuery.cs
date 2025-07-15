using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Cryptography;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.HesapCQRS.Queries
{
    public class KullaniciGirisBasicAuthQuery : IRequest<ResultModel<KullaniciSonucDto>>
    {
        public string KullaniciAdi { get; set; } = string.Empty;
        public string Sifre { get; set; } = string.Empty;

        public class KullaniciGirisBasicAuthCommandHandler : IRequestHandler<KullaniciGirisBasicAuthQuery, ResultModel<KullaniciSonucDto>>
        {
            private readonly IKullaniciRepository _userRepository;

            public KullaniciGirisBasicAuthCommandHandler(IKullaniciRepository userRepository)
            {
                _userRepository = userRepository;
            }

            public async Task<ResultModel<KullaniciSonucDto>> Handle(KullaniciGirisBasicAuthQuery request, CancellationToken cancellationToken)
            {
                var result = new ResultModel<KullaniciSonucDto>();

                if (string.IsNullOrWhiteSpace(request.KullaniciAdi) || string.IsNullOrWhiteSpace(request.Sifre))
                {
                    result.Exception(new ArgumentNullException("Hatalı veya Geçersiz Parametre Bilgisi! ERR-ULC-100"));
                    return await Task.FromResult(result);
                }

                var userPassword = CsbCryptography.Sha256(CsbCryptography.Sha256(CsbCryptography.MD5(request.Sifre)));
                var userResult = _userRepository.GetAllQueryable(x =>
                    x.KullaniciAdi.Equals(request.KullaniciAdi) && x.Sifre.Equals(userPassword) && x.AktifMi == true &&
                    x.SilindiMi == false).FirstOrDefault();

                if (userResult == null)
                {
                    result.Exception(new NullReferenceException("Geçersiz veya Hatalı Kullanıcı Bilgisi! ERR-ULC-200"));
                    return await Task.FromResult(result);
                }

                result.Result = new KullaniciSonucDto
                {
                    KullaniciAdi = request.KullaniciAdi,
                    KullaniciId = userResult.KullaniciId,
                    TcKimlikNo = userResult.TcKimlikNo??0,
                };

                return result;
            }
        }
    }
}