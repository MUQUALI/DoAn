using Model.Bus;
using Model.CustomModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Model.CustomModel.OutBillDetail;

namespace Model.Dao
{
    public class BillDao
    {
        SecondHandDbContext DbContext = null;
        ProductBus PBus = null;
        public BillDao()
        {
            DbContext = DataProvider.GetInstance();
            PBus = new ProductBus();
        }

        public List<Bill> GetBills(string BillID, DateTime? FromDate, DateTime? ToDate, int? Status)
        {
            List<Bill> Result = DbContext.Bills.ToList();
            if(!String.IsNullOrEmpty(BillID))
            {
                Result = Result.Where(x => x.PK_Bill_ID == int.Parse(BillID)).ToList();
                return Result;
            }
            if (FromDate != null)
            {
                Result = Result.Where(x => x.CreatedDate >= FromDate).ToList();
            }

            if (ToDate != null)
            {
                Result = Result.Where(x => x.CreatedDate <= ToDate).ToList();
            }
            if(Status != null)
            {
                int status = (int)Status;
                Result = Result.Where(x => x.Status == status).ToList();
            }

            return Result.OrderByDescending(x => x.CreatedDate).ToList();
        }

        public string ChangeStatusBill(int id, int Status, int UserId)
        {
            Bill ItemChange = DbContext.Bills.Find(id);
            if(Status == 4)
            {
                foreach (BillDetail item in ItemChange.BillDetails)
                {
                    Product p = DbContext.Products.Find(item.ProductID);
                    p.Quantity += item.Quantity;
                }
            }
            ItemChange.FK_AccountID = UserId;
            ItemChange.Status = Status;

            DbContext.SaveChanges();
            return ItemChange.Status.ToString();
        }

        public OutBillDetail GetProductInfo(string code)
        {
            try
            {
                Product info = DbContext.Products.Find(code);
                List<OrderDetail> ListOrderCustom = DbContext.OrderDetails.Where(x => x.ProductID == code).ToList();
                List<OutCustom> ListCustom = new List<OutCustom>();

                foreach (OrderDetail item in ListOrderCustom)
                {
                    Custom c = DbContext.Customs.Find(item.FK_CustomID);
                    OutCustom custom = new OutCustom();
                    custom.CustomID = c.CustomID;
                    custom.Size = c.Size;
                    custom.Color = c.Color;

                    ListCustom.Add(custom);
                }
                OutBillDetail Detail = new OutBillDetail(info.PK_ProductID, info.Name, ListCustom);
                return Detail;
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public OutBillDetail AddInfoProduct(string code, int customId, int qty)
        {
            try
            {
                Product info = DbContext.Products.Find(code);
                Custom custom = DbContext.Customs.Find(customId);
                OutBillDetail Detail = new OutBillDetail(info.PK_ProductID, info.Name, custom.Size, custom.Color, qty, info.SalePrice, (qty * info.SalePrice));

                return Detail;
            }
            catch(Exception e)
            {
                return null;
            }
            
        }

        public string CreateBill(InBillCreate Model, List<InBillCreate.ItemDetail> ListDetail, int FK_Customer)
        {
            try
            {
                Bill BillCreate = new Bill();
                BillCreate.FK_CustomerID = FK_Customer;
                BillCreate.TotalMoney = Model.TotalMoney;
                BillCreate.Note = Model.Customer + " - " + Model.Phone + " - " + Model.Note;
                BillCreate.Discount = 0;
                BillCreate.Status = 3;
                BillCreate.DelFlg = 0;
                BillCreate.ShipAddress = Model.Note;
                BillCreate.PhoneContact = Model.Phone;
                BillCreate.CreatedDate = DateTime.Now;
                DbContext.Bills.Add(BillCreate);

                int BillID = BillCreate.PK_Bill_ID;

                foreach (InBillCreate.ItemDetail item in ListDetail)
                {
                    BillDetail Detail = new BillDetail();
                    Product ItemChange = DbContext.Products.Find(item.ProductID);

                    Detail.Bill_ID = BillID;
                    Detail.ProductID = item.ProductID;
                    // find custom
                    Custom ct = DbContext.Customs.Where(x => x.Size == item.Size && x.Color.Equals(item.Color)).FirstOrDefault();

                    Detail.FK_CustomID = ct.CustomID;
                    Detail.ProductName = item.ProductName;
                    Detail.Quantity = item.Quantity;
                    Detail.UnitPrice = item.UnitPrice;
                    Detail.Money = item.Money;
                    //change qty product
                    ItemChange.Quantity -= item.Quantity;
                    //save to db
                    DbContext.BillDetails.Add(Detail);
                }
                DbContext.SaveChanges();
                return "200";
            }catch(Exception e)
            {
                return "";
            }
        }

        public int GetMaxQtyOfProduct(string ProductID, int FK_Custom)
        {
            List<Inventory> Invens = PBus.GetInventory(ProductID);

            Inventory Inventory = Invens.Where(x => x.FK_CustomID == FK_Custom).FirstOrDefault();

            return Inventory.Quantity;
        }

        public Bill GetBillDetail(int Id)
        {
            return DbContext.Bills.Find(Id);
        }

        public int GiveBack(int Id, string Note)
        {
            Bill ItemChange =  DbContext.Bills.Find(Id);
            ItemChange.Status = 5;
            ItemChange.Note = Note;

            foreach (BillDetail item in ItemChange.BillDetails)
            {
                Product p = DbContext.Products.Find(item.ProductID);
                p.Quantity += item.Quantity;
            }

            DbContext.SaveChanges();
            return ItemChange.PK_Bill_ID;
        }
    }
}