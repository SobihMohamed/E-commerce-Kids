using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Shared.EnumsHelper.Notification
{
    public enum NotificationType
    {
        // Admin Notifications
        NewOrderPlaced = 1,
        NewCustomerRegistered = 2,

        // Customer Notifications
        OrderProcessing = 10,     
        OrderShipped = 11,         
        OrderDelivered = 12,       
        OrderCancelled = 13,

        // System Notifications
        SystemAlert = 20        
    }
}
