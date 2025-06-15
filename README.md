# CarService — Next Generation Auto Repair Management

**CarService**, araç bakım ve tamirat sektöründeki küçük ve orta ölçekli atölyeler için geliştirilen modern, esnek ve ölçeklenebilir bir yönetim sistemidir.

Atölyenizi dijitalleştirin.  
İş süreçlerinizi merkezi ve kontrollü şekilde yönetin.  
Geleceğin SaaS platformları için güçlü bir temel sunar.

---

## Proje Durumu

- Proje aktif geliştirme aşamasındadır.
- Domain ve Application katmanları büyük ölçüde tamamlanmıştır.
- Auth ve JWT tabanlı temel kimlik doğrulama sistemine sahiptir.
- API katmanı henüz geliştirme aşamasındadır.

---

## Temel Özellikler

- **Müşteri Yönetimi:** Müşteri ve araç bilgileri takibi
- **Bakım ve Tamir Kayıtları:** Yapılan işlemler, değiştirilen parçalar, tedarikçiler ve servis geçmişi
- **Cari Hesap Takibi:** Ödemeler ve bakiye yönetimi
- **Yetkilendirme:** JWT tabanlı güvenli kimlik doğrulama altyapısı
- **Genişletilebilir Modüler Yapı:** Gelecek SaaS ihtiyaçları düşünülerek tasarlandı

---

## Teknoloji Altyapısı

- ✅ **Dil & Platform:** C# (.NET 9)
- ✅ **Mimari:** Modüler Monolit
- ✅ **CQRS:** Command & Query Responsibility Segregation Pattern
- ✅ **Validation:** FluentValidation
- ✅ **Paging:** Performanslı sayfalama
- ✅ **Response Handling:** Standardize Response Wrapper
- ✅ **Mapping:** Manuel Mapping Extension metotları
- ✅ **Kimlik Doğrulama:** JWT (JSON Web Token)
- ✅ **Temiz Kod:** SOLID ve Clean Architecture prensiplerine uygun katmanlı yapı

---

## SaaS Genişleme Potansiyeli

CarService, ilerleyen sürümlerde SaaS mimarisine dönüştürülebilecek güçlü bir çekirdek sunar:

- Çoklu tenant (multi-tenant) yapısı
- Abonelik ve ödeme sistemi entegrasyonu
- Web UI / Admin paneli
- Raporlama ve analiz modülleri

---

## Kurulum ve Katılım

Projeyi geliştirmek ve katkıda bulunmak isteyen geliştiriciler için:

```bash
git clone https://github.com/adnanertorer/CarService.git
