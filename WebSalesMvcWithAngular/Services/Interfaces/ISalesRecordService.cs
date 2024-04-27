using WebSalesMvc.Models;

namespace WebSalesMvcWithAngular.Services.Interfaces
{
    public interface ISalesRecordService
    {
        Task<List<SalesRecord>> FindAllAsync();
        Task<List<SalesRecord>> FindAllPaginatedAsync(int pageNumber, int pageSize);
        Task<int> CountAllAsync();
        Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate);

        Task<List<IGrouping<Department, SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate);
        Task<int> InsertAsync(SalesRecord salesRecord);
        Task UpdateAsync(SalesRecord sale);
        Task DeleteAsync(int id);
       
        Task<SalesRecord> FindByIdAsync(int? id);

        Task<(List<SalesRecord> Results, int TotalCount)> FindAlltoInvoiceAsync(int pageNumber = 1, int pageSize = 10);



    }
}
