namespace Utils.Responses
{
    /// <summary>
    /// API işlemlerinin sonucunu temsil eden temel arayüz.
    /// Her API yanıtı başarılı mı başarısız mı, hata var mı, durum kodu nedir gibi bilgileri içerir.
    /// </summary>
    public interface IResult
    {
        /// <summary>
        /// İşlemin başarılı olup olmadığını gösterir. true: başarılı, false: başarısız
        /// </summary>
        bool IsSuccess { get; }

        /// <summary>
        /// İşlem hakkında bilgilendirme mesajı (örn: "Kayıt başarıyla eklendi")
        /// </summary>
        string Message { get; }

        /// <summary>
        /// İşlem başarısız olduysa hata mesajları listesi
        /// </summary>
        IEnumerable<string> Errors { get; }

        /// <summary>
        /// HTTP durum kodu (200: OK, 201: Created, 400: Bad Request, 404: Not Found, 500: Internal Server Error vb.)
        /// </summary>
        int StatusCode { get; }
    }

    /// <summary>
    /// IResult arayüzünün generic versiyonu.
    /// İşlemin sonucunda döndürülecek veri (T tipi) var ise bu arayüz kullanılır.
    /// Örn: FindByIdAsync işlemi sonucu User nesnesi döndüreceği için IResult&lt;User&gt; kullanılır.
    /// </summary>
    /// <typeparam name="T">İşlem başarılı olduysa döndürülecek veri tipi (Entity, string, int vb.)</typeparam>
    public interface IResult<out T> : IResult
    {
        /// <summary>
        /// İşlem başarılı olduysa döndürülecek veri nesnesi
        /// </summary>
        T Data { get; }
    }

    /// <summary>
    /// IResult arayüzünün temel uygulaması.
    /// İşlem sonucunu (başarılı/başarısız, hata, durum kodu vb.) taşır.
    /// Bu sınıf veri döndürmez, sadece işlemin durumunu bildirir (örn: Create, Update, Delete işlemleri).
    /// </summary>
    public class Result : IResult
    {
        /// <summary>
        /// İşlemin başarılı olup olmadığını gösterir.
        /// init: özellik sadece nesne oluşturulurken atanabilir, daha sonra değiştirilemez.
        /// </summary>
        public bool IsSuccess { get; init; }

        /// <summary>
        /// İşlem hakkında açıklayıcı mesaj. Varsayılan değeri boş string'dir.
        /// </summary>
        public string Message { get; init; } = string.Empty;

        /// <summary>
        /// Hatalar listesi. Başarısız işlemler için hata mesajları burada yer alır.
        /// Varsayılan değeri boş liste'dir.
        /// </summary>
        public IEnumerable<string> Errors { get; init; } = [];

        /// <summary>
        /// HTTP durum kodu. Örn: 200 (OK), 201 (Created), 400 (Bad Request), 404 (Not Found), 500 (Internal Server Error)
        /// </summary>
        public int StatusCode { get; init; }

        /// <summary>
        /// Result nesnesinin yapıcı metodu. Protected olduğu için sadece bu sınıf ve kalıtım yapan sınıflar tarafından kullanılabilir.
        /// </summary>
        /// <param name="isSuccess">İşlemin başarılı olup olmadığı</param>
        /// <param name="statusCode">HTTP durum kodu</param>
        /// <param name="message">İşlem mesajı (isteğe bağlı)</param>
        /// <param name="errors">Hata mesajları listesi (isteğe bağlı)</param>
        protected Result(bool isSuccess, int statusCode, string? message = null, IEnumerable<string>? errors = null)
        {
            IsSuccess = isSuccess;
            Message = message ?? string.Empty;
            Errors = errors ?? [];
            StatusCode = statusCode;
        }

        /// <summary>
        /// Başarılı işlem sonucu oluşturur.
        /// Kullanım: Result.Success(201, "Kayıt eklendi")
        /// </summary>
        /// <param name="statusCode">HTTP durum kodu. Varsayılan: 200 (OK)</param>
        /// <param name="message">Başarı mesajı (isteğe bağlı)</param>
        /// <returns>Başarılı Result nesnesi</returns>
        public static Result Success(int statusCode = 200, string? message = null) => new(true, statusCode, message);

        /// <summary>
        /// Başarısız işlem sonucu oluşturur (hata listesi ile).
        /// Kullanım: Result.Failure(new[] { "Email alanı boş", "Şifre çok kısa" }, 400)
        /// </summary>
        /// <param name="errors">Hata mesajları listesi</param>
        /// <param name="statusCode">HTTP durum kodu. Varsayılan: 400 (Bad Request)</param>
        /// <returns>Başarısız Result nesnesi</returns>
        public static Result Failure(IEnumerable<string> errors, int statusCode = 400) => new(false, statusCode, null, errors);

        /// <summary>
        /// Başarısız işlem sonucu oluşturur (tek bir hata mesajı ile).
        /// Kullanım: Result.Failure("Veritabanı bağlantısı başarısız", 500)
        /// </summary>
        /// <param name="message">Hata mesajı</param>
        /// <param name="statusCode">HTTP durum kodu. Varsayılan: 400 (Bad Request)</param>
        /// <returns>Başarısız Result nesnesi</returns>
        public static Result Failure(string message, int statusCode = 400) => new(false, statusCode, message);
    }

    /// <summary>
    /// IResult&lt;T&gt; arayüzünün uygulaması.
    /// İşlem başarılı olduysa belirli bir veri tipi (T) döndüren işlemler için kullanılır.
    /// Örn: FindByIdAsync&lt;User&gt;() sonucunda User nesnesi döndüreceği için Result&lt;User&gt; kullanılır.
    /// </summary>
    /// <typeparam name="T">İşlem başarılı olduysa döndürülecek veri tipi</typeparam>
    public class Result<T> : Result, IResult<T>
    {
        /// <summary>
        /// İşlem başarılı olduysa döndürülecek veri nesnesi.
        /// Başarısız işlemlerde null değeri içerir.
        /// </summary>
        public T Data { get; init; }

        /// <summary>
        /// Result&lt;T&gt; nesnesinin yapıcı metodu. Private olduğu için sadece static factory metotları tarafından kullanılabilir.
        /// </summary>
        /// <param name="data">İşlem başarılı olduysa döndürülecek veri</param>
        /// <param name="isSuccess">İşlemin başarılı olup olmadığı</param>
        /// <param name="statusCode">HTTP durum kodu</param>
        /// <param name="message">İşlem mesajı (isteğe bağlı)</param>
        /// <param name="errors">Hata mesajları listesi (isteğe bağlı)</param>
        private Result(T data, bool isSuccess, int statusCode, string? message = null, IEnumerable<string>? errors = null) : base(isSuccess, statusCode, message, errors)
        {
            Data = data;
        }

        /// <summary>
        /// Başarılı işlem sonucu oluşturur ve belirli bir veri döndürür.
        /// Kullanım: Result&lt;User&gt;.Success(user, 200, "Kullanıcı bulundu")
        /// </summary>
        /// <param name="data">İşlem başarılı olduysa döndürülecek veri nesnesi</param>
        /// <param name="statusCode">HTTP durum kodu. Varsayılan: 200 (OK)</param>
        /// <param name="message">Başarı mesajı (isteğe bağlı)</param>
        /// <returns>Başarılı Result&lt;T&gt; nesnesi</returns>
        public static Result<T> Success(T data, int statusCode = 200, string? message = null) => new(data, true, statusCode, message);

        /// <summary>
        /// Başarısız işlem sonucu oluşturur (hata listesi ile, veri döndürmez).
        /// Kullanım: Result&lt;User&gt;.Failure(new[] { "Kullanıcı bulunamadı" }, 404)
        /// </summary>
        /// <param name="errors">Hata mesajları listesi</param>
        /// <param name="statusCode">HTTP durum kodu. Varsayılan: 400 (Bad Request)</param>
        /// <param name="message">Hata açıklaması (isteğe bağlı)</param>
        /// <returns>Başarısız Result&lt;T&gt; nesnesi (Data özelliği null)</returns>
        public static Result<T> Failure(IEnumerable<string> errors, int statusCode = 400, string? message = null) => new(default, false, statusCode, message, errors);
    }
}