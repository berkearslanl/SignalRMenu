create trigger IncreaseMoneyCases
on Orders
after Update

create trigger IncreaseOrderTotalPrice
on orderdetails
after Insert
as
declare @OrderId int 
declare @OrderPrice decimal(18,2)
Select @OrderId=OrderId from inserted 
select @OrderPrice = TotalPrice from inserted
update Orders set TotalPrice+=@OrderPrice where OrderID=@OrderId



create trigger DecreaseOrderTotalPrice
on orderdetails
after delete
as
declare @OrderId int 
declare @OrderPrice decimal(18,2)
Select @OrderId=OrderId from deleted 
select @OrderPrice = TotalPrice from deleted
update Orders set TotalPrice-=@OrderPrice where OrderID=@OrderId



create trigger SumMoneyCases
on Orders
after update
as
declare @Description nvarchar(max)
declare @TotalPrice decimal(18,2)
select @Description = Description from inserted
select @TotalPrice = TotalPrice from inserted
if(@Description=N'Hesap Kapatıldı')
Begin
Update MoneyCases set TotalAmount+=@TotalPrice 
End


create trigger SetTableFull
on Orders
after insert
as
declare @TableId int
select @TableId=RestaurantTableId from inserted
update RestaurantTables set status = 1 where RestaurantTableId=@TableId

create trigger SetTableEmpty
on Orders
after update
as
declare @Description nvarchar(max)
declare @TableId int
select @Description = Description from inserted
select @TableId = RestaurantTableId from inserted
if(@Description = N'Hesap Ödendi')
begin
update RestaurantTable set Status = 0 where RestaurantTableId=@TableId
end