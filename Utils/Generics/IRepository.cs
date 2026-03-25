using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Utils.Responses;

namespace Utils.Generics
{
    /// <summary>
    /// Veritabanı işlemleri (CRUD) için genel arayüz.
    /// Herhangi bir entity tipi üzerinde Create, Read, Update, Delete işlemleri gerçekleştirmek için kullanılır.
    /// </summary>
    /// <typeparam name="T">Veritabanı entity tipi (Kullanıcı, Ürün, Sipariş vb.)</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Tek bir entity'yi veritabanına ekler.
        /// </summary>
        /// <param name="entity">Eklenecek entity nesnesi</param>
        /// <returns>İşlem sonucu (başarılı/başarısız bilgisi)</returns>
        Task<IResult> CreateAsync(T entity);

        /// <summary>
        /// Birden fazla entity'yi veritabanına bir seferde ekler.
        /// </summary>
        /// <param name="entities">Eklenecek entity nesneleri listesi</param>
        /// <returns>İşlem sonucu (başarılı/başarısız bilgisi)</returns>
        Task<IResult> CreateManyAsync(IEnumerable<T> entities);

        /// <summary>
        /// Veritabanından belirtilen koşula göre birden fazla entity getirir.
        /// </summary>
        /// <param name="expression">Arama koşulu (örn: x => x.IsActive == true). Null ise tüm kayıtları getirir</param>
        /// <param name="includes">İlişkili verileri yüklemek için kullanılan navigation property isimleri (örn: "Orders", "Comments")</param>
        /// <returns>Bulunan entity'ler listesi ya da hata mesajı</returns>
        Task<IResult<IEnumerable<T>>> FindManyAsync(Expression<Func<T, bool>>? expression = null, params string[] includes);

        /// <summary>
        /// Veritabanından kimlik numarasına göre tek bir entity getirir.
        /// </summary>
        /// <param name="id">Entity'nin birincil anahtar değeri (ID)</param>
        /// <returns>Bulunan entity ya da hata mesajı</returns>
        Task<IResult<T>> FindByIdAsync(object id);

        /// <summary>
        /// Veritabanından belirtilen koşula uygun ilk entity'yi getirir.
        /// </summary>
        /// <param name="expression">Arama koşulu (örn: x => x.Email == "test@mail.com"). Null ise ilk kayıtı getirir</param>
        /// <returns>Bulunan entity ya da hata mesajı</returns>
        Task<IResult<T>> FindFirstAsync(Expression<Func<T, bool>>? expression = null);

        /// <summary>
        /// Veritabanında bulunan bir entity'yi günceller.
        /// </summary>
        /// <param name="entity">Güncellenecek entity nesnesi (güncellenmiş verilerle)</param>
        /// <returns>İşlem sonucu (başarılı/başarısız bilgisi)</returns>
        Task<IResult> UpdateAsync(T entity);

        /// <summary>
        /// Veritabanında bulunan birden fazla entity'yi günceller.
        /// </summary>
        /// <param name="entities">Güncellenecek entity nesneleri listesi</param>
        /// <returns>İşlem sonucu (başarılı/başarısız bilgisi)</returns>
        Task<IResult> UpdateManyAsync(IEnumerable<T> entities);

        /// <summary>
        /// Veritabanından kimlik numarasına göre bir entity'yi siler.
        /// </summary>
        /// <param name="id">Silinecek entity'nin birincil anahtar değeri (ID)</param>
        /// <returns>İşlem sonucu (başarılı/başarısız bilgisi)</returns>
        Task<IResult> DeleteAsync(object id);

        /// <summary>
        /// Veritabanından belirtilen entity nesnesi'ni siler.
        /// </summary>
        /// <param name="entity">Silinecek entity nesnesi</param>
        /// <returns>İşlem sonucu (başarılı/başarısız bilgisi)</returns>
        Task<IResult> DeleteAsync(T entity);

        /// <summary>
        /// Veritabanından birden fazla entity'yi siler.
        /// </summary>
        /// <param name="entities">Silinecek entity nesneleri listesi</param>
        /// <returns>İşlem sonucu (başarılı/başarısız bilgisi)</returns>
        Task<IResult> DeleteManyAsync(IEnumerable<T> entities);

        /// <summary>
        /// Veritabanından belirtilen koşula uygun tüm entity'leri siler.
        /// </summary>
        /// <param name="expression">Silme koşulu (örn: x => x.IsDeleted == true). Null ise tüm kayıtları siler</param>
        /// <returns>İşlem sonucu (başarılı/başarısız bilgisi)</returns>
        Task<IResult> DeleteManyAsync(Expression<Func<T, bool>>? expression = null);

        /// <summary>
        /// Veritabanındaki entity sayısını sayar.
        /// </summary>
        /// <param name="expression">Sayma koşulu (örn: x => x.Status == "Active"). Null ise tüm kayıtları sayar</param>
        /// <returns>Toplam entity sayısı ya da hata mesajı</returns>
        Task<IResult<int>> CountAsync(Expression<Func<T, bool>>? expression = null);

        /// <summary>
        /// Veritabanında belirtilen koşula uygun herhangi bir entity'nin olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="expression">Kontrol koşulu (örn: x => x.Email == "test@mail.com"). Null ise en az bir kayıt olup olmadığını kontrol eder</param>
        /// <returns>true/false değeri ya da hata mesajı</returns>
        Task<IResult<bool>> AnyAsync(Expression<Func<T, bool>>? expression = null);
    }

    /// <summary>
    /// IRepository arayüzünün temel uygulaması.
    /// Entity Framework Core kullanarak veritabanı işlemlerini gerçekleştirir.
    /// Bu sınıf kalıtım yoluyla kullanılmalıdır (abstract class olduğu için).
    /// </summary>
    /// <typeparam name="T">Veritabanı entity tipi</typeparam>
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        /// <summary>
        /// Entity Framework DbContext nesnesi. Veritabanı bağlantısını ve işlemlerini yönetir.
        /// </summary>
        protected readonly DbContext _db;

        /// <summary>
        /// Belirli entity tipi için DbSet. Bu entity'nin veritabanı tablosuna erişim sağlar.
        /// </summary>
        protected readonly DbSet<T> _table;

        /// <summary>
        /// Repository'nin yapıcı metodu. DbContext'i alır ve _table'ı başlatır.
        /// </summary>
        /// <param name="db">Kullanılacak DbContext örneği</param>
        protected Repository(DbContext db)
        {
            _db = db;
            _table = _db.Set<T>();
        }

        /// <summary>
        /// Veritabanında belirtilen koşula uygun herhangi bir kayıt olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="expression">Kontrol koşulu. Null ise en az bir kayıt olup olmadığını kontrol eder</param>
        /// <returns>true (kayıt var) / false (kayıt yok) bilgisi ya da hata</returns>
        public async Task<IResult<bool>> AnyAsync(Expression<Func<T, bool>>? expression = null)
        {
            try
            {
                bool any = expression != null ? await _table.AnyAsync(expression) : await _table.AnyAsync();
                return Result<bool>.Success(any);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure([ex.Message], 500);
            }
        }

        /// <summary>
        /// Veritabanında toplam kaç kayıt olduğunu sayar.
        /// </summary>
        /// <param name="expression">Sayma koşulu. Null ise tüm kayıtları sayar</param>
        /// <returns>Kayıt sayısı ya da hata</returns>
        public async Task<IResult<int>> CountAsync(Expression<Func<T, bool>>? expression = null)
        {
            try
            {
                int count = expression != null ? await _table.CountAsync(expression) : await _table.CountAsync();
                return Result<int>.Success(count);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure([ex.Message], 500);
            }
        }

        /// <summary>
        /// Veritabanına yeni bir kayıt ekler. DbContext.SaveChanges() çağrılmadığı sürece veritabanına yazılmaz.
        /// </summary>
        /// <param name="entity">Eklenecek entity nesnesi</param>
        /// <returns>201 Created status kodu ya da hata</returns>
        public async Task<IResult> CreateAsync(T entity)
        {
            try
            {
                await _table.AddAsync(entity);
                return Result.Success(201); // 201: Created
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message, 500);
            }
        }

        /// <summary>
        /// Veritabanına birden fazla yeni kayıt ekler. DbContext.SaveChanges() çağrılmadığı sürece veritabanına yazılmaz.
        /// </summary>
        /// <param name="entities">Eklenecek entity nesneleri listesi</param>
        /// <returns>201 Created status kodu ya da hata</returns>
        public async Task<IResult> CreateManyAsync(IEnumerable<T> entities)
        {
            try
            {
                await _table.AddRangeAsync(entities);
                return Result.Success(201); // 201: Created
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message, 500);
            }
        }

        /// <summary>
        /// Veritabanından ID'ye göre kayıt bulur ve siler.
        /// </summary>
        /// <param name="id">Silinecek kaydın birincil anahtar değeri</param>
        /// <returns>204 No Content status kodu ya da hata</returns>
        public async Task<IResult> DeleteAsync(object id)
        {
            var result = await FindByIdAsync(id);
            if (result.IsSuccess)
            {
                return await DeleteAsync(result.Data);
            }
            return result;
        }

        /// <summary>
        /// Verilen entity nesnesi'ni veritabanından siler.
        /// </summary>
        /// <param name="entity">Silinecek entity nesnesi</param>
        /// <returns>204 No Content status kodu ya da hata</returns>
        public async Task<IResult> DeleteAsync(T entity)
        {
            try
            {
                await Task.Run(() => _table.Remove(entity));
                return Result.Success(204);
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message, 500);
            }
        }

        /// <summary>
        /// Verilen birden fazla entity'yi veritabanından siler.
        /// </summary>
        /// <param name="entities">Silinecek entity nesneleri listesi</param>
        /// <returns>204 No Content status kodu ya da hata</returns>
        public async Task<IResult> DeleteManyAsync(IEnumerable<T> entities)
        {
            try
            {
                await Task.Run(() => _table.RemoveRange(entities));
                return Result.Success(204);
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message, 500);
            }
        }

        /// <summary>
        /// Belirtilen koşula uygun tüm kayıtları veritabanından siler.
        /// </summary>
        /// <param name="expression">Silme koşulu (örn: x => x.IsActive == false). Null ise tüm kayıtları siler</param>
        /// <returns>204 No Content status kodu ya da hata</returns>
        public async Task<IResult> DeleteManyAsync(Expression<Func<T, bool>>? expression = null)
        {
            var entities = expression == null ? _table : _table.Where(expression);

            if (entities == null)
            {
                return Result.Failure("Entities Not found!", 404);
            }

            return await DeleteManyAsync(entities);
        }

        /// <summary>
        /// Veritabanından ID'ye göre tek bir kayıt getirir.
        /// </summary>
        /// <param name="id">Getirilecek kaydın birincil anahtar değeri</param>
        /// <returns>Bulunan entity ya da "Entity Not found!" hata mesajı</returns>
        public async Task<IResult<T>> FindByIdAsync(object id)
        {
            var entity = await _table.FindAsync(id);

            if (entity == null)
            {
                return Result<T>.Failure(["Entity Not found!"], 404);
            }

            return Result<T>.Success(entity);
        }

        /// <summary>
        /// Belirtilen koşula uygun ilk kaydı veritabanından getirir.
        /// </summary>
        /// <param name="expression">Arama koşulu (örn: x => x.Email == "test@mail.com"). Null ise ilk kaydı getirir</param>
        /// <returns>Bulunan entity ya da "Entity Not found!" hata mesajı</returns>
        public async Task<IResult<T>> FindFirstAsync(Expression<Func<T, bool>>? expression = null)
        {
            var entity = expression == null ? await _table.FirstOrDefaultAsync() : await _table.FirstOrDefaultAsync(expression);

            if (entity == null)
            {
                return Result<T>.Failure(["Entity Not found!"], 404);
            }

            return Result<T>.Success(entity);
        }

        /// <summary>
        /// Belirtilen koşula uygun tüm kayıtları veritabanından getirir ve istenen ilişkili verileri yükler.
        /// </summary>
        /// <param name="expression">Arama koşulu (örn: x => x.IsActive == true). Null ise tüm kayıtları getirir</param>
        /// <param name="includes">Yüklenecek ilişkili veri property'lerinin isimleri (örn: "Category", "Comments")</param>
        /// <returns>Bulunan entity'ler listesi ya da "Entities Not found!" hata mesajı</returns>
        public async Task<IResult<IEnumerable<T>>> FindManyAsync(Expression<Func<T, bool>>? expression = null, params string[] includes)
        {
            var entities = expression == null ? _table : _table.Where(expression);

            if (entities == null)
            {
                return Result<IEnumerable<T>>.Failure(["Entities Not found!"], 404);
            }

            foreach (var include in includes)
            {
                entities = entities.Include(include);
            }

            return Result<IEnumerable<T>>.Success(await entities.ToListAsync());
        }

        /// <summary>
        /// Veritabanında bulunan bir entity'nin verilerini günceller. DbContext.SaveChanges() çağrılmadığı sürece veritabanına yazılmaz.
        /// </summary>
        /// <param name="entity">Güncellenecek entity nesnesi (güncellenmiş verilerle doldurulmuş olmalı)</param>
        /// <returns>200 OK status kodu ya da hata</returns>
        public async Task<IResult> UpdateAsync(T entity)
        {
            try
            {
                await Task.Run(() => _table.Update(entity));
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message, 500);
            }
        }

        /// <summary>
        /// Veritabanında bulunan birden fazla entity'nin verilerini günceller. DbContext.SaveChanges() çağrılmadığı sürece veritabanına yazılmaz.
        /// </summary>
        /// <param name="entities">Güncellenecek entity nesneleri listesi</param>
        /// <returns>200 OK status kodu ya da hata</returns>
        public async Task<IResult> UpdateManyAsync(IEnumerable<T> entities)
        {
            try
            {
                await Task.Run(() => _table.UpdateRange(entities));
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message, 500);
            }
        }
    }
}