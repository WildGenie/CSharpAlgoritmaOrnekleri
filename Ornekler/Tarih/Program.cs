using System;

namespace Tarih
{
    internal class Program
    {
        private static readonly string[] DayOfWeekName =
        {
            "Pazar", "Pazartesi", "Salı", "Çarşamba", "Perşembe", "Cuma",
            "Cumartesi"
        };

        private static readonly string[] MonthOfName =
        {
            "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül",
            "Ekim", "Kasım", "Aralık"
        };

        private static void Main(string[] args)
        {
            // Başlangıç tarihimizi ayarlayalım
            int y = 2016;
            int m = 1;
            int d = 1;

            Console.WriteLine(
                "Başlangıç tarihini aralarına nokta koyarak gün, ay, yıl şeklinde girin [Varsayılan: 1.1.2016]:");
            string readLine = Console.ReadLine();
            if (readLine != "")
            {
                string[] startDateRead = readLine.Split('.');
                int.TryParse(startDateRead[0], out d);
                int.TryParse(startDateRead[1], out m);
                int.TryParse(startDateRead[2], out y);
            }
            Console.WriteLine("Girilen Tarih: {0} {1} {2} {3}", d, MonthOfName[m - 1], y,
                DayOfWeekName[DayOfWeek(y, m, d)]);

            // Bitiş tarihimizi ayarlayalım
            int dY = 2016;
            int dM = 12;
            int dD = 31;

            Console.WriteLine(
                "Bitiş tarihini aralarına nokta koyarak gün, ay, yıl şeklinde girin [Varsayılan: 31.12.2016]:");
            readLine = Console.ReadLine();
            if (readLine != "")
            {
                string[] startDateRead = readLine.Split('.');
                int.TryParse(startDateRead[0], out dD);
                int.TryParse(startDateRead[1], out dM);
                int.TryParse(startDateRead[2], out dY);
            }
            Console.WriteLine("Girilen Tarih: {0} {1} {2} {3}", dD, MonthOfName[dM - 1], dY,
                DayOfWeekName[DayOfWeek(dY, dM, dD)]);

            int firstDayOfWeek = DayOfWeek(y, m, d);
            int dayOfWeek = firstDayOfWeek;
            Console.WriteLine("Haftanın kaçıncı günü için çıktı almak istiyorsunuz.");
            Console.WriteLine(
                "(0:\"Pazar\", 1:\"Pazartesi\", 2:\"Salı\", 3:\"Çarşamba\", 4:\"Perşembe\", 5:\"Cuma\", 6:\"Cumartesi\" [Varsayılan: {0}]:",
                dayOfWeek);
            readLine = Console.ReadLine();
            if (readLine != "")
                int.TryParse(readLine, out dayOfWeek);

            // Listelenmesi istenilen günün denk geldiği ilk tarihi bulmak için
            // ilk verilen tarihin günü ile istenilen günün tarihini çıkartıyoruz
            // eksi rakam çıkmaması için haftanın gün sayısını ekleyip gün sayısına modluyoruz
            // çıkan sonucu tarihe ekliyoruz
            int diffFirstDay = (7 + dayOfWeek - firstDayOfWeek) % 7;
            AddDay(diffFirstDay, ref y, ref m, ref d);

            // başlangıç bitiş tarihleri arasındaki toplam gün sayısını buluyoruz
            int diffDay = NumberOfDaysInYears(dY) - NumberOfDaysInYears(y) + NumberOfDaysSinceYearStart(dY, dM, dD) -
                          NumberOfDaysSinceYearStart(y, m, d);

            // İlk günü yazdırıyoruz ve gün farkı bitene kadar birer hafta ekleyerek yazdırmaya devam ediyoruz.
            Console.WriteLine("{0} {1} {2} {3}", d, MonthOfName[m - 1], y, DayOfWeekName[DayOfWeek(y, m, d)]);
            while (diffDay >= 7)
            {
                AddDay(7, ref y, ref m, ref d);
                Console.WriteLine("{0} {1} {2} {3}", d, MonthOfName[m - 1], y, DayOfWeekName[DayOfWeek(y, m, d)]);
                diffDay -= 7;
            }
            Console.ReadLine();
        }

        /// <summary>
        ///     İstenilen yılın artık yıl olup olmadığını bulur
        /// </summary>
        /// <param name="year">Yıl</param>
        /// <returns>true: Artık yıl, false: Artık yıl değil.</returns>
        private static bool IsLeap(int year)
        {
            // 100'e kalansız bölünemeyen ve 4'e kalansız bölünebilen tüm yıllar
            // veya 400'e kalansız bölünebilen yıllar artık yıldır.
            // Artık yıllar da Şubat ayı 29 gündür.
            // https://tr.wikipedia.org/wiki/Art%C4%B1k_y%C4%B1l
            return year % 4 == 0 && year % 100 != 0 || year % 400 == 0;
        }

        /// <summary>
        ///     İstenilen tarihin yılın kaçıncı günü olduğunu bulur
        /// </summary>
        /// <param name="year">Yıl</param>
        /// <param name="month">Ay</param>
        /// <param name="day">Gün</param>
        /// <returns></returns>
        private static int NumberOfDaysSinceYearStart(int year, int month, int day)
        {
            // Aylara ait gün sayısı aşağıdaki gibidir:
            // [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31]
            // https://tr.wikipedia.org/wiki/Miladi_takvim#Y.C4.B1lba.C5.9F.C4.B1

            // İstenilen aya kadar olan günleri topladığımızda aşağıdaki tablo oluşmaktadır
            int[] days =
            {
                0, 31, 59, 90, 120, 151,
                181, 212, 243, 273, 304, 334
            };

            // Ayın gün sayısını da eklediğimiz zaman yılın kaçıncı günü olduğunu hesaplamış oluruz
            int result = days[month - 1] + day;

            // Bu hesaba artık yıllar dahil ediyoruz
            if (month > 2)
                result += IsLeap(year) ? 1 : 0;

            return result;
        }

        /// <summary>
        ///     İstenilen yıla kadar olan toplam gün sayısı
        /// </summary>
        /// <param name="year">Yıl</param>
        /// <returns>Gün sayısı</returns>
        private static int NumberOfDaysInYears(int year)
        {
            // Hesaplanmak istenen yıla kadar olan, artık yıllar hariç toplam gün sayısı
            int normalDays = 365 * year;

            // Toplamda ki artık gün sayısı (dört yılda bir, 100 yılda bir hariç, 400 yılda bir dahil
            int leapDays = year / 4 - year / 100 + year / 400;

            return normalDays + leapDays;
        }

        /// <summary>
        ///     Verilen tarihin haftanın kaçıncı gününe ait olduğunu bulur.
        ///     Projede kullanılmamaktadır referans amaçlıdır
        /// </summary>
        /// <param name="year">Yıl</param>
        /// <param name="month">Ay</param>
        /// <param name="day">Gün</param>
        /// <returns>Pazar günü 0 olarak kabul edilir</returns>
        private int DayOfWeek2(int year, int month, int day)
        {
            // Miladdan önce 1 yıl 31 aralık tarihi pazar günü olarak düşündüğümüzde
            // geçtiği yıla kadar olan günler ve yılın kaçıncı günü olduğunu toplayıp
            // 7 ye bölündüğünde kalanı aldığımızda bize haftanın kaçıncı günü olduğunu verecektir.
            // Not: Haftanın sıfırıncı günü pazar günü olduğunu unutmayın.
            int result = (
                             NumberOfDaysInYears(year - 1) +
                             NumberOfDaysSinceYearStart(year, month, day)
                         ) % 7;

            return result;
        }

        /// <summary>
        ///     Verilen tarihin haftanın kaçıncı gününe ait olduğunu bulur
        /// </summary>
        /// <param name="year">Yıl</param>
        /// <param name="month">Ay</param>
        /// <param name="day">Gün</param>
        /// <returns>Pazar günü 0 olarak kabul edilir</returns>
        private static int DayOfWeek(int year, int month, int day)
        {
            // Biraz önce yaptığımız işlemleri biraz daha sadeleştirelim şimdi
            // 365 günde bir gün değişir (365 % 7 => 1)
            // Yıla artık yıl sayısını ekleyelim
            int lastYear = year - 1;
            int dayOfWeekDiff = year + lastYear / 4 - lastYear / 100 + lastYear / 400;

            // Aya kadar olan gün sayısını da modlayıp ayın başında kaç gün oynadığını bulalım
            // int[] days = { 0, 31, 59, 90, 120, 151,
            //               181, 212, 243, 273, 304, 334 };
            int[] dayOfWeekDiffMoon =
            {
                0, 3, 3, 6, 1, 4,
                6, 2, 5, 0, 3, 5
            };

            // Bütün gün farklarına ayın kaçıncı günü olduğunu ekleyelim
            // Artık yılda bulunuyorsak o fazlalığıda ekleyelim
            // Bu şekilde haftanın kaçıncı günü olduğunu sadeleştirmiş şekilde bulabiliyoruz.
            int leapYearDiff = month > 2 && IsLeap(year) ? 1 : 0;
            int result = (
                             dayOfWeekDiff +
                             dayOfWeekDiffMoon[month - 1] +
                             day - 1 +
                             leapYearDiff
                         ) % 7;

            // Bu bulduğumuz algoritmayi biraz daha sadeleştirdiğimiz zaman
            // Sakamoto'nun haftanın günü algoritmasını bulabilirsiniz
            // https://en.wikipedia.org/wiki/Determination_of_the_day_of_the_week#Implementation-dependent_methods

            return result;
        }

        /// <summary>
        ///     Verilen tarihe istenilen gün sayısını ekler ve değerler eklenmiş olarak geri döner
        /// </summary>
        /// <param name="addDay">Eklenecek gün</param>
        /// <param name="year">Yıl</param>
        /// <param name="month">Ay</param>
        /// <param name="day">Gün</param>
        private static void AddDay(int addDay, ref int year, ref int month, ref int day)
        {
            int[,] days =
            {
                {31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31},
                {31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31}
            };

            day += addDay;

            // Ayın en son günü için iki boyutlu dize oluşturduk eğer artık yıl varsa ikinci dizideki günü alıyor
            while (day > days[IsLeap(year) ? 1 : 0, month - 1])
            {
                // 7 gün eklediğimizde eğer o ayın en son gününden fazla ise bir sonraki aya geçiyor
                day -= days[IsLeap(year) ? 1 : 0, month - 1];
                month += 1;
                // 12 ayın üzerindeki rakamları yıl olarak ekliyor
                if (month > 12)
                {
                    year += 1;
                    month = 1;
                }
            }
        }
    }
}