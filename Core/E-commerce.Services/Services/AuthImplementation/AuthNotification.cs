using E_commerce.Domain.Models.User;
using E_commerce.Shared.Dto_s.Notificaiton;
using E_commerce.Shared.EnumsHelper.Notification;
using E_commerce.Shared.EnumsHelper.User;

namespace E_commerce.Services.Services.AuthImplementation
{
    public partial class AuthService
    {

        private async Task NotifyAdminsOfNewUserAsync(ApplicationUser newUser)
        {
            var admins = await _userManager.GetUsersInRoleAsync(UserType.Admin.ToString());
            if (!admins.Any()) return;

            var notificationTasks = new List<Task>();

            foreach (var admin in admins)
            {
                var adminNotification = new NotificationContentDto
                {
                    UserId = admin.Id,
                    Subject = "تسجيل عميل جديد 🥳",
                    Body = $"سجل عميل جديد في النظام باسم: '{newUser.FullName}'.",
                    ReferenceId = newUser.Id,
                };
                notificationTasks.Add(_notificationService.SendNotificationAsync(adminNotification, NotificationType.Push));
            }

            await Task.WhenAll(notificationTasks);
        }

        private async Task SendWelcomeEmailAsync(ApplicationUser user)
        {
            var welcomeNotification = new NotificationContentDto
            {
                UserId = user.Id,
                Email = user.Email,
                Subject = "مرحباً بك في متجرنا! 🪄",
                Body = $"أهلاً بك يا {user.FullName} في منصتنا. ابدأ في تصميم تيشرتك السحري الآن!",
            };
            await _notificationService.SendNotificationAsync(welcomeNotification, NotificationType.Email);
        }

        private async Task SendOtpEmailAsync(ApplicationUser user, string otp)
        {
            var otpNotification = new NotificationContentDto
            {
                UserId = user.Id,
                Email = user.Email,
                Subject = "كود استعادة كلمة المرور 🔐",
                Body = $"كود الـ OTP الخاص بك هو: {otp}. صالح لمدة دقيقتين. لا تشارك هذا الكود مع أحد.",
            };
            await _notificationService.SendNotificationAsync(otpNotification, NotificationType.Email);
        }

        private async Task SendPasswordChangedAlertAsync(ApplicationUser user)
        {
            var securityNotification = new NotificationContentDto
            {
                UserId = user.Id,
                Email = user.Email,
                Subject = "تم تغيير كلمة المرور بنجاح ✅",
                Body = "تم تغيير كلمة المرور الخاصة بحسابك للتو. إذا لم تكن أنت من قام بهذا الإجراء، يرجى التواصل مع الدعم الفني فوراً.",
            };
            await _notificationService.SendNotificationAsync(securityNotification, NotificationType.Push, NotificationType.Email);
        }

    }
}