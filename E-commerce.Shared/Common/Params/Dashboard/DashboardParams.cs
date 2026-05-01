using System;

namespace E_commerce.Shared.Common.Params.Dashboard
{
    public class DashboardParams
    {
        // 1. فلترة بالأيام (الديفولت 30 يوم)
        public int? Days { get; set; } = 30;

        // 2. فلترة بتواريخ مخصصة (لو العميل اختار من النتيجة)
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // 3. التحكم في حجم الـ Lists اللي راجعة عشان منعملش Load على السيرفر
        public int RecentOrdersCount { get; set; } = 5; // هات آخر 5 طلبات بس
        public int TopProductsCount { get; set; } = 5;  // هات أكتر 5 منتجات مبيعاً بس
    }
}