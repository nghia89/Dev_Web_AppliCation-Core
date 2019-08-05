using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCore.Extensions
{
	public class RelativeDate
	{
		public static string RelativeDateTime(DateTime theDate)
		{
			Dictionary<long,string> thresholds = new Dictionary<long,string>();
			int minute = 60;
			int hour = 60 * minute;
			int day = 24 * hour;
			thresholds.Add(60,"{0} Cách đây vài giây");
			thresholds.Add(minute * 2,"phút trước");
			thresholds.Add(45 * minute,"{0} một vài phút trước");
			thresholds.Add(120 * minute,"giờ trước");
			thresholds.Add(day,"{0} giờ trước");
			thresholds.Add(day * 2,"hôm qua");
			thresholds.Add(day * 30,"{0} ngày trước");
			thresholds.Add(day * 365,"{0} tháng trước");
			thresholds.Add(long.MaxValue,"{0} Năm trước");
			long since = (DateTime.Now.Ticks - theDate.Ticks) / 10000000;
			foreach(long threshold in thresholds.Keys)
			{
				if(since < threshold)
				{
					TimeSpan t = new TimeSpan((DateTime.Now.Ticks - theDate.Ticks));
					return string.Format(thresholds[threshold],(t.Days > 365 ? t.Days / 365 : (t.Days > 0 ? t.Days : (t.Hours > 0 ? t.Hours : (t.Minutes > 0 ? t.Minutes : (t.Seconds > 0 ? t.Seconds : 0))))).ToString());
				}
			}
			return "";
		}
	}
}
