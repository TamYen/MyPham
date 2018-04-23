using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyPham.Models;

namespace MyPham.Controllers
{
    public class GiohangController : Controller
    {
        // GET: Giohang
        public ActionResult Index()
        {
            return View();
        }

        DbMyPhamDataContext db = new DbMyPhamDataContext();

        public List<Giohang> Laygiohang()
        {
            List<Giohang> listG = Session["giohang"] as List<Giohang>;
            if (listG == null)
            {
                listG = new List<Giohang>();
                Session["giohang"] = listG;
            }
            return listG;
        }

        //them gio hang
        public ActionResult Themgiohang(int idSp, string strUrl)
        {
            // lay session gio hang
            List<Giohang> listGio = Laygiohang();
            // kiem tra sp da co trong gio hang chua?
            Giohang sp = listGio.Find(n => n.idSp == idSp);
            if (sp == null)
            {
                sp = new Giohang(idSp);
                listGio.Add(sp);
                return Redirect(strUrl);
            }
            else
            {
                sp.soLuong++;
                return Redirect(strUrl);
            }
        }

        // tong so luong
        private int tongSl()
        {
            int s = 0;
            List<Giohang> listG = Session["giohang"] as List<Giohang>;
            if (listG != null)
            {
                s = listG.Sum(n => n.soLuong);

            }
            return s;
        }

        // tinh tong tien
        private double tongTien()
        {
            double s = 0;
            List<Giohang> listG = Session["giohang"] as List<Giohang>;
            if (listG != null)
            {
                s = listG.Sum(n => n.thanhTien);
            }
            return s;
        }

        public ActionResult Giohang()
        {
            List<Giohang> listG = Laygiohang();
            if (listG.Count == 0)
            {
                return RedirectToAction("index", "mypham");
            }
            ViewBag.tongsoluong = tongSl();
            ViewBag.tongtien = tongTien();
            return View(listG);
        }

        public ActionResult SoluongPartial()
        {
            ViewBag.tongsoluong = tongSl();
            return PartialView();
        }

        public ActionResult TongtienPartial()
        {
            ViewBag.tongtien = tongTien();
            return PartialView();
        }

        public ActionResult Xoagiohang(int id)
        {
            List<Giohang> listG = Laygiohang();
            Giohang sp = listG.SingleOrDefault(n => n.idSp == id);
            if (sp != null)
            {
                listG.RemoveAll(n => n.idSp == id);
                return RedirectToAction("Giohang");
            }
            if (listG.Count == 0)
            {
                return RedirectToAction("Index", "MyPham");
            }
            return RedirectToAction("Giohang");
        }

        public ActionResult Capnhatgiohang(int id, FormCollection f)
        {
            // lay hang tu session
            List<Giohang> listG = Laygiohang();
            // kiemtra sach dax co trong gio hang
            Giohang sp = listG.SingleOrDefault(n => n.idSp == id);
            // chi sua so luong khi no ton tai
            if (sp != null)
            {
                sp.soLuong = int.Parse(f["soluong"].ToString());
            }
            return RedirectToAction("Giohang");
        }

        public ActionResult Xoahet()
        {
            List<Giohang> listG = Laygiohang();
            listG.Clear();
            return RedirectToAction("Index", "Mypham");
        }

        [HttpGet]
        public ActionResult Dathang()
        {
            if(Session["Taikhoan"]==null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Dangnhap", "Nguoidung");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "Mypham");
            }
            // lay gio hang tu session
            List<Giohang> listG = Laygiohang();
            ViewBag.Tongsoluong = tongSl();
            ViewBag.Tongtien = tongTien();
            return View(listG);
        }

        [HttpPost]
        public ActionResult Dathang(FormCollection f)
        {
            DonDatHang ddh = new DonDatHang();
            KhachHang kh = (KhachHang)Session["Taikhoan"];
            List<Giohang> listG = Laygiohang();
            ddh.MaKH = kh.MaKH;
            ddh.NgayDat = DateTime.Now;
            
            //try
            //{
                var ngaygiao = string.Format("{0:MM/dd/YYYY}", f["Ngaygiao"]);
                ddh.NgayGiao = DateTime.Parse(ngaygiao);
                ddh.TinhTrangGiaoHang = false;
                ddh.DaThanhToan = false;
                ddh.Tongtien = (int)tongTien();
                db.DonDatHangs.InsertOnSubmit(ddh);
                db.SubmitChanges();

                foreach (var item in listG)
                {
                    ChiTietDDH ct = new ChiTietDDH();
                    ct.MaDH = ddh.MaDH;
                    ct.MaSP = item.idSp;
                    ct.SoLuong = item.soLuong;
                    ct.Dongia = (int)item.giaSp;
                    db.ChiTietDDHs.InsertOnSubmit(ct);
                }
                db.SubmitChanges();
                Session["Giohang"] = null;
                Session["Taikhoan"] = null;
                return RedirectToAction("Xacnhandonhang", "Giohang");
            }
            //catch (Exception e)
            //{
                
            //    ViewData["LoiDathang"] = "Ngày giao chưa phù hợp";
            //    //return RedirectToAction("Dathang", "Giohang");
            //}
            
        

        public ActionResult Xacnhandonhang()
        {
            return View();
        }

    }
}