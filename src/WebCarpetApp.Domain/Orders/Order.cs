using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace WebCarpetApp.Orders;

public class Order : AuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid UserId { get; private set; }
    public Guid? ReceivedId { get; private set; }
    public int OrderDiscount { get; private set; }
    public decimal OrderAmount { get; private set; }
    public decimal OrderTotalPrice { get; private set; }
    public OrderStatus OrderStatus { get; private set; }
    public int OrderRowNumber { get; private set; }
    public bool Active { get; private set; }
    public bool CalculatedUsed { get; private set; }
    Guid? IMultiTenant.TenantId => TenantId;
    
    private Order() { }
    
    public Order(
        Guid userId,
        Guid? receivedId,
        int orderDiscount,
        decimal orderAmount,
        OrderStatus orderStatus = OrderStatus.Passive,
        int orderRowNumber = 0)
    {
        UserId = userId;
        ReceivedId = receivedId;
        OrderDiscount = orderDiscount;
        OrderAmount = orderAmount;
        OrderStatus = orderStatus;
        OrderRowNumber = orderRowNumber;
        Active = true;
        CalculatedUsed = false;
        
        // Discount hesaplama
        CalculateOrderTotalPrice();
    }
    
    public void CalculateOrderTotalPrice()
    {
        OrderTotalPrice = OrderAmount * (1 - (decimal)OrderDiscount / 100);
    }
    
    public void UpdateStatus(OrderStatus status)
    {
        OrderStatus = status;
        
        // Cancelled durumunda Active false olmalı
        if (status == OrderStatus.Cancelled)
        {
            SetActive(false);
        }
    }
    
    public void SetActive(bool active)
    {
        Active = active;
    }
    
    public void UpdateDiscount(int discount)
    {
        OrderDiscount = discount;
        CalculateOrderTotalPrice();
    }
    
    public void UpdateAmount(decimal amount)
    {
        OrderAmount = amount;
        CalculateOrderTotalPrice();
    }
    
    public void UpdateRowNumber(int rowNumber)
    {
        OrderRowNumber = rowNumber;
    }
    
    public void SetCalculatedUsed(bool calculatedUsed)
    {
        CalculatedUsed = calculatedUsed;
    }
    
    public void UpdateReceivedId(Guid? receivedId)
    {
        ReceivedId = receivedId;
    }
}