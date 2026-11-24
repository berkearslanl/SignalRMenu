# 🍔 SignalR Menu — QR Kodlu Restoran Sipariş ve Yönetim Platformu

**SignalR Menu**, **ASP.NET Core (.NET 10)** üzerinde **çok katmanlı mimari** ile geliştirilmiş, **SignalR** ile gerçek zamanlı çalışan bir restoran dijital menü ve sipariş yönetim sistemidir. Müşteriler masalarına oturup menüden sepetlerini oluşturur ve siparişlerini doğrudan verir; yönetici ise siparişleri, masa durumlarını, bildirimleri ve kasayı **anlık olarak** tek bir panelden takip eder.

---

## 🌟 Öne Çıkan Özellikler

| Özellik | Açıklama |
|---|---|
| 🏛️ **Katmanlı Mimari** | Entity, DataAccess, Business, Dto, API ve WebUI katmanları arasında net bağımlılık yönü ve sorumluluk ayrımı |
| ⚡ **Gerçek Zamanlı Panel (SignalR)** | İstatistikler, siparişler, masa durumları, bildirimler ve kasa tutarı sayfa yenilenmeden anlık güncellenir |
| 🪑 **Masa Oturum Yönetimi** | Müşteri masaya oturunca masa `Dolu`, kalkınca `Boş` olur; dolu masaya ikinci kişi oturamaz |
| 🍪 **Çerez Tabanlı Masa Kimliği** | Masa bilgisi oturum çerezinde (HttpOnly) tutulur; müşteri başka masanın sepetine erişemez |
| 🛒 **Canlı Sepet Rozeti** | SignalR grupları (`Table-{id}`) ile sepetteki ürün sayısı yalnızca ilgili masanın ekranında anlık güncellenir |
| 🗄️ **SQL Trigger Zinciri** | Sipariş toplamı (KDV dahil), kasa tutarı ve masa durumu veritabanı tetikleyicileriyle tutarlı şekilde güncellenir |
| ✅ **FluentValidation** | Tüm varlıklar için Ekleme/Güncelleme doğrulayıcıları; hatalar Türkçe ve form alanlarının altında gösterilir |
| 🔐 **Cookie Authentication** | Yönetim paneli global `AuthorizeFilter` ile korunur; müşteri sayfaları `AllowAnonymous` ile ayrıştırılır |
| 📱 **QR Kod Üretimi** | Masa için QR kod oluşturma ekranı (QRCoder) |
| 🍳 **Tarif Entegrasyonu** | RapidAPI (Tasty) üzerinden harici tarif verisi çekilen modern kart arayüzü |
| 📬 **E-posta Gönderimi** | MailKit ile SMTP üzerinden e-posta gönderimi |

---

## 🛠️ Kullanılan Teknolojiler

| Alan | Teknoloji |
|---|---|
| Framework | ASP.NET Core (.NET 10), MVC, Web API |
| Mimari / Desenler | Çok katmanlı mimari, Repository Pattern, DTO, Dependency Injection |
| Veri erişimi | Entity Framework Core 10, SQL Server, Migrations |
| Veritabanı | SQL Server **Trigger**'ları (sipariş toplamı, kasa, masa durumu) |
| Gerçek zamanlı | SignalR (Hub, istemci grupları, `IHubContext`) |
| Doğrulama | FluentValidation 12 |
| Eşleme | AutoMapper |
| Kimlik doğrulama | ASP.NET Core Identity, Cookie Authentication, global `AuthorizeFilter` |
| Oturum / Durum | Çerez tabanlı masa kimliği (HttpOnly, oturum çerezi) |
| HTTP istemcisi | `IHttpClientFactory`, Newtonsoft.Json |
| API dokümantasyonu | OpenAPI + Scalar |
| Harici servisler | RapidAPI (Tasty tarifleri), MailKit (SMTP) |
| QR kod | QRCoder |
| Arayüz | Razor, ViewComponent, Bootstrap, jQuery, AJAX |

---

## 🖥️ Fonksiyonel Kapsam

### Müşteri tarafı
- Masa seçim ekranı: boş/dolu masa kartları, durum filtreleri ve canlı sayaçlar
- Kategori ve ürün listeleme, ürünü karttan sepete ekleme (aynı ürün eklenirse adet artar)
- Sepet sayfası: ürün adedi azaltma/silme, ara toplam, **%20 KDV** ve genel toplam
- "Ödeme Adımına Geç" ile siparişi tamamlama
- Sağ altta sabit **"Masadan Kalk"** balonu: masa bilgisi, menüye dönüş ve masadan kalkma
- Rezervasyon formu, iletişim formu, referanslar, hakkımızda ve tarifler sayfaları

### Yönetim paneli
- **Dashboard ve istatistik:** kategori/ürün sayıları, ortalama fiyat, en pahalı/ucuz ürün, günlük sipariş tutarı, en çok sipariş veren masa, aktif masa ve rezervasyon sayısı, kasa tutarı (tümü SignalR ile canlı)
- Kategori, ürün, indirim, özellik, referans, hakkımızda, iletişim ve rezervasyon yönetimi (CRUD)
- Sipariş ve masa yönetimi: kartlar üzerinden masa durumunu tek tıkla değiştirme
- Anlık **bildirim** sistemi ("Masa X için yeni sipariş alındı")
- Yönetici–ziyaretçi **canlı mesajlaşma** ve mesaj kutusu
- Kasa (MoneyCase) görüntüleme
- Kayıt/giriş ekranları ve profil ayarları

---

## 🎬 Senaryo: Müşterinin Masadan Siparişe Yolculuğu

Aşağıdaki akış, sistemin baştan sona nasıl çalıştığını göstermektedir.

### 1️⃣ Masaya oturma
1. Müşteri QR kod ile **masa seçim sayfasına** ulaşır.
2. Sayfada tüm masalar `Boş` / `Dolu` durumuyla listelenir. `Dolu` masalarda buton pasiftir.
3. **"Masaya Otur"** seçildiğinde WebUI, API'deki `SitTable/{id}` işlemini çağırır:
   - Masa zaten doluysa `409 Conflict` döner, müşteri tekrar masa listesine yönlendirilir.
   - Masa müsaitse durumu `Dolu` olur.
4. WebUI, masa numarasını **`TableId` çerezine** yazar (oturum çerezi, `HttpOnly`) ve müşteriyi menüye yönlendirir.

### 2️⃣ Menüden sepete ürün ekleme
1. Müşteri menüde bir ürünün **sepete ekle** butonuna basar. Masa numarası formdan değil, **çerezden** okunur.
2. API, o masanın sepetini kontrol eder:
   - Ürün sepette varsa **adet 1 artar**,
   - yoksa yeni sepet satırı oluşturulur.
3. İşlem başarısız olursa (ör. çerez yok) kullanıcıya anlamlı bir hata mesajı gösterilir.

### 3️⃣ Canlı sepet rozeti
1. Sayfa açıldığında tarayıcı, hub'a `type=customer&tableId={id}` parametreleriyle bağlanır.
2. Hub bağlantıyı `Table-{id}` **SignalR grubuna** ekler.
3. Sepet her değiştiğinde API, yalnızca bu gruba `ReceiveBasketCount` mesajı gönderir; üst menüdeki **sepet rozeti** sayfa yenilenmeden güncellenir (0 olduğunda gizlenir, 99'u aşınca `99+` gösterilir).

### 4️⃣ Siparişi tamamlama
Müşteri sepet sayfasında ara toplamı, **%20 KDV**'yi ve genel toplamı görür ve **"Siparişi Tamamla"** butonuna basar. Tarayıcı **yalnızca bir istek** gönderir; fiyat veya ürün bilgisi istemciden alınmaz:

1. WebUI, masa numarasını çerezden okuyup API'deki `CompleteOrder/{tableId}` işlemini çağırır.
2. API sırasıyla şunları doğrular: masa var mı → masa `Dolu` mu → sepet boş değil mi.
3. Sepetteki her satırdan, **ürünün veritabanındaki güncel fiyatıyla** bir `OrderDetail` üretilir.
4. `Order` ve `OrderDetail` kayıtları **tek `SaveChanges`** ile birlikte kaydedilir (ya ikisi de kaydolur ya da hiçbiri).
5. Sipariş kaydından sonra sepet temizlenir, yöneticiye bildirim yazılır ve masanın sepet rozeti anında **0**'a düşer.
6. Müşteri "Siparişiniz alındı" mesajını görür. Masa, müşteri kalkana kadar `Dolu` kalır; müşteri aynı oturumda tekrar sipariş verebilir.

### 5️⃣ Veritabanı tetikleyicilerinin devreye girmesi
Kayıt anında SQL Server tetikleyicileri çalışır:

| Trigger | Tablo / Olay | Görevi |
|---|---|---|
| `SetTableFull` | `Orders` / INSERT | Siparişin ait olduğu masayı `Dolu` yapar |
| `IncreaseOrderTotalPrice` | `OrderDetails` / INSERT | Detay tutarını (KDV dahil) siparişin toplamına ve **kasaya** ekler |
| `DecreaseOrderTotalPrice` | `OrderDetails` / DELETE | Silinen detay tutarını siparişten ve kasadan düşer |

Örnek: ara toplamı 440 ₺ olan bir sepet için `Orders.TotalPrice` **528 ₺** (440 × 1,20) olarak oluşur ve kasa aynı tutar kadar artar.

### 6️⃣ Yönetici panelinde anlık görünüm
Müşteri siparişi verdiği anda, yönetici sayfasını yenilemeden:
- **Bildirim** ikonuna "Masa X için yeni sipariş alındı. Tutar: 528,00 ₺" düşer (SignalR ile tüm yönetici ekranlarına iletilir),
- Sipariş kaydı, kasa tutarı ve masa durumu veritabanında güncellenmiş olur; **sipariş listesi, masa kartları ve dashboard** değerleri hub üzerinden bu yeni veriyi yansıtır.

### 7️⃣ Masadan kalkma
1. Müşteri, sayfanın sağ altındaki **sabit balondan "Masadan Kalk"** seçeneğini kullanır (onay istenir).
2. API'deki `LeaveTable/{id}` isteği çağrılır: o masanın **tüm sepeti tek `DELETE` ile silinir** ve masa `Boş` olur.
3. `TableId` çerezi silinir, sepet rozeti sıfırlanır ve müşteri masa seçim sayfasına döner. Masa bir sonraki müşteri için hazırdır.

---

## 📸 Ekran Görüntüleri

### 🎬 Çalışma Akışı

https://github.com/user-attachments/assets/24eb3caa-03a2-4e65-b462-369b7d4f5437

### 🛒 Müşteri Tarafı

| Masa Seçimi | Menü |
|:---:|:---:|
| ![Masa Seçimi](https://github.com/user-attachments/assets/5550695c-a831-4b08-8303-ffed3807a2aa) | ![Menü](https://github.com/user-attachments/assets/b8e2d5d6-dfac-48bd-adb2-149f43140859) |

| Sepet | Tarifler |
|:---:|:---:|
| ![Sepet](https://github.com/user-attachments/assets/9df4f092-2a98-4d25-82df-460ea560d5c8) | ![Tarifler](https://github.com/user-attachments/assets/854a36a7-45e1-4f71-964d-feaa493c3ff5) |

### 🧑‍💼 Yönetim Paneli

| Giriş | Dashboard |
|:---:|:---:|
| ![Giriş](https://github.com/user-attachments/assets/237b2a99-6d73-446a-88f6-875f87328d27) | ![Dashboard](https://github.com/user-attachments/assets/bb807a86-b2ca-4368-a66b-d24b8ae1467f) |

| İstatistikler | Masalar |
|:---:|:---:|
| ![İstatistikler](https://github.com/user-attachments/assets/d0fb7604-9a29-4ff9-bf97-b70b4eae4f46) | ![Masalar](https://github.com/user-attachments/assets/612137c1-f2d2-450c-ac3a-7ff8c9490d6e) |

| Siparişler | Bildirimler |
|:---:|:---:|
| ![Siparişler](https://github.com/user-attachments/assets/c90c8028-2f1a-44ae-bf18-eceaef17d5fc) | ![Bildirimler](https://github.com/user-attachments/assets/4bb49e1a-a40c-4ea4-8a81-f4cd881f6118) |

<details>
<summary>🏠 Ana sayfa (tam sayfa) ve hata sayfası</summary>

<br>

![Ana Sayfa](https://github.com/user-attachments/assets/0dc3c98f-fb87-455d-bbde-d15f367bcd06)

![Hata Sayfası](https://github.com/user-attachments/assets/8d55205e-c245-432f-9091-3f1c888ac5aa)

</details>

---

## 🙏 Teşekkür

Bu projenin temelini oluşturan eğitim için **Murat Yücedağ**'a teşekkür ederim.
