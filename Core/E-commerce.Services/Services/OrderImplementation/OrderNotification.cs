using E_commerce.Domain.Models.Order;
using E_commerce.Shared.Dto_s.Notificaiton;
using E_commerce.Shared.EnumsHelper.Notification;
using E_commerce.Shared.EnumsHelper.Order;
using E_commerce.Shared.EnumsHelper.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.Services.OrderImplementation
{
    public partial class OrderService
    {
        private async Task NotifyOnOrderCreationAsync(OrderEntity order)
        {
            var user = await userManager.FindByIdAsync(order.UserId);
            if (user == null) return;

            var customerNotification = new NotificationContentDto
            {
                UserId = order.UserId,
                Email = user.Email,
                Subject = "تم استلام طلبك بنجاح! 👕✨",
                Body = $"شكراً لطلبك من Radiant Studio يا {user.FullName}! جاري الآن تجهيز طلبك رقم {order.OrderNumber} وطباعة التصميمات الكرتونية المخصصة لملابس طفلك. سنتواصل معك قريباً بمجرد الشحن.",
                ReferenceId = order.Id.ToString(), 
            };
            await notificationService.SendNotificationAsync(customerNotification, NotificationType.Push, NotificationType.Email);

            var admins = await userManager.GetUsersInRoleAsync(UserType.Admin.ToString());
            if (admins.Any())
            {
                var notificationTasks = new List<Task>();
                foreach (var admin in admins)
                {
                    var adminNotification = new NotificationContentDto
                    {
                        UserId = admin.Id,
                        Subject = "طلب جديد! 📦",
                        Body = $"تم استلام طلب جديد رقم {order.OrderNumber} بقيمة {order.TotalAmount} جنيه من العميل {user.FullName}. يرجى مراجعة تفاصيل التخصيص.",
                        ReferenceId = order.Id.ToString(),
                    };
                    notificationTasks.Add(notificationService.SendNotificationAsync(adminNotification, NotificationType.Push));
                }
                await Task.WhenAll(notificationTasks);
            }
        }

        private async Task NotifyOnOrderStatusUpdateAsync(OrderEntity order)
        {
            var user = await userManager.FindByIdAsync(order.UserId);
            if (user == null) return;

            string statusMessage = order.OrderStatus switch
            {
                OrderStatus.Processing => "تم تأكيد طلبك! جاري الآن تجهيز ملابسك المخصصة للطباعة. ✨",
                OrderStatus.Shipped => "خبر سعيد! طلبك في الطريق إليك الآن. 🚚",
                OrderStatus.Delivered => "تم تسليم الطلب. نتمنى أن تنال ملابس Radiant Studio إعجاب طفلك! ❤️",
                OrderStatus.Cancelled => "للأسف، تم إلغاء طلبك. يمكنك التواصل معنا لمعرفة السبب.",
                _ => $"تم تحديث حالة طلبك رقم {order.OrderNumber} إلى {order.OrderStatus}"
            };

            var customerNotification = new NotificationContentDto
            {
                UserId = order.UserId,
                Email = user.Email,
                Subject = "تحديث بخصوص طلبك 🛍️",
                Body = $"مرحباً {user.FullName}، {statusMessage}",
                ReferenceId = order.Id.ToString(),
            };

            await notificationService.SendNotificationAsync(customerNotification, NotificationType.Push, NotificationType.Email);
        }
    }
}