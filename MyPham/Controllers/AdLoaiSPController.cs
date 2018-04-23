using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyPham.Models;
using PagedList.Mvc;
using PagedList;

namespace MyPham.Controllers
{
    public class AdLoaiSPController : Controller
    {
        DbMyPhamDataContext db = new DbMyPhamDataContext();
        // GET: AdLoaiSP
        public ActionResult QLloaisp()
        {
            if(Session["Taikhoanadmin"] == null)
            {

            }

            return View(db.LoaiSPs.OrderBy(n => n.TenLoai));
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            LoaiSP lsp = db.LoaiSPs.SingleOrDefault(n => n.MaLoai == id);
            if (lsp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            else
            {
                return View(lsp);
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(LoaiSP l)
        {
            LoaiSP lsp = db.LoaiSPs.SingleOrDefault(n => n.MaLoai == l.MaLoai);
            lsp.TenLoai = l.TenLoai;
            db.SubmitChanges();
            return RedirectToAction("QLloaiSp");
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(LoaiSP l)
        {
            db.LoaiSPs.InsertOnSubmit(l);
            db.SubmitChanges();
            return RedirectToAction("QLloaiSP");
        }


        [HttpGet]
        public ActionResult Delete(int id)
        {
            LoaiSP l = db.LoaiSPs.SingleOrDefault(n => n.MaLoai == id);
            if(l == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View();
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult Xoa(int id)
        {
            LoaiSP l = db.LoaiSPs.SingleOrDefault(n => n.MaLoai == id);
            if(l == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.LoaiSPs.DeleteOnSubmit(l);
            db.SubmitChanges();
            return RedirectToAction("QLloaiSP");
        }
    }
}