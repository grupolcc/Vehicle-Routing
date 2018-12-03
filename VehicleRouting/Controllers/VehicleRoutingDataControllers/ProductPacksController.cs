using System.Linq;
using System.Net;
using System.Web.Mvc;
using VehicleRouting.Models;

namespace VehicleRouting.Controllers.VehicleRoutingDataControllers
{
    public class ProductPacksController : Controller
    {
        private VehicleDbContext db = new VehicleDbContext();

        // GET: ProductPacks
        public ActionResult Index()
        {
            return this.View(this.db.ProductPacks.ToList());
        }

        // GET: ProductPacks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductPack productPack = this.db.ProductPacks.Find(id);
            if (productPack == null)
            {
                return this.HttpNotFound();
            }
            return this.View(productPack);
        }

        // GET: ProductPacks/Create
        public ActionResult Create()
        {
            this.ViewBag.Products =
                this.db.Products.Select(v => new SelectListItem {Text = v.Name, Value = v.ID.ToString()}).ToList();
            this.ViewBag.PointsOfDelivery = this.db.PointsOfDeliveries
                .Select(v => new SelectListItem {Text = v.Name, Value = v.ID.ToString()}).ToList();
            return this.View();
        }

        // POST: ProductPacks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductPackInputModel productPack)
        {
            if (this.ModelState.IsValid)
            {
                ProductPack pack = new ProductPack
                {
                    Product = this.db.Products.First(v => v.ID == productPack.ProductID),
                    Amount = productPack.Amount,
                    PointOfDelivery = this.db.PointsOfDeliveries.First(v => v.ID == productPack.PointOfDeliveryID)
                };

                this.db.ProductPacks.Add(pack);
                this.db.SaveChanges();
                return this.RedirectToAction("Index");
            }

            return this.View(productPack);
        }

        // GET: ProductPacks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductPack productPack = this.db.ProductPacks.Find(id);
            if (productPack == null)
            {
                return this.HttpNotFound();
            }
            return this.View(productPack);
        }

        // POST: ProductPacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductPack productPack = this.db.ProductPacks.Find(id);
            this.db.ProductPacks.Remove(productPack);
            this.db.SaveChanges();
            return this.RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
