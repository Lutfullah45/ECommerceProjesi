# 🛍️ Core Shop - E-Ticaret Platformu

![.NET Core](https://img.shields.io/badge/.NET%20Core-6.0%2F7.0-purple) ![ASP.NET MVC](https://img.shields.io/badge/ASP.NET-MVC-blue) ![EF Core](https://img.shields.io/badge/Entity%20Framework-Core-green) ![Bootstrap](https://img.shields.io/badge/Bootstrap-5-orange)

**Core Shop**, ASP.NET Core MVC teknolojisi kullanılarak geliştirilmiş, **N-Katmanlı Mimari (N-Layer Architecture)** prensiplerine uygun, modern ve ölçeklenebilir bir e-ticaret uygulamasıdır.

Kullanıcıların kategori bazlı vitrinlerde gezinebildiği, detaylı filtreleme yapabildiği, sepete ürün ekleyip sipariş verebildiği uçtan uca bir alışveriş deneyimi sunar. Proje, **Clean Code** ve **SOLID** prensipleri gözetilerek tasarlanmıştır.

---

## 📸 Proje Görselleri

### 1. Ana Sayfa ve Karşılama Ekranı
Kullanıcıyı karşılayan Hero alanı ve popüler ürünlerin listelendiği ana giriş sayfası.
![Ana Sayfa](home_screen.png)

### 2. Kategori Vitrini (Showcase Modu)
Kullanıcı "Tümünü Gör" dediğinde açılan, ürünlerin kategorilerine göre şeritler halinde ayrıldığı (Netflix tarzı) özel vitrin görünümü.
![Kategori Vitrini](category_filter.png)

### 3. Ürün Detay ve Sepet İşlemleri
Ürün açıklamalarının, stok bilgisinin ve fiyat seçeneklerinin yer aldığı detay sayfası.
![Ürün Detay](product_detail.png)

---

## 🌟 Öne Çıkan Özellikler

### 👤 Kullanıcı Arayüzü (UI)
* **Akıllı Vitrin Sistemi:** Kategori bazlı ayrıştırılmış, kullanıcı dostu ürün sunumu.
* **Dinamik Listeleme:** Arama ve kategori filtrelerine göre otomatik değişen liste görünümü.
* **Sepet Yönetimi:** Session bazlı, dinamik sepet işlemleri (Ekleme/Çıkarma).
* **Üyelik Sistemi:** ASP.NET Core Identity ile güvenli kayıt ve giriş işlemleri.
* **Stok Kontrolü:** Stokta olmayan ürünler için "Tükendi" uyarısı ve satın alma engeli.

### 🛠️ Yönetim Paneli (Admin Dashboard)
* **Ürün & Kategori Yönetimi:** Veritabanına dinamik ürün ekleme, düzenleme ve silme.
* **Resim Yönetimi:** Ürünlere ait görsellerin yönetimi.
* **Marka Tanımlamaları:** Marka bazlı filtreleme için altyapı.

---

## 🏗️ Mimari ve Teknolojiler

Proje, sürdürülebilirlik açısından **N-Katmanlı Mimari** üzerine kurulmuştur:

1.  **Core (Entities):** Veritabanı tablolarına karşılık gelen saf sınıflar (POCO).
2.  **DataAccess (DAL):** Veritabanı erişimi, Entity Framework Core konfigürasyonları ve Repository Pattern.
3.  **Business (BL):** İş kuralları, validasyonlar ve servis katmanı.
4.  **WebUI (MVC):** Kullanıcı etkileşimi, Controller, View, ViewModel yapıları.

**Teknoloji Yığını:**
* **Backend:** C#, ASP.NET Core MVC
* **Veritabanı:** MSSQL Server, Entity Framework Core (Code First)
* **Frontend:** HTML5, CSS3, Bootstrap 5
* **Araçlar:** Visual Studio, Git

---

## 🚀 Kurulum ve Çalıştırma

Projeyi yerel makinenizde çalıştırmak için:

1.  **Projeyi Klonlayın:**
    ```bash
    git clone [https://github.com/Lutfullah45/ECommerceProjesi.git](https://github.com/Lutfullah45/ECommerceProjesi.git)
    ```

2.  **Veritabanı Ayarları:**
    `WebUI` katmanındaki `appsettings.json` dosyasını açın ve **ConnectionStrings** alanını kendi SQL Server bilgilerinize göre güncelleyin.

3.  **Migration İşlemi:**
    Visual Studio'da **Package Manager Console**'u açın ve aşağıdaki komutu çalıştırın (Default Project: DataAccess seçili olmalıdır):
    ```powershell
    Update-Database
    ```

4.  **Başlatma:**
    `WebUI` projesini "Set as Startup Project" yapın ve çalıştırın.

---

## 📝 Lisans

Bu proje eğitim ve portföy amaçlı geliştirilmiştir. [MIT](LICENSE) lisansı ile lisanslanmıştır.
