/* =====================================================================
   SignalRMenu - Veritabanı Trigger'ları
   Not: Sipariş toplamı KDV dahil (%20) hesaplanır ve kasaya sipariş
   anında eklenir. Bu yüzden eski "Hesap Ödendi" ile kasaya ekleyen
   SumMoneyCases trigger'ı kaldırılmıştır.
   ===================================================================== */

-- Eski kasa trigger'ı (kasa artık OrderDetails trigger'ında güncelleniyor)
DROP TRIGGER IF EXISTS SumMoneyCases;
GO

-- Sipariş eklenince masayı "Dolu" yapar
CREATE OR ALTER TRIGGER SetTableFull
ON Orders
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE t
    SET t.Status = 1
    FROM RestaurantTables t
    JOIN inserted i ON i.RestaurantTableId = t.RestaurantTableId;
END
GO

-- Hesap ödendi olarak işaretlenince masayı "Boş" yapar
CREATE OR ALTER TRIGGER SetTableEmpty
ON Orders
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE t
    SET t.Status = 0
    FROM RestaurantTables t
    JOIN inserted i ON i.RestaurantTableId = t.RestaurantTableId
    WHERE i.Description = N'Hesap Ödendi';
END
GO

-- Sipariş detayı eklenince: siparişin toplamına ve kasaya KDV dahil tutar ekler
CREATE OR ALTER TRIGGER IncreaseOrderTotalPrice
ON OrderDetails
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE o
    SET o.TotalPrice += x.Toplam * 1.20
    FROM Orders o
    JOIN (SELECT OrderId, SUM(TotalPrice) AS Toplam FROM inserted GROUP BY OrderId) x
      ON x.OrderId = o.OrderID;

    UPDATE MoneyCases
    SET TotalAmount += (SELECT SUM(TotalPrice) FROM inserted) * 1.20;
END
GO

-- Sipariş detayı silinince: siparişin toplamından ve kasadan KDV dahil tutarı düşer
CREATE OR ALTER TRIGGER DecreaseOrderTotalPrice
ON OrderDetails
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE o
    SET o.TotalPrice -= x.Toplam * 1.20
    FROM Orders o
    JOIN (SELECT OrderId, SUM(TotalPrice) AS Toplam FROM deleted GROUP BY OrderId) x
      ON x.OrderId = o.OrderID;

    UPDATE MoneyCases
    SET TotalAmount -= (SELECT SUM(TotalPrice) FROM deleted) * 1.20;
END
GO
