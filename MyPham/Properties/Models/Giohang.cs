using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPham.Models
{
    public class Giohang
    {
        DbMyPhamDataContext db = new DbMyPhamDataContext();

        public int idSp { set; get; }
        public string tenSp { set; get; }
        public string anhSp { set; get; }
        public double giaSp { set; get; }
        public int soLuong { set; get; }
        public double thanhTien
        {
            get { return giaSp * soLuong; }
        }

        public Giohang(int idSp)
        {
            this.idSp = idSp;
            SanPham sp = db.SanPhams.Single(n => n.MaSP == idSp);
            this.tenSp = sp.TenSP;
            this.anhSp = sp.HinhAnh;
            this.giaSp = double.Parse(sp.GiaBan.ToString());
            soLuong = 1;
        }
    }
}