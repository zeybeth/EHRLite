using Microsoft.AspNetCore.Authorization;
using EHRLite.Models;
using EHRLite.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EHRLite.Controllers
{
    [Authorize]
    public class LabResultController : Controller
    {
        private readonly ILabResultRepository _labRepo;
        private readonly IVisitRepository _visitRepo;

        public LabResultController(ILabResultRepository labRepo, IVisitRepository visitRepo)
        {
            _labRepo = labRepo;
            _visitRepo = visitRepo;
        }

        // GET: Bir muayeneye ait tahlil sonuçlarını listele
        public IActionResult Index(int? visitId)
        {
            if (visitId == null || visitId == 0)
            {
                return NotFound("Muayene ID'si bulunamadı.");
            }

            // O muayeneye ait sonuçları getir
            List<LabResult> objList = _labRepo.GetAll(u => u.VisitId == visitId).ToList();

            // Başlıkta göstermek için Muayene bilgisini çekelim
            var visit = _visitRepo.Get(u => u.Id == visitId);
            if (visit != null)
            {
                ViewBag.VisitDate = visit.VisitDate.ToShortDateString();
                ViewBag.VisitId = visit.Id;
                // Geri dön butonunun nereye döneceğini bilmesi için PatientId lazım
                ViewBag.PatientId = visit.PatientId;
            }
            // GRAFİK İÇİN VERİ HAZIRLIĞI
            // Test isimlerini ve Değerlerini ayrı listelere ayırıyoruz
            var testNames = objList.Select(x => x.TestName).ToList();
            var testValues = objList.Select(x => x.Value).ToList();

            // Bu listeleri View'a gönderiyoruz
            ViewBag.TestNames = testNames;
            ViewBag.TestValues = testValues;

            return View(objList);
        }

        // GET: Yeni Tahlil Ekleme Sayfası
        public IActionResult Create(int visitId)
        {
            LabResult labResult = new LabResult();
            labResult.VisitId = visitId;
            return View(labResult);
        }

        // POST: Yeni Tahlil Kaydetme
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(LabResult obj)
        {
            if (ModelState.IsValid)
            {
                _labRepo.Add(obj);
                _labRepo.Save();
                TempData["success"] = "Tahlil sonucu eklendi.";
                return RedirectToAction("Index", new { visitId = obj.VisitId });
            }
            return View(obj);
        }

        // GET: Düzenleme Sayfası
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var result = _labRepo.Get(u => u.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        // POST: Düzenleme Kaydetme
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(LabResult obj)
        {
            if (ModelState.IsValid)
            {
                _labRepo.Update(obj);
                _labRepo.Save();
                TempData["success"] = "Tahlil güncellendi.";
                return RedirectToAction("Index", new { visitId = obj.VisitId });
            }
            return View(obj);
        }

        // GET: Silme Sayfası
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var result = _labRepo.Get(u => u.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        // POST: Silme İşlemi
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _labRepo.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            int visitId = obj.VisitId; // Geri dönüş için ID'yi sakla
            _labRepo.Remove(obj);
            _labRepo.Save();
            TempData["success"] = "Tahlil silindi.";

            return RedirectToAction("Index", new { visitId = visitId });
        }

        // GET: Belirli bir testin zaman içindeki değişimini gösterir (Longitudinal Chart)
        public IActionResult History(int patientId, string testName)
        {
            // 1. O hastanın tüm muayenelerini bul
            // 2. O muayenelerdeki, ismi 'testName' olan tüm sonuçları çek
            // 3. Tarihe göre sırala

            var historyData = _labRepo.GetAll(x => x.TestName == testName && x.Visit.PatientId == patientId, includeProperties: "Visit")
                                      .OrderBy(x => x.Visit.VisitDate)
                                      .Select(x => new
                                      {
                                          Date = x.Visit.VisitDate.ToShortDateString(),
                                          Value = x.Value
                                      })
                                      .ToList();

            ViewBag.TestName = testName;

            // Verileri grafik formatına ayır
            ViewBag.Dates = historyData.Select(x => x.Date).ToList();
            ViewBag.Values = historyData.Select(x => x.Value).ToList();

            return View();
        }
    }
}