using Microsoft.AspNetCore.Authorization;
using EHRLite.Models;
using EHRLite.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Include için gerekli

namespace EHRLite.Controllers
{
    [Authorize]
    // : Controller yazarak bu sınıfı bir MVC Controller'a dönüştürüyoruz
    public class VisitController : Controller
    {
        private readonly IVisitRepository _visitRepo;
        private readonly IPatientRepository _patientRepo;

        // Dependency Injection (Bağımlılık Enjeksiyonu) ile Repository'leri alıyoruz
        public VisitController(IVisitRepository visitRepo, IPatientRepository patientRepo)
        {
            _visitRepo = visitRepo;
            _patientRepo = patientRepo;
        }

        // GET: Belirli bir hastanın muayenelerini listele
        // URL'den patientId parametresi bekliyoruz (Örn: /Visit?patientId=5)
        public IActionResult Index(int? patientId)
        {
            if (patientId == null || patientId == 0)
            {
                return NotFound("Hasta ID'si bulunamadı.");
            }

            // Repository'deki yeni GetAll metodunu kullanarak filtreleme yapıyoruz.
            // includeProperties: "Patient" diyerek, Visit tablosunun yanına Hasta bilgilerini de çekiyoruz.
            List<Visit> objVisitList = _visitRepo.GetAll(u => u.PatientId == patientId, includeProperties: "Patient").ToList();

            // Eğer liste boşsa bile Hasta bilgilerini bulup View'a taşıyalım ki başlıkta "Ahmet Yılmaz'ın Muayeneleri" yazabilelim.
            var patient = _patientRepo.Get(u => u.Id == patientId);

            if (patient != null)
            {
                // View'a veri taşıma (ViewBag) - 2. PDF'te de bu yöntem kullanılmış [cite: 317]
                ViewBag.PatientName = patient.FirstName + " " + patient.LastName;
                ViewBag.PatientId = patient.Id;
            }
            else
            {
                return NotFound("Hasta veritabanında bulunamadı.");
            }

            return View(objVisitList);
        }

        // GET: Yeni Muayene Ekleme Sayfasını Açar
        // Hangi hastaya ekleyeceğimizi bilmek için patientId parametresi alıyoruz
        public IActionResult Create(int patientId)
        {
            // Boş bir Visit nesnesi oluşturup içine Hasta ID'sini koyuyoruz
            // Böylece form açıldığında ID'si hazır olacak.
            Visit visit = new Visit();
            visit.PatientId = patientId;

            return View(visit);
        }

        // POST: Muayeneyi Veritabanına Kaydeder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Visit obj)
        {
            if (ModelState.IsValid)
            {
                _visitRepo.Add(obj); // Ekle
                _visitRepo.Save();   // Kaydet
                TempData["success"] = "Muayene kaydı başarıyla oluşturuldu.";

                // Kayıt bitince o hastanın muayene listesine geri dön (patientId ile)
                return RedirectToAction("Index", new { patientId = obj.PatientId });
            }
            return View(obj);
        }
        // GET: Düzenleme Sayfasını Açar
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            // Düzenlenecek muayeneyi bul
            var visit = _visitRepo.Get(u => u.Id == id);

            if (visit == null)
            {
                return NotFound();
            }

            return View(visit);
        }

        // POST: Düzenlemeyi Kaydeder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Visit obj)
        {
            if (ModelState.IsValid)
            {
                _visitRepo.Update(obj);
                _visitRepo.Save();
                TempData["success"] = "Muayene başarıyla güncellendi.";

                // İşlem bitince yine o hastanın muayene listesine dönüyoruz
                return RedirectToAction("Index", new { patientId = obj.PatientId });
            }
            return View(obj);
        }
        // GET: Silme Onay Sayfası
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var visit = _visitRepo.Get(u => u.Id == id);

            if (visit == null)
            {
                return NotFound();
            }

            return View(visit);
        }

        // POST: Silme İşlemi
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _visitRepo.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            // Silmeden önce hasta ID'sini alalım ki geri dönebilelim
            int hastaId = obj.PatientId;

            _visitRepo.Remove(obj);
            _visitRepo.Save();
            TempData["success"] = "Muayene kaydı silindi.";

            return RedirectToAction("Index", new { patientId = hastaId });
        }

        // GET: Yazdırılabilir Muayene Özeti
        public IActionResult Summary(int id)
        {
            // Muayeneyi bulurken hem Hastayı (Patient) hem de Tahlilleri (LabResults) dahil ediyoruz.
            // Repository'deki GetAll metodunu filtre vererek "Get" gibi kullanıyoruz çünkü Include özelliğine ihtiyacımız var.
            var visit = _visitRepo.GetAll(u => u.Id == id, includeProperties: "Patient,LabResults").FirstOrDefault();

            if (visit == null)
            {
                return NotFound();
            }

            return View(visit);
        }
    }
}