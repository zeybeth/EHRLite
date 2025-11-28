using Microsoft.AspNetCore.Authorization;
using EHRLite.Models;
using EHRLite.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EHRLite.Controllers
{
    [Authorize]
    public class PatientController : Controller
    {
        // Repository'i buraya çağırıyoruz (Dependency Injection)
        private readonly IPatientRepository _patientRepo;

        public PatientController(IPatientRepository patientRepo)
        {
            _patientRepo = patientRepo;
        }

        // Hastaları Listeleme Sayfası
        public IActionResult Index()
        {
            // Veritabanından tüm hastaları çek
            List<Patient> objPatientList = _patientRepo.GetAll().ToList();

            // Listeyi View'a gönder
            return View(objPatientList);
        }

        // GET: Ekleme Sayfasını Açar
        public IActionResult Create()
        {
            return View();
        }

        // POST: Formdan gelen veriyi Veritabanına Kaydeder
        [HttpPost]
        [ValidateAntiForgeryToken] // Güvenlik önlemi (CSRF saldırılarına karşı)
        public IActionResult Create(Patient obj)
        {
            // Girilen veriler kurallara (örn: Zorunlu alanlar dolu mu?) uygun mu?
            if (ModelState.IsValid)
            {
                _patientRepo.Add(obj); // Repository'e ekle
                _patientRepo.Save();   // Değişiklikleri veritabanına işle
                TempData["success"] = "Yeni hasta başarıyla oluşturuldu."; // Geçici başarı mesajı
                return RedirectToAction("Index"); // Listeye geri dön
            }
            return View(obj); // Hata varsa formu tekrar göster
        }
        // GET: Düzenleme Sayfasını Açar (Verilen ID'ye göre hastayı bulur getirir)
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound(); // ID yoksa hata dön
            }

            // Veritabanından o ID'ye sahip hastayı bul
            Patient? patientFromDb = _patientRepo.Get(u => u.Id == id);

            if (patientFromDb == null)
            {
                return NotFound(); // Hasta bulunamazsa hata dön
            }

            return View(patientFromDb); // Hastayı bulduk, bilgilerini View'a gönder
        }

        // POST: Düzenlenmiş veriyi kaydeder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Patient obj)
        {
            if (ModelState.IsValid)
            {
                _patientRepo.Update(obj); // Güncelleme işlemi
                _patientRepo.Save();      // Kaydet
                TempData["success"] = "Hasta bilgileri güncellendi.";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        // GET: Silme Sayfasını Açar (Kullanıcıya "Emin misin?" diye sormak için)
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            // Silinecek hastayı bulup ekrana getiriyoruz
            Patient? patientFromDb = _patientRepo.Get(u => u.Id == id);

            if (patientFromDb == null)
            {
                return NotFound();
            }

            return View(patientFromDb);
        }

        // POST: Silme İşlemini Gerçekleştirir
        [HttpPost, ActionName("Delete")] // C# metod adı DeletePOST olsa da URL'de "Delete" olarak çalışsın
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            // Silinecek hastayı tekrar bul
            Patient? obj = _patientRepo.Get(u => u.Id == id);

            if (obj == null)
            {
                return NotFound();
            }

            _patientRepo.Remove(obj); // Repository üzerinden sil
            _patientRepo.Save();      // Veritabanına işle
            TempData["success"] = "Hasta kaydı silindi.";
            return RedirectToAction("Index");
        }
    }
}