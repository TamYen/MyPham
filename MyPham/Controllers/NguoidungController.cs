using MyPham.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyPham.Controllers
{
    public class NguoidungController : Controller
    {
        DbMyPhamDataContext db = new DbMyPhamDataContext();
        // GET: Nguoidung
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Dangky()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Dangky(FormCollection col, KhachHang kh)
        {
            var hoten = col["hoten"];
            var tenDN = col["tenDN"];
            var matkhau = col["matkhau"];
            var nhaplaiMK = col["nhaplaiMK"];
            var email = col["Email"];
            var ngsinh = string.Format("{0:MM/dd/YYYY}", col["ngaysinh"]);
            var dienthoai = col["Dienthoai"];
            var diachi = col["diachi"];

            if (string.IsNullOrEmpty(hoten))
            {
                ViewData["loi1"] = "Vui lòng nhập tên của bạn";
            }else if (string.IsNullOrEmpty(tenDN))
            {
                ViewData["loi2"] = "Cần phải có tên đang nhập";
            }else if (string.IsNullOrEmpty(matkhau))
            {
                ViewData["loi3"] = "Phải nhập mật khẩu";
            }else if (string.IsNullOrEmpty(nhaplaiMK))
            {
                ViewData["loi4"] = "Phải nhập lại mật khẩu";
            }

            if (string.IsNullOrEmpty(email))
            {
                ViewData["loi5"] = "Phải nhập email của bạn";
            }

            if (string.IsNullOrEmpty(dienthoai))
            {
                ViewData["loi6"] = "Vui lòng nhập SDT";
            }else if (string.IsNullOrEmpty(diachi))
            {
                ViewData["loi7"] = "phải nhập địa chỉ";
            }else
            {
                kh.TenKH = hoten;
                kh.TaiKhoan = tenDN;
                kh.MatKhau = matkhau;
                kh.Email = email;
                kh.DiaChiKH = diachi;
                kh.DienThoaiKH = dienthoai;
                kh.NgaySinh = DateTime.Parse(ngsinh);
                db.KhachHangs.InsertOnSubmit(kh);
                db.SubmitChanges();
               // return RedirectToAction("Dangnhap");    
            }

            return RedirectToAction("Dangnhap");
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangnhap(FormCollection col)
        {
            var tenDN = col["tenDN"];
            var matkhau = col["matkhau"];

            if (string.IsNullOrEmpty(tenDN))
            {
                ViewData["loi1"] = "Nhập tên người dùng của bạn";
            }else if (string.IsNullOrEmpty(matkhau))
            {
                ViewData["loi2"] = "Phải nhập mật khẩu";
            }else
            {
                KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.TaiKhoan == tenDN && n.MatKhau == matkhau);
                if(kh != null)
                {
                    ViewBag.thongbao2 = "Đăng nhập thành công";
                    Session["Taikhoan"] = kh;
                    return RedirectToAction("Giohang", "giohang");
                }
                else
                {
                    ViewBag.thongbao = "tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }
    }
}