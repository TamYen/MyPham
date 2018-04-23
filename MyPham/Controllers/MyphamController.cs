using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_DoAn.Controllers;
using MyPham.Models;
using PagedList;
using PagedList.Mvc;

namespace MVC_DoAn.Controllers
{
    public class MyPhamController : Controller
    {
        DbMyPhamDataContext data = new DbMyPhamDataContext();
        private List<SanPham> Ar1;
        private List<SanPham> Ar2;

        private List<SanPham> LayHangMoi(int count1, int count2)
        {
            List<SanPham> ar = new List<SanPham>();
            Ar1 = data.SanPhams.OrderByDescending(a => a.NgayCapNhat).Take(count1).ToList();
            Ar2 = data.SanPhams.OrderBy(a => a.NgayCapNhat).Take(count2).ToList();
            foreach (var a in Ar1)
            {
                ar.Add(a);
            }
            foreach (var a in Ar2)
            {
                ar.Add(a);
            }
            return ar;

        }


        // GET: MyPham
        public ActionResult Index()
        {
            var hangmoi = LayHangMoi(6, 3);
            return View(hangmoi);
        }

        public ActionResult LoaiSP()
        {
            var loaiSP = from a in data.LoaiSPs select a;
            return PartialView(loaiSP);
        }
        public ActionResult NhaSX()
        {
            var nsx = from a in data.NhaSanXuats select a;
            return PartialView(nsx);
        }

        public ActionResult SpTheoLoai(int id, int ? page)
        {
            int pageSize = 12;
            int pageNum = (page ?? 1);
            var sp = from s in data.SanPhams.OrderByDescending(a=>a.NgayCapNhat) where s.MaLoai == id select s;
            return View(sp.ToPagedList(pageNum,pageSize));
        }

        public ActionResult SpTheoNSX(int id, int? page)
        {
            int pageSize = 12;
            int pageNum = (page ?? 1);
            var sp = from s in data.SanPhams.OrderByDescending(a => a.NgayCapNhat) where s.MaNSX == id select s;
            return View(sp.ToPagedList(pageNum,pageSize));
        }

        public ActionResult Details(int id)
        {
            var sp = from s in data.SanPhams where s.MaSP == id select s;
            return View(sp.Single());
        }
        [HttpPost]
        public ActionResult KQtimkiem(string Search, int? page)
        {

            List<SanPham> kqTk = data.SanPhams.Where(n => n.TenSP.Contains(Search)).ToList();

            int pageSize = 12;
            int pageNum = (page ?? 1);

            if (kqTk.Count == 0)
            {
                return View(data.SanPhams.OrderBy(n => n.TenSP).ToPagedList(pageNum, pageSize));
            }
            
            return View(kqTk.OrderBy(n => n.TenSP).ToPagedList(pageNum, pageSize));
        }
        [HttpGet]
        public ActionResult search()
        {
            return View();
        }
    }
}