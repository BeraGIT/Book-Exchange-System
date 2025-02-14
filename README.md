# Kitap Takas Sistemi / Book Exchange System

## Açıklama / Description
**Türkçe:** Bu proje, üniversite içinde kitap takasının yapılmasını sağlayan bir sistemdir. Kullanıcılar, diğer kullanıcıların sisteme yüklediği kitaplara kendi yüklediği kitapları takas teklifi olarak sunabilirler. Sistem yalnızca bir Restful API olarak geliştirilmiştir ve herhangi bir kullanıcı arayüzü (UI) bulunmamaktadır. Döndürülen yanıtlar ve yorum satırları Türkçedir. API'lar henüz beta aşamasındadır ve geliştirilmeye devam edilmektedir.

**English:** This project is a system that allows book exchanges within a university. Users can offer their books as exchange proposals for books listed by other users. The system is developed solely as a Restful API and does not include any user interface (UI). The APIs are still in the beta phase and are being developed. The API responses and comments are in Turkish.

## 📌 Proje Mimarisi / Project Architecture
**Türkçe:** Proje, .NET 8 üzerinde N-Tier (çok katmanlı) mimari kullanılarak geliştirilmiştir.

**English:** The project is developed using an N-Tier architecture on .NET 8.

### 1️⃣ Entities Katmanı / Entities Layer
**Türkçe:**
- Veri modelleri, veritabanı şemasıyla uyumlu olarak tasarlanmıştır.
- Veri yapıları optimize edilmiştir.
- Interface yapıları ile esneklik sağlanmıştır.

**English:**
- Data models are designed to be compatible with the database schema.
- Data structures are optimized.
- Flexibility is ensured through interface structures.

### 2️⃣ DAL (Data Access Layer) Katmanı / DAL (Data Access Layer)
**Türkçe:**
- Veritabanı işlemleri ve CRUD (Create, Read, Update, Delete) operasyonları kodlanmıştır.
- Veritabanı erişim katmanı oluşturularak sorgular optimize edilmiştir.
- Uygulamanın veritabanı bağlantısı yalnızca bu katmandan gerçekleştirilir.

**English:**
- Database operations and CRUD (Create, Read, Update, Delete) operations are coded.
- Database access layer is created and queries are optimized.
- The application's database connection is only made from this layer.

### 3️⃣ BLL (Business Logic Layer) Katmanı / BLL (Business Logic Layer)
**Türkçe:**
- İş mantığına uygun yeni metotlar geliştirilmiştir.
- Interface yapısı genişletilmiştir.
- DAL ile API arasındaki iletişimi yönetir.
- API katmanındaki iş yükü BLL katmanına taşınacaktır.

**English:**
- New methods are developed according to business logic.
- Interface structure is extended.
- Manages communication between DAL and API.
- The API layer's workload is transferred to the BLL layer.

### 4️⃣ API Katmanı / API Layer
**Türkçe:**
- Sistemin dışa açılan katmanıdır.
- BLL’den gelen işlevleri dışa açarak Restful API hizmeti sunar.
- Güvenlik önlemleri ve giriş parametre kontrolleri uygulanmıştır.

**English:**
- The outward-facing layer of the system.
- Provides Restful API services by exposing functions from the BLL.
- Security measures and input validation checks are implemented.

## 🔄 Sistemin Kullanım Senaryosu / Usage Scenarios
**Türkçe:** DAL Katmanı MyContext'te 1-5 arası ID'lerle rastgele fakülteler eklenmiştir.

**English:** Random faculty entries with IDs ranging from 1 to 5 have been preloaded into MyContext in the DAL Layer.

1. **Kullanıcı Kaydı & Giriş / User Registration & Login:** Kullanıcı, sisteme kayıt olup giriş yapar.

   The user registers and logs into the system.

2. **Profil Yönetimi / Profile Management:** Kullanıcı, profilini oluşturur ve düzenler.

   The user creates and edits their profile.

3. **İlanları Görüntüleme / Viewing Listings:** Kullanıcı, sistemdeki mevcut ilanları görüntüler.

   The user views existing listings in the system.

4. **Teklif Gönderme / Sending Offers:** Kullanıcı, elindeki kitaplardan birini seçerek bir ilana teklif sunar.

   The user selects one of their books and makes an offer on a listing.

5. **Takas İşlemi / Exchange Process:**
   - İlan sahibi teklifi kabul ederse, takas ilan sahibinin belirlediği fiziksel fakülte lokasyonunda gerçekleşir.
   - If the listing owner accepts the offer, the exchange occurs at the specified faculty location.
   - İlan sahibi teklifi reddederse, işlem iptal edilir.
   - If the listing owner rejects the offer, the process is canceled.

6. **Yeni İlan Oluşturma / Creating a New Listing:** Kullanıcı, takas yapmak istediği kitabı seçerek yeni bir ilan oluşturabilir.
   - The user selects the book they want to exchange and creates a new listing.
   - İhtiyaç duyduğu kitap türünü not olarak ekler.
   - Adds a note specifying the type of book they need.
   - Takas lokasyonunu belirtir.
   - Specifies the exchange location.

7. **Güvenlik Önlemleri / Security Measures:**
   - İlan ve teklif durumundaki kitaplar geçici olarak kilitlenir.
   - Books in listing and offer status are temporarily locked.

## 🚀 Kurulum & Çalıştırma / Setup & Running

**1. API Katmanında appsettings.json'daki "MyConnection" bağlantı ayarlarını kendi veri tabanı ayarlarınıza göre güncelleyin / Update the "MyConnection" connection string in appsettings.json within the API layer to match your database settings.**


**2. Proje dizinine gidin / Navigate to the project directory:**
    ```shell
    cd proje-adi / cd project-name
    ```

**3. Bağımlılıkları yükleyin / Install dependencies:**
    ```shell
    dotnet restore
    ```

**4. İlk Migration’ı oluşturun (hata bulunmazsa) / Create the initial migration (if no errors):**
    ```shell
    dotnet ef migrations add InitialMigration --project .\DAL\DAL.csproj --startup-project .\API\API.csproj
    ```

**5. Veritabanını güncelleyin / Update the database:**
    ```shell
    dotnet ef database update --project .\DAL\DAL.csproj --startup-project .\API\API.csproj
    ```

**6. Projeyi çalıştırın / Run the project:**
    ```shell
    dotnet run --project .\API\API.csproj
    ```

**7. Swagger üzerinden API’yi test edin / Test the API via Swagger.**


## Lisans / License
Bu proje [MIT Lisansı](LICENSE.md) altında lisanslanmıştır. / This project is licensed under the [MIT License](LICENSE.md).
