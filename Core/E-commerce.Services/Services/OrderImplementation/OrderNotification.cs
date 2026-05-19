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

            Console.WriteLine($"📨 Sending email to: {user.Email ?? "NULL"}");

            var customerBody = $"مرحباً {user.FullName}،\n\n" +
                               $"شكراً لتسوقك من Mine Store! ✨\n" +
                               $"تم استلام طلبك بنجاح، وجاري الآن تجهيزه وطباعة تصميماتك المخصصة.\n\n" +
                               $"رقم الطلب: {order.OrderNumber}\n\n" +
                               $"سنتواصل معك قريباً بمجرد الشحن.";

            var customerNotification = new NotificationContentDto
            {
                UserId = order.UserId,
                Email = user.Email,
                Subject = "تم استلام طلبك بنجاح! 👕✨",
                Body = customerBody,
                ReferenceId = order.Id.ToString(),
            };
            await notificationService.SendNotificationAsync(customerNotification, NotificationType.Push, NotificationType.Email);

            var admins = await userManager.GetUsersInRoleAsync(UserType.Admin.ToString());
            if (admins.Any())
            {
                var adminBody = $"طلب جديد متاح الآن! 📦\n\n" +
                                $"رقم الطلب: {order.OrderNumber}\n" +
                                $"العميل: {user.FullName}\n" +
                                $"القيمة الإجمالية: {order.TotalAmount} جنيه\n\n" +
                                $"يرجى مراجعة تفاصيل التخصيص من لوحة التحكم.";

                var notificationTasks = new List<Task>();
                foreach (var admin in admins)
                {
                    var adminNotification = new NotificationContentDto
                    {
                        UserId = admin.Id,
                        Email = admin.Email, 
                        Subject = "طلب جديد! 📦",
                        Body = adminBody,
                        ReferenceId = order.Id.ToString(),
                    };

                    notificationTasks.Add(notificationService.SendNotificationAsync(adminNotification, NotificationType.Push, NotificationType.Email));
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
                OrderStatus.Delivered => "تم تسليم الطلب بنجاح. نتمنى أن تنال المنتجات إعجابك! ❤️",
                OrderStatus.Cancelled => "للأسف، تم إلغاء طلبك. يمكنك التواصل معنا لمزيد من التفاصيل.",
                _ => $"تم تحديث حالة الطلب إلى: {order.OrderStatus}"
            };

            var updateBody = $"مرحباً {user.FullName}،\n\n" +
                             $"تحديث جديد بخصوص طلبك من Mine Store.\n\n" +
                             $"رقم الطلب: {order.OrderNumber}\n" +
                             $"الحالة: {statusMessage}\n\n" +
                             $"شكراً لثقتك بنا!";

            var customerNotification = new NotificationContentDto
            {
                UserId = order.UserId,
                Email = user.Email,
                Subject = "تحديث بخصوص طلبك 🛍️",
                Body = updateBody,
                ReferenceId = order.Id.ToString(),
            };

            await notificationService.SendNotificationAsync(customerNotification, NotificationType.Push, NotificationType.Email);
        }
    }
}