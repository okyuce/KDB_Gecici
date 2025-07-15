using AutoMapper;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Application.Models.DataTable;
using Csb.YerindeDonusum.Application.Models.PagedListExtension;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Transactions;

namespace Csb.YerindeDonusum.Application.Extensions;

public static class PagedListExtension
{
    private static readonly int TakeDefault = 10;

    public static async Task<ResultModel<DataTableResponseModel<List<T>>>> Paginate<T, K>(this IQueryable<K> query, DataTableModel? request, IMapper _mapper)
    {
        int toplamKayit = query.Count();

        var pagedList = _mapper.ProjectTo<T>(query.Skip(request?.start ?? 0).Take(request?.length ?? TakeDefault)).ToList();
        var response = new ResultModel<DataTableResponseModel<List<T>>>(new DataTableResponseModel<List<T>>
        {
            data = pagedList,
            draw = request?.draw ?? 0,
            recordsFiltered = toplamKayit,
            recordsTotal = toplamKayit
        });

        return await Task.FromResult(response);
    }
    
    public static async Task<ResultModel<DataTableResponseModel<List<T>>>> Paginate<T>(this IQueryable<T> query, DataTableModel? request)
    {
        int toplamKayit = query.Count();

        var pagedList = query.Skip(request?.start ?? 0).Take(request?.length ?? TakeDefault).ToList();

        var response = new ResultModel<DataTableResponseModel<List<T>>>(new DataTableResponseModel<List<T>>
        {
            data = pagedList,
            draw = request?.draw ?? 0,
            recordsFiltered = toplamKayit,
            recordsTotal = toplamKayit
        });

        return await Task.FromResult(response);
    }    
    public static async Task<ResultModel<DataTableResponseModel<List<T>>>> Paginate<T>(this IEnumerable<T?> query, DataTableModel? request)
    {
        int toplamKayit = query.Count();

        var pagedList = query.Skip(request?.start ?? 0).Take(request?.length ?? TakeDefault).ToList();

        var response = new ResultModel<DataTableResponseModel<List<T>>>(new DataTableResponseModel<List<T>>
        {
            data = pagedList,
            draw = request?.draw ?? 0,
            recordsFiltered = toplamKayit,
            recordsTotal = toplamKayit
        });

        return await Task.FromResult(response);
    }

    /// <summary>
    /// Belirtilen sayfa numarası <paramref name="sayfaSayisi"/> ve sayfa büyüklüğüne göre <paramref name="sayfaBoyutu"/> kaynak listeyi <paramref name="kaynak"/> <see cref="PagedList{T}"/> ye dönüştürür.
    /// </summary>
    /// <typeparam name="T">Kaynak tipi.</typeparam>
    /// <param name="kaynak">Kaynak.</param>
    /// <param name="sayfaSayisi">Sayfa numarası.</param>
    /// <param name="sayfaBoyutu">Sayfa büyüklüğü. Sayfadaki Max. Kayıt Sayısı</param
    public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> kaynak, int sayfaSayisi, int sayfaBoyutu, CancellationToken cancellationToken = default(CancellationToken))
    {
        if (sayfaSayisi <= 0)
        {
            throw new ArgumentException($"sayfaSayisi: {sayfaSayisi} <= 0 , sayfaSayisi > 0 olmalı.");
        }

        var count = await kaynak.CountAsync(cancellationToken).ConfigureAwait(false);
        var items = await kaynak.Skip((sayfaSayisi - 1) * sayfaBoyutu)
                                .Take(sayfaBoyutu).ToListAsync(cancellationToken).ConfigureAwait(false);

        var pagedList = new PagedList<T>()
        {
            SayfaNo = sayfaSayisi,
            SayfaBoyutu = sayfaBoyutu,
            ToplamKayitSayisi = count,
            VeriListesi = items,
            ToplamSayfaSayisi = (int)Math.Ceiling(count / (double)sayfaBoyutu)
        };

        return pagedList;
    }
   
    /// <summary>
    /// Belirtilen sayfa numarası <paramref name="sayfaSayisi"/> ve sayfa büyüklüğüne göre <paramref name="sayfaBoyutu"/> kaynak listeyi <paramref name="kaynak"/> <see cref="PagedList{T}"/> ye dönüştürür. Kirli veri bulunabilir.
    /// </summary>
    /// <typeparam name="T">Kaynak tipi.</typeparam>
    /// <param name="kaynak">Kaynak.</param>
    /// <param name="sayfaSayisi">Sayfa numarası.</param>
    /// <param name="sayfaBoyutu">Sayfa büyüklüğü. Sayfadaki Max. Kayıt Sayısı</param>
    public static async Task<PagedList<T>> ToPagedListWithNoLockAsync<T>(this IQueryable<T> kaynak, int sayfaSayisi, int sayfaBoyutu, CancellationToken cancellationToken = default(CancellationToken))
    {
        if (sayfaSayisi <= 0)
        {
            throw new ArgumentException($"sayfaSayisi: {sayfaSayisi} <= 0 , sayfaSayisi > 0 olmalı.");
        }

        var count = await kaynak.CountAsync(cancellationToken).ConfigureAwait(false);
        var items = await kaynak.Skip((sayfaSayisi - 1) * sayfaBoyutu)
            .Take(sayfaBoyutu).ToListWithNoLockAsync(cancellationToken).ConfigureAwait(false);

        var pagedList = new PagedList<T>()
        {
            SayfaNo = sayfaSayisi,
            SayfaBoyutu = sayfaBoyutu,
            ToplamKayitSayisi = count,
            VeriListesi = items,
            ToplamSayfaSayisi = (int)Math.Ceiling(count / (double)sayfaBoyutu)
        };

        return pagedList;
    }
    
    public static async Task<List<T>> ToListWithNoLockAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken = default, Expression<Func<T, bool>> expression = null)
    {
        List<T> result = default;
        using (var scope = CreateTrancation())
        {
            if (expression != null)
            {
                query = query.Where(expression);
            }
            result = await query.ToListAsync(cancellationToken);
            scope.Complete();
        }
        return result;
    }
    
    private static TransactionScope CreateTrancation()
    {
        return new TransactionScope(TransactionScopeOption.Required,
            new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            },
            TransactionScopeAsyncFlowOption.Enabled);
    }
    
    /// <summary>
    /// Belirtilen sayfa numarası <paramref name="pageIndex"/> ve sayfa büyüklüğüne göre <paramref name="sayfaBoyutu"/> kaynak listeyi <paramref name="kaynak"/> <see cref="IPagedList{T}"/> ye dönüştürür.
    /// </summary>
    /// <typeparam name="T">Kaynak tipi.</typeparam>
    /// <param name="kaynak">Kaynak.</param>
    /// <param name="sayfaSayisi">Sayfa numarası.</param>
    /// <param name="sayfaBoyutu">Sayfa büyüklüğü.</param>
    public static PagedList<T> ToPagedList<T>(this IEnumerable<T> kaynak, int sayfaSayisi, int sayfaBoyutu) => new PagedList<T>(kaynak, sayfaSayisi, sayfaBoyutu);
}